using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Utils;

namespace FeatureDBPortal.Server.Extensions
{
    public static class AllowModeExtensions
    {
        public static bool IsAvailable(this AllowMode allowMode)
        {
            return allowMode != AllowMode.No;
        }

        public static bool IsVisible(this AllowMode allowMode)
        {
            return allowMode == AllowMode.A;
        }
    }
}
