using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.Utilities;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class AvailabilityCombinationService : IAvailabilityCombinationService
    {
        private readonly ICombinationGroupService _combinationGroupByAnyService;
        private readonly ICombinationGroupService _combinationGroupByOneService;
        private readonly ICombinationGroupService _combinationGroupByTwoService;
        private readonly ICombinationGroupService _combinationGroupByThreeService;

        public AvailabilityCombinationService(CombinationGroupServiceResolver combinationGroupServiceResolver)
        {
            _combinationGroupByAnyService = combinationGroupServiceResolver(CombinationGroupServiceTypes.COMBINATION_GROUP_BY_ANY);
            _combinationGroupByOneService = combinationGroupServiceResolver(CombinationGroupServiceTypes.COMBINATION_GROUP_BY_ONE);
            _combinationGroupByTwoService = combinationGroupServiceResolver(CombinationGroupServiceTypes.COMBINATION_GROUP_BY_TWO);
            _combinationGroupByThreeService = combinationGroupServiceResolver(CombinationGroupServiceTypes.COMBINATION_GROUP_BY_THREE);
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
