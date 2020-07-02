using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RA
{
    public partial class RdVersions
    {
        public RdVersions()
        {
            RdFeatureAvailabilities = new HashSet<RdFeatureAvailabilities>();
        }

        public decimal VerMajor { get; set; }
        public DateTime? VerTimestamp { get; set; }
        public string VerUser { get; set; }
        public string VerIslatest { get; set; }

        public virtual ICollection<RdFeatureAvailabilities> RdFeatureAvailabilities { get; set; }
    }
}
