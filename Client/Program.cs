using FeatureDBPortal.Client.Components;
using FeatureDBPortal.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
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

            builder.Services.AddSingleton<SpinnerService>();

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddTransient<IFilterService, FilterService>();
            builder.Services.AddTransient<IAvailabilityCombinationService, AvailabilityCombinationService>();

            await builder.Build().RunAsync();
        }
    }
}
