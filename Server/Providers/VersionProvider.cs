using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Server.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Providers
{
    public class VersionProvider : IVersionProvider
    {
        private readonly IGenericRepository<MinorVersionAssociation> _repository;

        public VersionProvider(IGenericRepository<MinorVersionAssociation> repository)
        {
            _repository = repository;
            Build();
        }

        int _min;
        public int Min => _min;

        int _max;
        public int Max => _max;

        IEnumerable<int> _versioNumbers;
        public IEnumerable<int> VersioNumbers => _versioNumbers;

        IEnumerable<IQueryableCombination> _versions;
        public IEnumerable<IQueryableCombination> Versions => _versions;

        #region Public Functions

        public void Build()
        {
            _min = _repository
                .Get()
                .Min(item => item.Major);
            _max = _repository
                .Get()
                .Max(item => item.Major);

            _versioNumbers = Enumerable
                .Range(Min, Max - Min + 1)
                .Select(version => GetNumericVersionFromMajor(version));

            _versions = _versioNumbers.Select(item => new QueryableCombination
            {
                Id = item,
                Name = GetStringVersion(item)
            });
        }

        #endregion

        #region Private Functions

        string GetStringVersion(int IntVersion)
        {
            // converts from format 60002 => 06.00.02
            if (IntVersion >= 10000)
            {
                var major = GetMajorFromNumericVersion(IntVersion);
                var middle = (IntVersion - major * 10000) / 100;
                var minor = IntVersion - middle * 100 - major * 10000;
                return BuildStringVersion(major, middle, minor);
            }
            else
            {
                return null;
            }
        }
        string BuildStringVersion(int? major, int? middle, int? minor)
        {
            return (major != null ? major.ToString().PadLeft(2, '0') : "") + "."
                   + (middle != null ? middle.ToString().PadLeft(2, '0') : "") + "."
                   + (minor != null ? minor.ToString().PadLeft(2, '0') : "");
        }

        int GetMajorFromNumericVersion(int IntVersion)
        {
            return IntVersion / 10000;
        }

        int GetNumericVersionFromMajor(double MajorVersion)
        {
            // converts from format 7
            return (int)(MajorVersion * 10000);
        }

        #endregion
    }
}
