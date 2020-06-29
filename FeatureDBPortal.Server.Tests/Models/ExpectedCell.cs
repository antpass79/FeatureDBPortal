using FeatureDBPortal.Shared;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Tests.Models
{
    public class ExpectedCell
    {
        public int? ForRowId { get; set; }
        public int? ForColumnId { get; set; }
        public string ForRowName { get; set; }
        public string ForColumnName { get; set; }

        public AllowModeDTO? ExpectedAllowMode { get; set; }
        public bool? ExpectedVisibility { get; set; }
        public bool? ExpectedAvailability { get; set; }
        public string ExpectedName { get; set; }
    }
}