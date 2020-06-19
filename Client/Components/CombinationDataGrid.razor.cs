using FeatureDBPortal.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FeatureDBPortal.Client.Components
{
    public class CombinationDataGridDataModel : ComponentBase
    {
        [Parameter]
        public Combination Combination { get; set; }
    }
}