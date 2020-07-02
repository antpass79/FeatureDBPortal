using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RA
{
    public partial class TestRdFeatures
    {
        public TestRdFeatures()
        {
            TestRdFeatureAvail = new HashSet<TestRdFeatureAvail>();
        }

        public string TfeaCode { get; set; }
        public string TfeaName { get; set; }
        public string TfeaCategory { get; set; }
        public DateTime? TfeaTimestamp { get; set; }
        public string TfeaUser { get; set; }

        public virtual ICollection<TestRdFeatureAvail> TestRdFeatureAvail { get; set; }
    }
}
