using FeatureDBPortal.Server.Models;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class Probe : IQueryableCombination
    {
        public string Name => SaleName;
    }
}
