using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Client.Components
{
    public class CombinationMatrixDataModel : ComponentBase
    {
        [Parameter]
        public CombinationDTO Combination { get; set; }

        protected IEnumerable<ColumnDTO> Columns =>
            Combination == null || Combination.Columns == null ? new List<ColumnDTO>() : Combination.Columns;
    }
}