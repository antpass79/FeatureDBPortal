using FeatureDBPortal.Server.Models;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class Probe : IQueryableEntity
    {
        public string Name => SaleName;
    }
}
