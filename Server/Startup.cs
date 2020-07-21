using AutoMapper;
using FeatureDBPortal.Server.ActiveDirectory;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.gRPC;
using FeatureDBPortal.Server.Options;
using FeatureDBPortal.Server.Providers;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Server.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FeatureDBPortal.Server
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Options
            services
                .AddOptions()
                .Configure<JwtAuthenticationOptions>(options =>
                {
                    _configuration.GetSection(nameof(JwtAuthenticationOptions)).Bind(options);
                })
                .Configure<ActiveDirectoryOptions>(options =>
                {
                    _configuration.GetSection(nameof(ActiveDirectoryOptions)).Bind(options);
                });

            // Configuration
            var databaseOptions = new DatabaseOptions();
            _configuration.GetSection(nameof(DatabaseOptions)).Bind(databaseOptions);
            databaseOptions.DefaultSqlServerConnectionString = _configuration.GetConnectionString("DefaultSqlServerConnectionString");
            databaseOptions.DefaultSqliteConnectionString = _configuration.GetConnectionString("DefaultSqliteConnectionString");

            // Database
            _ = databaseOptions.DatabaseType switch
            {
                DatabaseType.SqlServer =>
                    services.AddDbContext<FeaturesContext>(
                        options => options.UseSqlServer(databaseOptions.DefaultSqlServerConnectionString),
                        ServiceLifetime.Scoped),
                DatabaseType.Sqlite =>
                    services.AddDbContext<FeaturesContext>(
                        options => options.UseSqlite(databaseOptions.DefaultSqliteConnectionString),
                        ServiceLifetime.Scoped),
                _ => throw new NotSupportedException("DbContext not supported")
            };

            // Feature Services
            services
                .AddScoped<IGenericRepository<Application>, GenericRepository<FeaturesContext, Application>>()
                .AddScoped<IGenericRepository<Country>, GenericRepository<FeaturesContext, Country>>()
                .AddScoped<IGenericRepository<LogicalModel>, GenericRepository<FeaturesContext, LogicalModel>>()
                .AddScoped<IGenericRepository<MinorVersionAssociation>, GenericRepository<FeaturesContext, MinorVersionAssociation>>()
                .AddScoped<IGenericRepository<BiopsyKits>, GenericRepository<FeaturesContext, BiopsyKits>>()
                .AddScoped<IGenericRepository<Option>, GenericRepository<FeaturesContext, Option>>()
                .AddScoped<IGenericRepository<Probe>, GenericRepository<FeaturesContext, Probe>>();
            services
                .AddSingleton<IFilterCache, FilterCache>();
            services
                .AddScoped<IVersionProvider, VersionProvider>()
                .AddScoped<IAsyncCsvService, CsvService>()
                .AddScoped<IAvailabilityCombinationService, AvailabilityCombinationService>();
            services
                .AddTransient<GroupProviderBuilder>()
                .AddTransient<CombinationGroupByAnyService>()
                .AddTransient<CombinationGroupByOneService>()
                .AddTransient<CombinationGroupByTwoService>()
                .AddTransient<CombinationGroupByThreeService>();
            services.AddTransient<CombinationGroupServiceResolver>(serviceProvider => key =>
            {
                ICombinationGroupService combinationGroupService = key switch
                {
                    CombinationGroupServiceTypes.COMBINATION_GROUP_BY_ANY => serviceProvider.GetService<CombinationGroupByAnyService>(),
                    CombinationGroupServiceTypes.COMBINATION_GROUP_BY_ONE => serviceProvider.GetService<CombinationGroupByOneService>(),
                    CombinationGroupServiceTypes.COMBINATION_GROUP_BY_TWO => serviceProvider.GetService<CombinationGroupByTwoService>(),
                    CombinationGroupServiceTypes.COMBINATION_GROUP_BY_THREE => serviceProvider.GetService<CombinationGroupByThreeService>(),
                    _ => throw new KeyNotFoundException()
                };

                return combinationGroupService;
            });

            // Active Directory
            services
                .AddScoped<IAsyncLoginService, ADLoginService>()
                .AddScoped<IUserProvider, AdUserProvider>()
                .AddSingleton<IJwtTokenEncoder<AdUser>, ADJwtTokenEncoder>()
                .AddSingleton<ISigningCredentialsBuilder, SigningCredentialsBuilder>()
                .AddSingleton<IConfigureOptions<JwtAuthenticationOptions>, ConfigureJwtAuthenticationOptions>()
                .AddSingleton<IConfigureOptions<ActiveDirectoryOptions>, ConfigureActiveDirectoryOptions>();

            // Mapping
            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });
            services.AddRazorPages();

            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Active Directory
            //app.UseAdMiddleware();

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapGrpcService<CombinationService>().EnableGrpcWeb();
                endpoints.MapGrpcService<FilterService>().EnableGrpcWeb();
            });
        }
    }
}
