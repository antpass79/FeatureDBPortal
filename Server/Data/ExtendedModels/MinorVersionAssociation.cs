using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Data.Models
{
    public partial class MinorVersionAssociation : IQueryableCombination
    {
        public string Name => BuildVersion;
    }
}
