using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Providers
{
    public class VersionProvider : IVersionProvider
    {
        private readonly FeaturesContext _context;

        public VersionProvider(FeaturesContext context)
        {
            _context = context;
            Update();
        }

        int _min;
        public int Min => _min;

        int _max;
        public int Max => _max;

        IEnumerable<int> _versioNumbers;
        public IEnumerable<int> VersioNumbers => _versioNumbers;

        IEnumerable<IQueryableEntity> _versions;
        public IEnumerable<IQueryableEntity> Versions => _versions;

        #region Public Functions

        public void Update()
        {
            _min = _context.MinorVersionAssociation
                .Min(item => item.Major);
            _max = _context.MinorVersionAssociation
                .Max(item => item.Major);

            _versioNumbers = Enumerable
                .Range(Min, Max - Min + 1)
                .Select(version => GetNumericVersionFromMajor(version));

            _versions = _versioNumbers.Select(item => new QueryableEntity
            {
                Id = item,
                Name = BuildStringVersion(item)
            });
        }

        public int BuildDefaultVersion(int countryId, int modelId)
        {
            var countryVersion = _context.CountryVersion
                .Single(item => item.CountryId == countryId && item.LogicalModelId == modelId);

            return GetNumericVersionFromMajor(countryVersion.MajorVersion);
        }

        public string BuildStringVersion(int version)
        {
            // converts from format 60002 => 06.00.02
            if (version >= 10000)
            {
                var major = GetMajorFromNumericVersion(version);
                var middle = (version - major * 10000) / 100;
                var minor = version - middle * 100 - major * 10000;
                return BuildStringVersion(major, middle, minor);
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Private Functions

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
