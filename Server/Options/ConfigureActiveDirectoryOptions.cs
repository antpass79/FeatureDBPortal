using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FeatureDBPortal.Server.Options
{
    public class ConfigureActiveDirectoryOptions : IConfigureOptions<ActiveDirectoryOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public ConfigureActiveDirectoryOptions(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        public void Configure(ActiveDirectoryOptions options)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var activeDirectoryOptions = new ActiveDirectoryOptions();
                _configuration.GetSection(nameof(ActiveDirectoryOptions)).Bind(activeDirectoryOptions);

                options.Domain = activeDirectoryOptions.Domain;
            }
        }
    }
}
