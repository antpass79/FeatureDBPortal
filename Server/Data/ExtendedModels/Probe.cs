using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Data.Models
{
    public partial class Probe : IQueryableCombination
    {
        public string Name => SaleName;
    }
}
