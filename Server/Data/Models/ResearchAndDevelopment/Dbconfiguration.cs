using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class Dbconfiguration
    {
        public int Id { get; set; }
        public int Compatibility { get; set; }
        public int Version { get; set; }
    }
}
