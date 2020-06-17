﻿using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Components;

namespace FeatureDBPortal.Client.Components
{
    public class CombinationMatrixDataModel : ComponentBase
    {
        [Parameter]
        public CombinationDTO Combination { get; set; }
    }
}