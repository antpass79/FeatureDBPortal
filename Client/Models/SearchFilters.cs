using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Client.Models
{
    public class SearchFilters
    {
        public ApplicationDTO Application { get; set; } = new ApplicationDTO();
        public ProbeDTO Probe { get; set; } = new ProbeDTO();
        public CountryDTO Country { get; set; } = new CountryDTO();
        public VersionDTO Version { get; set; } = new VersionDTO();
        public ModelDTO Model { get; set; } = new ModelDTO();
        public OptionDTO Option { get; set; } = new OptionDTO();
        public KitDTO Kit { get; set; } = new KitDTO();
        public UserLevelDTO UserLevel { get; set; }
        public LayoutTypeDTO RowLayout { get; set; }
        public LayoutTypeDTO ColumnLayout { get; set; }
        public LayoutTypeDTO CellLayout { get; set; }
    }
}
