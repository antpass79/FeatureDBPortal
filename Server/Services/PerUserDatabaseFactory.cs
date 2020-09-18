using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Options;
using FeatureDBPortal.Server.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Security.Claims;

namespace FeatureDBPortal.Server.Services
{
    public class PerUserDatabaseFactory : IPerUserFeaturesDatabaseFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAsyncPerUserDatabaseService _perUserDatabaseService;

        public PerUserDatabaseFactory(
            IHttpContextAccessor httpContextAccessor,
            IAsyncPerUserDatabaseService perUserDatabaseService)
        {
            _httpContextAccessor = httpContextAccessor;
            _perUserDatabaseService = perUserDatabaseService;
        }

        public FeaturesContext Create(DatabaseOptions databaseOptions)
        {
            var databaseConnection = BuildDatabaseConnection();
            var context = BuildContext(databaseOptions, databaseConnection);

            return context;
        }

        private string BuildDatabaseConnection()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var database = _perUserDatabaseService[userName];
            var databaseConnection = database;

            if (database != FeaturesPortalConstants.DEFAULT_DATABASE_NAME)
            {
                var databasesFolder = Path.Combine(Directory.GetCurrentDirectory(), FeaturesPortalConstants.DATABASES_FOLDER);
                databaseConnection = $"Data Source={databasesFolder}\\{database};";
            }

            return databaseConnection;
        }

        private FeaturesContext BuildContext(DatabaseOptions databaseOptions, string databaseConnection)
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<FeaturesContext>();

            if (databaseConnection == FeaturesPortalConstants.DEFAULT_DATABASE_NAME)
            {
                _ = databaseOptions.DatabaseType switch
                {
                    DatabaseType.SqlServer =>
                            contextOptionsBuilder.UseSqlServer(databaseOptions.DefaultSqlServerConnectionString),
                    DatabaseType.Sqlite =>
                            contextOptionsBuilder.UseSqlite(databaseOptions.DefaultSqliteConnectionString),
                    _ => throw new NotSupportedException("DbContext not supported")
                };
            }
            else
            {
                contextOptionsBuilder.UseSqlite(databaseConnection);
            }

            return new FeaturesContext(contextOptionsBuilder.Options);
        }
    }
}
