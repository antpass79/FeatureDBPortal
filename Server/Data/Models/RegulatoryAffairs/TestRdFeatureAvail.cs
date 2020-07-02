using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RA
{
    public partial class TestRdFeatureAvail
    {
        public string TfavFeaturecode { get; set; }
        public decimal TfavMajorversion { get; set; }
        public string TfavModelcode { get; set; }
        public string TfavFeaturePartnumber { get; set; }
        public string TfavFeatureBosName { get; set; }
        public DateTime? TfavTimestamp { get; set; }
        public string TfavUser { get; set; }

        public virtual TestRdFeatures TfavFeaturecodeNavigation { get; set; }
        public virtual TestRdVersions TfavMajorversionNavigation { get; set; }
        public virtual TestRdModels TfavModelcodeNavigation { get; set; }
    }
}
