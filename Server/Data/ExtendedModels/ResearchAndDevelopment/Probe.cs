using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Data.Models.RD
{
    public partial class Probe : IQueryableCombination
    {
        public string Name => SaleName;
    }
}
