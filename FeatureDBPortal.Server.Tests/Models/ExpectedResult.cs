using System.Collections.Generic;

namespace FeatureDBPortal.Server.Tests.Models
{
    public class ExpectedResult
    {
        public int? ExpectedHeaderCount { get; set; }
        public int? ExpectedRowCount { get; set; }

        public FindBy FilterBy { get; set; }

        public IEnumerable<ExpectedHeader> ExpectedHeaders { get; set; }
        public IEnumerable<ExpectedRow> ExpectedRows { get; set; }
        public IEnumerable<ExpectedCell> ExpectedCells { get; set; }
        public IEnumerable<ExpectedItem> ExpectedItems { get; set; }
    }
}
