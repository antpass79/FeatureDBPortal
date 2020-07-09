using FeatureDBPortal.Server.Models;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class MinorVersionAssociation : IQueryableCombination
    {
        public string Name => BuildVersion;
    }
}
