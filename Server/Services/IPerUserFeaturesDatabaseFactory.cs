using FeatureDBPortal.Server.Data.Models.RD;

namespace FeatureDBPortal.Server.Services
{
    public interface IPerUserFeaturesDatabaseFactory : IPerUserDatabaseFactory<FeaturesContext>
    {
    }
}
