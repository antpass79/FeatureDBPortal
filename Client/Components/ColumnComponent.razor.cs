using FeatureDBPortal.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FeatureDBPortal.Client.Components
{
    public class ColumnComponentDataModel : ComponentBase
    {
        [Parameter]
        public Column Column { get; set; }
    }
}