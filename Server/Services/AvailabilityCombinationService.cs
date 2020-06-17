using AutoMapper;
using FeatureDBPortal.Client.Extensions;
using FeatureDBPortal.Server.Data;
using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class AvailabilityCombinationService : IAvailabilityCombinationService
    {
        private readonly FeaturesContext _context;
        private readonly IMapper _mapper;

        public AvailabilityCombinationService(IMapper mapper, DbContext context)
        {
            _mapper = mapper;
            _context = context as FeaturesContext;
        }

        async public Task<CombinationDTO> Get(CombinationSearchDTO search)
        {
            var start = DateTime.Now;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                IEnumerable<LayoutType> groupBy = GetGroups(search);
                CombinationDTO combination = groupBy.Count() switch
                {
                    0 => await GroupByAny(search),
                    1 => await GroupByOne(search, groupBy),
                    2 => await GroupByTwo(search, groupBy),
                    3 => await GroupByThree(search, groupBy),
                    _ => throw new NotImplementedException()
                };
                return combination;
            }
            catch (Exception e)
            {
                return await Task.FromResult<CombinationDTO>(null);
            }
            finally
            {
                Trace.WriteLine(string.Empty);
                Trace.WriteLine($"Process starts at {start} and stops at {DateTime.Now} with duration of {stopwatch.Elapsed}");
            }
        }

        async public Task<CombinationDTO> GroupByAny(CombinationSearchDTO search)
        {
            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);
            if (normalRules.Count() == 0)
                throw new ArgumentOutOfRangeException();

            var allow = normalRules.All(normalRule => normalRule.Allow != 0);

            var matrix = new MatrixDTO();
            var row = new MatrixRowDTO();
            row.Columns.Add(-1, new CellDTO() { RowId = -1, ColumnId = -1, Allow = allow });
            matrix.Rows.Add(-1, row);

            var combination = new CombinationDTO
            {
                Headers = new List<TitleDTO>
                {
                    new TitleDTO { Id = -1, Name = "Allow" },
                },
                Rows = matrix.Rows.Values.Select(item => new RowDTO
                {
                    TitleCell = new CellDTO(),
                    Cells = item.Columns.Select(innerItem => new CellDTO
                    {
                        Allow = innerItem.Value.Allow
                    }).ToList()
                }).ToList(),
                GroupBy = GroupByDTO.Any
            };

            return await Task.FromResult(combination);
        }

        async public Task<CombinationDTO> GroupByOne(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy)
        {
            var firstLayoutGroup = groupBy.ElementAt(0);
            var selectedRowField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup.ToString());

            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);

            // Maybe add firstLayoutGroup + "Id" == null
            var groups = normalRules
                .GroupBy(normalRule => normalRule.GetPropertyValue<int?>(firstLayoutGroup + "Id"));

            var nullGroup = groups.SingleOrDefault(group => group.Key == null);

            var orderedSelectedRowField = selectedRowField
                .ToList()
                .OrderBy(item => item.Name)
                .ToList();

            var matrix = new MatrixDTO();
            orderedSelectedRowField
                .ForEach(rowItem =>
                {
                    var row = new MatrixRowDTO() { Name = rowItem.Name };
                    matrix.Rows.Add(rowItem.Id, row);
                });

            groups
                .ToList()
                .ForEach(group =>
                {
                    matrix.Rows[group.Key].Columns.Add(group.Key, new CellDTO
                    {
                        RowId = group.Key,
                        ColumnId = group.Key,
                        Allow = nullGroup == null ? group.All(normalRule => normalRule.Allow != 0) : group.Union(nullGroup).All(normalRule => normalRule.Allow != 0)
                    });
                });

            var combination = new CombinationDTO
            {
                Headers = new List<TitleDTO>
                {
                    new TitleDTO { Id = -1, Name = firstLayoutGroup.ToString() },
                    new TitleDTO { Id = -1, Name = "Allow" }
                },
                Rows = matrix.ToRows(),
                GroupBy = GroupByDTO.One
            };

            return await Task.FromResult(combination);
        }

        async public Task<CombinationDTO> GroupByTwo(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy)
        {
            var firstLayoutGroup = groupBy.ElementAt(0);
            var secondLayoutGroup = groupBy.ElementAt(1);

            var selectedRowField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup.ToString());
            var selectedColumnField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(secondLayoutGroup.ToString());

            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);

            var groups = normalRules
                .GroupBy(normalRule => normalRule.GetPropertyValue<int?>(firstLayoutGroup + "Id"))
                .Select(group => new
                {
                    RowId = group.Key,
                    RowName = selectedRowField.SingleOrDefault(item => item.Id == group.Key)?.Name,
                    Combinations = group.Select(groupItem => new
                    {
                        RowId = group.Key,
                        Allow = group.All(item => item.Allow != 0),
                        ColumnId = groupItem.GetPropertyValue<int?>(secondLayoutGroup + "Id")
                    }).ToList()
                });

            var orderedSelectedRowField = selectedRowField
                .ToList()
                .OrderBy(item => item.Name)
                .ToList();
            var orderedSelectedColumnField = selectedColumnField
                .ToList()
                .OrderBy(item => item.Name)
                .ToList();

            var matrix = new MatrixDTO();
            orderedSelectedRowField
                .ForEach(rowItem =>
                {
                    var row = new MatrixRowDTO();
                    row.Name = rowItem.Name;
                    orderedSelectedColumnField
                    .ForEach(columnItem =>
                    {
                        row.Columns.Add(columnItem.Id, new CellDTO
                        {
                            RowId = rowItem.Id,
                            ColumnId = columnItem.Id
                        });
                    });
                    matrix.Rows.Add(rowItem.Id, row);
                });

            for (var x = 0; x < groups.Count(); x++)
            {
                var rowA = groups.ElementAt(x);
                for (var y = 0; y < rowA.Combinations.Count(); y++)
                {
                    var columnA = rowA.Combinations.ElementAt(y);

                    var rowKey = columnA.RowId.HasValue ? columnA.RowId : -1;
                    var columnKey = columnA.ColumnId.HasValue ? columnA.ColumnId : -1;

                    if (matrix.Rows.ContainsKey(rowKey))
                    {
                        var selectedRow = matrix.Rows[rowKey];

                        if (selectedRow.Columns.ContainsKey(columnKey))
                        {
                            matrix.Rows[rowKey].Columns[columnKey] = new CellDTO
                            {
                                RowId = rowKey,
                                ColumnId = columnKey,
                                Allow = columnA.Allow
                            };
                        }
                        else
                        {
                            matrix.Rows[rowKey].Columns.Add(columnKey,  new CellDTO
                            {
                                RowId = rowKey,
                                ColumnId = columnKey,
                                Allow = columnA.Allow
                            });
                        }
                    }
                    else
                    {
                        var newRow = new MatrixRowDTO();
                        newRow.Columns.Add(columnKey,  new CellDTO()
                        {
                            RowId = rowKey,
                            ColumnId = columnKey,
                            Allow = columnA.Allow
                        });
                        matrix.Rows.Add(rowKey, newRow);
                    }
                }
            }

            var combination = new CombinationDTO
            {
                Headers = selectedColumnField.Select(item => new TitleDTO { Id = item.Id, Name = item.Name }),
                Rows = matrix.ToRows(),
                GroupBy = GroupByDTO.Two
            };

            return await Task.FromResult(combination);
        }

        async public Task<CombinationDTO> GroupByThree(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy)
        {
            var firstLayoutGroup = groupBy.ElementAt(0);
            var secondLayoutGroup = groupBy.ElementAt(1);
            var thirdLayoutGroup = groupBy.ElementAt(2);

            var selectedRowField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup.ToString());
            var selectedColumnField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(secondLayoutGroup.ToString());
            var selectedellField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(thirdLayoutGroup.ToString());

            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);

            var firstGroups = normalRules
                .AsEnumerable()
                .Join(selectedRowField, a => a.ApplicationId, b => b.Id, (a, b) => new
                {
                    Name = b.Name,
                    Rule = a,
                })
                .ToList()
                .GroupBy(item => item.Name)
                .Select(secondGroup => new
                {
                    Description = secondGroup.Key,
                    Probes = secondGroup.Select(group => new
                    {
                        Description = selectedColumnField.SingleOrDefault(item => item.Id == group.Rule.ProbeId)?.Name,
                        Versions = secondGroup.GroupBy(element => element.Rule.Version).Select(thirdGroup => new
                        {
                            Description = thirdGroup.Key,
                            Allow = thirdGroup.All(i => i.Rule.Allow != 0)
                        })
                    })
                });

            //var combination = new CombinationDTO()
            //{
            //    Combinations = firstGroups.Select(group => new CombinationDTO
            //    {
            //        Description = group.Description,
            //        Combinations = group.Probes.Select(item => new CombinationDTO
            //        {
            //            Description = item.Description,
            //            Allow = item.Versions.All(element => element.Allow)
            //        })
            //    }),
            //    GroupBy = GroupByDTO.Three
            //};

            return await Task.FromResult(new CombinationDTO());
        }

        #region Private Functions

        private IEnumerable<LayoutType> GetGroups(CombinationSearchDTO search)
        {
            List<LayoutType> groupBy = new List<LayoutType>();
            if (search.RowLayout != LayoutTypeDTO.None)
            {
                groupBy.Add(_mapper.Map<LayoutType>(search.RowLayout));
            }
            if (search.ColumnLayout != LayoutTypeDTO.None)
            {
                groupBy.Add(_mapper.Map<LayoutType>(search.ColumnLayout));
            }
            if (search.CellLayout != LayoutTypeDTO.None)
            {
                groupBy.Add(_mapper.Map<LayoutType>(search.CellLayout));
            }

            return groupBy;
        }

        private IEnumerable<NormalRule> FilterNormalRules(CombinationSearchDTO search)
        {
            return _context
                .NormalRule
                    .WhereIf(item => item.LogicalModelId == search.Model.Id || !item.LogicalModelId.HasValue, search.Model != null)
                    .WhereIf(item => item.CountryId == search.Country.Id || !item.CountryId.HasValue, search.Country != null)
                    .WhereIf(item => item.UserLevel == (short)search.UserLevel || !item.UserLevel.HasValue, search.UserLevel != UserLevelDTO.None)
                    .WhereIf(item => item.ProbeId == search.Probe.Id || !item.ProbeId.HasValue, search.Probe != null)
                    .WhereIf(item => item.KitId == search.Kit.Id || !item.KitId.HasValue, search.Kit != null)
                    .WhereIf(item => item.OptionId == search.Option.Id || !item.OptionId.HasValue, search.Option != null)
                    .WhereIf(item => item.ApplicationId == search.Application.Id || !item.ApplicationId.HasValue, search.Application != null)
                .ToList();
        }

        #endregion
    }
}
