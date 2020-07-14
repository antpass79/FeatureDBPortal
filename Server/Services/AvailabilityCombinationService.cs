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
                var groupCount = GetGroupCount(search);
                CombinationDTO combination = groupCount switch
                {
                    0 => await _combinationGroupByAnyService.GetCombination(search),
                    1 => await _combinationGroupByOneService.GetCombination(search),
                    2 => await _combinationGroupByTwoService.GetCombination(search),
                    3 => await _combinationGroupByThreeService.GetCombination(search),
                    _ => throw new NotImplementedException()
                };

                return combination;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                throw e;
            }
        }

        #region Private Functions

        private int GetGroupCount(CombinationSearchDTO search)
        {
            int count = 0;
            if (search.RowLayout != LayoutTypeDTO.None)
            {
                count++;
            }
            if (search.ColumnLayout != LayoutTypeDTO.None)
            {
                count++;
            }
            if (search.CellLayout != LayoutTypeDTO.None)
            {
                count++;
            }

            return count;
        }

        #endregion
    }
}
