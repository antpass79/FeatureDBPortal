using AutoMapper;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.Utilities;
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

        public AvailabilityCombinationService(IMapper mapper, DbContext context, IVersionProvider versionProvider)
        {
            _mapper = mapper;
            _context = context as FeaturesContext;

            _combinationGroupByAnyService = new CombinationGroupByAnyService(context, versionProvider);
            _combinationGroupByOneService = new CombinationGroupByOneService(context, versionProvider);
            _combinationGroupByTwoService = new CombinationGroupByTwoService(context, versionProvider);
            _combinationGroupByThreeService = new CombinationGroupByThreeService(context, versionProvider);
        }

        async public Task<CombinationDTO> Get(CombinationSearchDTO search)
        {
            using var watcher = new Watcher("ON SERVER");

            try
            {
                IEnumerable<LayoutType> groupBy = GetGroups(search);
                CombinationDTO combination = groupBy.Count() switch
                {
                    0 => await _combinationGroupByAnyService.GetCombination(search, groupBy),
                    1 => await _combinationGroupByOneService.GetCombination(search, groupBy),
                    2 => await _combinationGroupByTwoService.GetCombination(search, groupBy),
                    3 => await _combinationGroupByThreeService.GetCombination(search, groupBy),
                    _ => throw new NotImplementedException()
                };

                return combination;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return await Task.FromResult<CombinationDTO>(null);
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
