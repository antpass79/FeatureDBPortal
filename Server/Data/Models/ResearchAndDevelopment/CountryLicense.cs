﻿using System;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class CountryLicense
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int? DistributorId { get; set; }
        public int LicenseId { get; set; }

        public virtual Country Country { get; set; }
        public virtual Distributor Distributor { get; set; }
        public virtual License License { get; set; }
    }
}
