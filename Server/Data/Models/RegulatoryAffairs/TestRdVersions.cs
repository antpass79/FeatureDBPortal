using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RA
{
    public partial class TestRdVersions
    {
        public TestRdVersions()
        {
            TestRdFeatureAvail = new HashSet<TestRdFeatureAvail>();
        }

        public decimal TverMajor { get; set; }
        public DateTime? TverTimestamp { get; set; }
        public string TverUser { get; set; }
        public string TverIslatest { get; set; }

        public virtual ICollection<TestRdFeatureAvail> TestRdFeatureAvail { get; set; }
    }
}
