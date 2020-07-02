using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RA
{
    public partial class TestRdModels
    {
        public TestRdModels()
        {
            TestRdFeatureAvail = new HashSet<TestRdFeatureAvail>();
        }

        public string TmodCode { get; set; }
        public string TmodName { get; set; }
        public DateTime? TmodTimestamp { get; set; }
        public string TmodUser { get; set; }
        public string TmodType { get; set; }

        public virtual ICollection<TestRdFeatureAvail> TestRdFeatureAvail { get; set; }
    }
}
