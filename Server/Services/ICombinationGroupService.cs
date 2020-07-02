﻿using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public interface ICombinationGroupService
    {
        Task<CombinationDTO> Combine(CombinationSearchDTO search, IEnumerable<LayoutType> groupBy);
    }
}
