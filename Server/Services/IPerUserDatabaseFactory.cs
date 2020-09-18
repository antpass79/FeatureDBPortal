using FeatureDBPortal.Server.Options;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FeatureDBPortal.Server.Services
{
    public interface IPerUserDatabaseFactory<TContext>
        where TContext : DbContext
    {
        TContext Create(DatabaseOptions databaseOptions);
    }
}
