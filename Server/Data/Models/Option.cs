using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models
{
    public partial class Option
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFake { get; set; }
        public bool IsPreset { get; set; }
        public int FeatureId { get; set; }

        public virtual Feature Feature { get; set; }
    }
}
