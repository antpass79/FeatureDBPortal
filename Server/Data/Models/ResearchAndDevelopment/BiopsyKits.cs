using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class BiopsyKits
    {
        public BiopsyKits()
        {
            NormalRule = new HashSet<NormalRule>();
            Uirule = new HashSet<Uirule>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? FeatureId { get; set; }

        public virtual Feature Feature { get; set; }
        public virtual ICollection<NormalRule> NormalRule { get; set; }
        public virtual ICollection<Uirule> Uirule { get; set; }
    }
}
