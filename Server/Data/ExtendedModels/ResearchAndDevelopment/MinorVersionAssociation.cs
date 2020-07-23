using FeatureDBPortal.Server.Models;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class MinorVersionAssociation : IQueryableEntity
    {
        public string Name => BuildVersion;
    }
}
