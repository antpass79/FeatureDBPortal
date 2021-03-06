﻿using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class Probe
    {
        public Probe()
        {
            ProbePreset = new HashSet<ProbePreset>();
            ProbeSettingsFamily = new HashSet<ProbeSettingsFamily>();
            ProbeTransducers = new HashSet<ProbeTransducers>();
        }

        public int Id { get; set; }
        public string SaleName { get; set; }
        public string ProbeDescription { get; set; }
        public string HwCode { get; set; }
        public string ProbeStringCode { get; set; }
        public short? ProbeFamily { get; set; }
        public bool? MultiConnector { get; set; }
        public bool? Biplane { get; set; }
        public string TeeFamily { get; set; }
        public bool? Motorized { get; set; }
        public int? FeatureId { get; set; }

        public virtual Feature Feature { get; set; }
        public virtual ICollection<ProbePreset> ProbePreset { get; set; }
        public virtual ICollection<ProbeSettingsFamily> ProbeSettingsFamily { get; set; }
        public virtual ICollection<ProbeTransducers> ProbeTransducers { get; set; }
    }
}
