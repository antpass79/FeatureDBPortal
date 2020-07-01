using FeatureDBPortal.Client.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace FeatureDBPortal.Client.Components
{
    public class HeaderComponentDataModel : ComponentBase
    {
        [Parameter]
        public string IntersectionTitle { get; set; }

        [Parameter]
        public IEnumerable<Column> Columns { get; set; }
    }
}
