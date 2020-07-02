using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Extensions;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
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

        private readonly ICombinationGroupService _combinationGroupByAnyService;
        private readonly ICombinationGroupService _combinationGroupByOneService;
        private readonly ICombinationGroupService _combinationGroupByTwoService;
        private readonly ICombinationGroupService _combinationGroupByThreeService;

        public AvailabilityCombinationService(IMapper mapper, DbContext context)
        {
            _mapper = mapper;
            _context = context as FeaturesContext;

            _combinationGroupByAnyService = new CombinationGroupByAnyService(context);
            _combinationGroupByOneService = new CombinationGroupByOneService(context);
            _combinationGroupByTwoService = new CombinationGroupByTwoService(context);
            _combinationGroupByThreeService = new CombinationGroupByThreeService(context);
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
                    0 => await _combinationGroupByAnyService.Combine(search, groupBy),
                    1 => await _combinationGroupByOneService.Combine(search, groupBy),
                    2 => await _combinationGroupByTwoService.Combine(search, groupBy),
                    3 => await _combinationGroupByThreeService.Combine(search, groupBy),
                    _ => throw new NotImplementedException()
                };

                return combination;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return await Task.FromResult<CombinationDTO>(null);
            }
            finally
            {
                Trace.WriteLine(string.Empty);
                Trace.WriteLine($"ON SERVER: Process starts at {start} and stops at {DateTime.Now} with duration of {stopwatch.Elapsed}");
            }
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

        #endregion
    }
}
