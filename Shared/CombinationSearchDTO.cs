namespace FeatureDBPortal.Shared
{
    public class CombinationSearchDTO
    {
        public ApplicationDTO Application { get; set; }
        public ProbeDTO Probe { get; set; }
        public CountryDTO Country { get; set; }
        public VersionDTO Version { get; set; }
        public ModelDTO Model { get; set; }
        public OptionDTO Option { get; set; }
        public KitDTO Kit { get; set; }

        public UserLevelDTO UserLevel { get; set; }

        public LayoutTypeDTO RowLayout { get; set; }
        public LayoutTypeDTO ColumnLayout { get; set; }
        public LayoutTypeDTO CellLayout { get; set; }
    }
}
