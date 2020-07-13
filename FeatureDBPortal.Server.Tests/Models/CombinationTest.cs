using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Tests.Models
{
    public class CombinationTest
    {
        public string Title { get; set; }

        public LayoutType? FirstGroup { get; set; }
        public LayoutType? SecondGroup { get; set; }
        public LayoutType? ThirdGroup { get; set; }

        public int? ModelId { get; set; }
        public int? CountryId { get; set; }
        public int? ProbeId { get; set; }
        public int? OptionId { get; set; }
        public int? ApplicationId { get; set; }
        public int? KitId { get; set; }
        public int? VersionId { get; set; }
        public UserLevelDTO User { get; set; } = UserLevelDTO.None;

        public ExpectedResult Result { get; set; }
    }
}
