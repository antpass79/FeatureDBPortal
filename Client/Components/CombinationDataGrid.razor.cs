using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Components;

namespace FeatureDBPortal.Client.Components
{
    public class CombinationDataGridDataModel : ComponentBase
    {
        [Parameter]
        public CombinationDTO Combination { get; set; }
    }
}