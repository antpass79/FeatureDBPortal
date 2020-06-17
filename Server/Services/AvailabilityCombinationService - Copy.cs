//using AutoMapper;
//using FeatureDBPortal.Server.Data;
//using FeatureDBPortal.Server.Data.Models;
//using FeatureDBPortal.Server.Extensions;
//using FeatureDBPortal.Shared;
//using Microsoft.AspNetCore.Mvc.Formatters;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using SQLitePCL;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FeatureDBPortal.Server.Services
//{
//    public class AvailabilityCombinationService1 : IAvailabilityCombinationService
//    {
//        private readonly FeaturesContext _context;
//        private readonly IMapper _mapper;

//        public AvailabilityCombinationService1(IMapper mapper, DbContext context)
//        {
//            _mapper = mapper;
//            _context = context as FeaturesContext;
//        }

//        async public Task<CombinationDTO> Get(CombinationSearchDTO search)
//        {
//            try
//            {
//                IEnumerable<LayoutType> groupBy = GetGroups(search);
//                CombinationDTO combination = groupBy.Count() switch
//                {
//                    0 => await GroupByAny(search),
//                    1 => await GroupByOne(search, groupBy),
//                    2 => await GroupByTwo(search, groupBy),
//                    3 => await GroupByThree(search, groupBy),
//                    _ => throw new NotImplementedException()
//                };
//                return combination;
//            }
//            catch (Exception e)
//            {
//                return await Task.FromResult<CombinationDTO>(null);
//            }
//        }

//        async public Task<CombinationDTO> GroupByAny(CombinationSearchDTO search)
//        {
//            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);
//            if (normalRules.Count() == 0)
//                throw new ArgumentOutOfRangeException();

//            var allow = normalRules.All(normalRule => normalRule.Allow != 0);

//            return await Task.FromResult(new CombinationDTO
//            {
//                Description = string.Empty,
//                Allow = allow,
//                GroupBy = GroupByDTO.Any
//            });
//        }

//        async public Task<CombinationDTO> GroupByOne(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy)
//        {
//            var firstLayoutGroup = groupBy.ElementAt(0);
//            var selectedRowField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup.ToString());

//            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);

//            // Maybe add firstLayoutGroup + "Id" == null
//            var groups = normalRules
//                .GroupBy(normalRule => normalRule.GetPropertyValue<int?>(firstLayoutGroup + "Id"));

//            var nullGroup = groups.SingleOrDefault(group => group.Key == null);

//            var combinations = groups
//                .Select(group => new CombinationDTO
//                {
//                    Description = selectedRowField.Single(item => item.Id == group.Key).Name,
//                    // If there aren't rules?
//                    Allow = nullGroup == null ? group.All(normalRule => normalRule.Allow != 0) : group.Union(nullGroup).All(normalRule => normalRule.Allow != 0),
//                })
//                .OrderBy(combination => combination.Description)
//                .ToList();

//            var combination = new CombinationDTO()
//            {
//                Description = firstLayoutGroup.ToString(),
//                Combinations = combinations,
//                GroupBy = GroupByDTO.One,
//                Columns = new List<ColumnDTO>
//                {
//                    new ColumnDTO
//                    {
//                        Name = "Allow"
//                    }
//                }
//            };

//            return await Task.FromResult(combination);
//        }

//        async public Task<CombinationDTO> GroupByTwo(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy)
//        {
//            var firstLayoutGroup = groupBy.ElementAt(0);
//            var secondLayoutGroup = groupBy.ElementAt(1);

//            var selectedRowField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup.ToString());
//            var selectedColumnField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(secondLayoutGroup.ToString());

//            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);

//            var firstGroups = normalRules
//                .Join(selectedRowField, a => a.GetPropertyValue<int?>(firstLayoutGroup + "Id"), b => b.Id, (a, b) => new
//                {
//                    Name = b.Name,
//                    Rule = a,
//                })
//                .ToList()
//                .GroupBy(item => item.Name)
//                .Select(secondGroup => new
//                {
//                    Description = secondGroup.Key,
//                    Combinations = secondGroup.Select(group => new
//                    {
//                        Description = selectedColumnField.SingleOrDefault(p => p.Id == group.Rule.GetPropertyValue<int?>(secondLayoutGroup + "Id"))?.Name,
//                        Allow = secondGroup.All(item => item.Rule.Allow != 0)
//                    }).ToList()
//                });

//            var combination = new CombinationDTO()
//            {
//                Columns = selectedColumnField.ToList().Select(item => new ColumnDTO
//                {
//                    Id = item.Id,
//                    Name = item.Name
//                })
//                .OrderBy(item => item.Name),
//                Combinations = firstGroups.Select(group => new CombinationDTO
//                {
//                    Description = group.Description,
//                    Combinations = group.Combinations.Select(item => new CombinationDTO
//                    {
//                        Description = item.Description,
//                        Allow = item.Allow
//                    })
//                    .OrderBy(item => item.Description)
//                })
//                .OrderBy(item => item.Description),
//                GroupBy = GroupByDTO.Two
//            };

//            return await Task.FromResult(combination);
//        }

//        async public Task<CombinationDTO> GroupByThree(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy)
//        {
//            var firstLayoutGroup = groupBy.ElementAt(0);
//            var secondLayoutGroup = groupBy.ElementAt(1);
//            var thirdLayoutGroup = groupBy.ElementAt(2);

//            var selectedRowField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(firstLayoutGroup.ToString());
//            var selectedColumnField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(secondLayoutGroup.ToString());
//            var selectedellField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(thirdLayoutGroup.ToString());

//            IEnumerable<NormalRule> normalRules = FilterNormalRules(search);

//            var firstGroups = normalRules
//                .AsEnumerable()
//                .Join(selectedRowField, a => a.ApplicationId, b => b.Id, (a, b) => new
//                {
//                    Name = b.Name,
//                    Rule = a,
//                })
//                .ToList()
//                .GroupBy(item => item.Name)
//                .Select(secondGroup => new
//                {
//                    Description = secondGroup.Key,
//                    Probes = secondGroup.Select(group => new
//                    {
//                        Description = selectedColumnField.SingleOrDefault(item => item.Id == group.Rule.ProbeId)?.Name,
//                        Versions = secondGroup.GroupBy(element => element.Rule.Version).Select(thirdGroup => new
//                        {
//                            Description = thirdGroup.Key,
//                            Allow = thirdGroup.All(i => i.Rule.Allow != 0)
//                        })
//                    })
//                });

//            var combination = new CombinationDTO()
//            {
//                Combinations = firstGroups.Select(group => new CombinationDTO
//                {
//                    Description = group.Description,
//                    Combinations = group.Probes.Select(item => new CombinationDTO
//                    {
//                        Description = item.Description,
//                        Allow = item.Versions.All(element => element.Allow)
//                    })
//                }),
//                GroupBy = GroupByDTO.Three
//            };

//            return await Task.FromResult(combination);
//        }

//        #region Private Functions

//        private IEnumerable<LayoutType> GetGroups(CombinationSearchDTO search)
//        {
//            List<LayoutType> groupBy = new List<LayoutType>();
//            if (search.RowLayout != LayoutTypeDTO.None)
//            {
//                groupBy.Add(_mapper.Map<LayoutType>(search.RowLayout));
//            }
//            if (search.ColumnLayout != LayoutTypeDTO.None)
//            {
//                groupBy.Add(_mapper.Map<LayoutType>(search.ColumnLayout));
//            }
//            if (search.CellLayout != LayoutTypeDTO.None)
//            {
//                groupBy.Add(_mapper.Map<LayoutType>(search.CellLayout));
//            }

//            return groupBy;
//        }

//        private IEnumerable<NormalRule> FilterNormalRules(CombinationSearchDTO search)
//        {
//            return _context
//                .NormalRule
//                    .WhereIf(item => item.LogicalModelId == search.Model.Id || !item.LogicalModelId.HasValue, search.Model != null)
//                    .WhereIf(item => item.CountryId == search.Country.Id || !item.CountryId.HasValue, search.Country != null)
//                    .WhereIf(item => item.UserLevel == (short)search.UserLevel || !item.UserLevel.HasValue, search.UserLevel != UserLevelDTO.None)
//                    .WhereIf(item => item.ProbeId == search.Probe.Id || !item.ProbeId.HasValue, search.Probe != null)
//                    .WhereIf(item => item.KitId == search.Kit.Id || !item.KitId.HasValue, search.Kit != null)
//                    .WhereIf(item => item.OptionId == search.Option.Id || !item.OptionId.HasValue, search.Option != null)
//                    .WhereIf(item => item.ApplicationId == search.Application.Id || !item.ApplicationId.HasValue, search.Application != null)
//                .ToList();
//        }

//        #endregion
//    }
//}
