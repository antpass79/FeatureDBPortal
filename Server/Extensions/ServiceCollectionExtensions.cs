using FeatureDBPortal.Server.Options;
using FeatureDBPortal.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FeatureDBPortal.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRuntimeDbContext<TContext>(this IServiceCollection services, IConfiguration configuration)
            where TContext : DbContext, new()
        {
            // Configuration
            var databaseOptions = new DatabaseOptions();
            configuration.GetSection(nameof(DatabaseOptions)).Bind(databaseOptions);
            databaseOptions.DefaultSqlServerConnectionString = configuration.GetConnectionString("DefaultSqlServerConnectionString");
            databaseOptions.DefaultSqliteConnectionString = configuration.GetConnectionString("DefaultSqliteConnectionString");

            if (!databaseOptions.PerUser)
            {
                services.AddDefaultDbContext<TContext>(databaseOptions);
            }
            else
            {
                services.AddPerUserDbContext<TContext>(databaseOptions);
            }

            return services;
        }

        public static IServiceCollection AddDefaultDbContext<TContext>(this IServiceCollection services, DatabaseOptions databaseOptions)
            where TContext : DbContext
        {
            _ = databaseOptions.DatabaseType switch
            {
                DatabaseType.SqlServer =>
                    services.AddDbContext<TContext>(
                        options => options.UseSqlServer(databaseOptions.DefaultSqlServerConnectionString),
                        ServiceLifetime.Scoped),
                DatabaseType.Sqlite =>
                    services.AddDbContext<TContext>(
                        options => options.UseSqlite(databaseOptions.DefaultSqliteConnectionString),
                        ServiceLifetime.Scoped),
                _ => throw new NotSupportedException("DbContext not supported")
            };

            return services;
        }

        public static IServiceCollection AddPerUserDbContext<TContext>(this IServiceCollection services, DatabaseOptions databaseOptions)
            where TContext : DbContext, new()
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IPerUserFeaturesDatabaseFactory, PerUserDatabaseFactory>();
            services.AddScoped(provider =>
            {
                return ResolveDbContext<TContext>(provider, databaseOptions);
            });
            return services;
        }

        static TContext ResolveDbContext<TContext>(IServiceProvider provider, DatabaseOptions databaseOptions)
            where TContext : DbContext
        {
            var perUserDatabaseFactory = provider.GetService<IPerUserFeaturesDatabaseFactory>();
            return perUserDatabaseFactory.Create(databaseOptions) as TContext;
        }
    }
}
