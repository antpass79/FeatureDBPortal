using Blazored.LocalStorage;
using FeatureDBPortal.Client.Components;
using FeatureDBPortal.Client.Services;
using FeatureDBPortal.Client.Services.RuleManagement;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcCombination;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IGlobeDataStorage, GlobeLocalStorage>();
            builder.Services.AddScoped<ICsvExportService, CsvExportService>();
            builder.Services.AddSingleton<ToolbarButtonsService>();

            builder.Services.AddSingleton(services =>
            {
                // Create a gRPC-Web channel pointing to the backend server
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
                var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });

                // Now we can instantiate gRPC clients for this channel
                return new Combiner.CombinerClient(channel);
            });

            builder.Services.AddSingleton(services =>
            {
                // Create a gRPC-Web channel pointing to the backend server
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
                var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });

                // Now we can instantiate gRPC clients for this channel
                return new Filter.FilterClient(channel);
            });

            builder.Services.AddSingleton<SpinnerService>();

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddTransient<IFilterService, FilterService>();
            builder.Services.AddTransient<IAvailabilityCombinationService, AvailabilityCombinationService>();

            builder.Services.AddTransient<IBlockedFeaturesCountriesRdRuleService, BlockedFeaturesCountriesRdRuleService>();
            builder.Services.AddTransient<IMinorVersionRuleService, MinorVersionRuleService>();
            builder.Services.AddTransient<IRequirementRuleService, RequirementRuleService>();

            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

            await builder.Build().RunAsync();

            // Workaround: https://stackoverflow.com/questions/60793142/decoding-jwt-in-blazore-client-side-results-wasm-system-argumentexception-idx1
            _ = new JwtPayload();
        }
    }
}
