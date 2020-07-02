using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class Bundle
    {
        public int Id { get; set; }
        public int ParentFeatureId { get; set; }
        public int FeatureId { get; set; }

        public virtual Feature Feature { get; set; }
        public virtual Feature ParentFeature { get; set; }
    }
}
