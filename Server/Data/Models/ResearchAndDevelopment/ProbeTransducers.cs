using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class ProbeTransducers
    {
        public int Id { get; set; }
        public int ProbeId { get; set; }
        public short TransducerType { get; set; }
        public short? TransducerPosition { get; set; }

        public virtual Probe Probe { get; set; }
    }
}
