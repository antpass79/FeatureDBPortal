using Microsoft.AspNetCore.Builder;

namespace FeatureDBPortal.Server.ActiveDirectory
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAdMiddleware(this IApplicationBuilder builder) =>
            builder.UseMiddleware<AdUserMiddleware>();
    }
}
