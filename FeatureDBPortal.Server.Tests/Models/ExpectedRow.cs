using System.Collections.Generic;

namespace FeatureDBPortal.Server.Tests.Models
{
    public class ExpectedRow
    {
        public int? ForRowId { get; set; }

        public string? ExpectedName { get; set; }
        public int? ExpectedCellCount { get; set; }
    }
}