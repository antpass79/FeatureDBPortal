using FeatureDBPortal.Server.Models;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class PhysicalModel : IQueryableEntity
    {
        string IQueryableEntity.Name => this.Description;
    }
}
