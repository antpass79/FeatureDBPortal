using FeatureDBPortal.Server.Models;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Providers
{
    public interface IVersionProvider
    {
        int Min { get; }
        int Max { get; }

        IEnumerable<int> VersioNumbers { get; }
        IEnumerable<IQueryableCombination> Versions { get; }
    }
}
