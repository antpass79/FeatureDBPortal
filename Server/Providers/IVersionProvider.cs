using FeatureDBPortal.Server.Models;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Providers
{
    public interface IVersionProvider
    {
        int Min { get; }
        int Max { get; }

        IEnumerable<int> VersioNumbers { get; }
        IEnumerable<IQueryableEntity> Versions { get; }
        void Update();
        int BuildDefaultVersion(int countryId, int modelId);
        string BuildStringVersion(int version);
    }
}
