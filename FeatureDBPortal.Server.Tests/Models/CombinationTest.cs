using FeatureDBPortal.Server.Data.Models;
using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Tests.Models
{
    public class CombinationTest
    {
        public string Title { get; set; }

        public LayoutType? FirstGroup { get; set; }
        public LayoutType? SecondGroup { get; set; }
        public LayoutType? ThirdGroup { get; set; }

        public ModelDTO Model { get; set; } = new ModelDTO();
        public CountryDTO Country { get; set; } = new CountryDTO();
        public ProbeDTO Probe { get; set; } = new ProbeDTO();
        public OptionDTO Option { get; set; } = new OptionDTO();
        public ApplicationDTO Application { get; set; } = new ApplicationDTO();
        public KitDTO Kit { get; set; } = new KitDTO();
        public VersionDTO Version { get; set; } = new VersionDTO();
        public UserLevelDTO User { get; set; } = UserLevelDTO.None;

        public ExpectedResult Result { get; set; }
    }
}
