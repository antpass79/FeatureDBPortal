using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RA
{
    public partial class RdFeatureAvailabilities
    {
        public string FavFeaturecode { get; set; }
        public decimal FavMajorversion { get; set; }
        public string FavModelcode { get; set; }
        public string FavFeaturePartnumber { get; set; }
        public string FavFeatureBosName { get; set; }
        public DateTime? FavTimestamp { get; set; }
        public string FavUser { get; set; }

        public virtual RdFeatures FavFeaturecodeNavigation { get; set; }
        public virtual RdVersions FavMajorversionNavigation { get; set; }
        public virtual RdModels FavModelcodeNavigation { get; set; }
    }
}
