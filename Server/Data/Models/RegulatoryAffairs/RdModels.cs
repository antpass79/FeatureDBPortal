using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RA
{
    public partial class RdModels
    {
        public RdModels()
        {
            RdFeatureAvailabilities = new HashSet<RdFeatureAvailabilities>();
        }

        public string ModCode { get; set; }
        public string ModName { get; set; }
        public DateTime? ModTimestamp { get; set; }
        public string ModUser { get; set; }
        public string ModType { get; set; }

        public virtual ICollection<RdFeatureAvailabilities> RdFeatureAvailabilities { get; set; }
    }
}
