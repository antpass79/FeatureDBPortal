using AutoMapper;
using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Server.Options;
using FeatureDBPortal.Server.Repositories;
using FeatureDBPortal.Server.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
            // Configuration
            var databaseOptions = new DatabaseOptions();
            _configuration.GetSection(nameof(DatabaseOptions)).Bind(databaseOptions);
            databaseOptions.DefaultSqlServerConnectionString = _configuration.GetConnectionString("DefaultSqlServerConnectionString");
            databaseOptions.DefaultSqliteConnectionString = _configuration.GetConnectionString("DefaultSqliteConnectionString");

            // Database
            _ = databaseOptions.DatabaseType switch
            {
                DatabaseType.SqlServer =>
                    services.AddDbContext<DbContext, FeaturesContext>(
                        options => options.UseSqlServer(databaseOptions.DefaultSqlServerConnectionString),
                        ServiceLifetime.Scoped),
                DatabaseType.Sqlite =>
                    services.AddDbContext<DbContext, FeaturesContext>(
                        options => options.UseSqlite(databaseOptions.DefaultSqliteConnectionString),
                        ServiceLifetime.Scoped),
                _ => throw new NotSupportedException("DbContext not supported")
            };

            // Services
            services
                .AddScoped<IGenericRepository<Application>, GenericRepository<Application>>()
                .AddScoped<IGenericRepository<Country>, GenericRepository<Country>>()
                .AddScoped<IGenericRepository<LogicalModel>, GenericRepository<LogicalModel>>()
                .AddScoped<IGenericRepository<MinorVersionAssociation>, GenericRepository<MinorVersionAssociation>>()
                .AddScoped<IGenericRepository<BiopsyKits>, GenericRepository<BiopsyKits>>()
                .AddScoped<IGenericRepository<Option>, GenericRepository<Option>>()
                .AddScoped<IGenericRepository<Probe>, GenericRepository<Probe>>();

            services
                .AddScoped<IAvailabilityCombinationService, AvailabilityCombinationService>();

            // Mapping
            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });
            services.AddRazorPages();
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

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
