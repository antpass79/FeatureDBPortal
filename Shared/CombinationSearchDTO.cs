namespace FeatureDBPortal.Shared
{
    public class CombinationSearchDTO
    {
        public int? ApplicationId { get; set; }
        public int? ProbeId { get; set; }
        public int? CountryId { get; set; }
        public int? VersionId { get; set; }
        public int? ModelId { get; set; }
        public int? OptionId { get; set; }
        public int? KitId { get; set; }

        public UserLevelDTO UserLevel { get; set; }

        public LayoutTypeDTO RowLayout { get; set; }
        public LayoutTypeDTO ColumnLayout { get; set; }
        public LayoutTypeDTO CellLayout { get; set; }
    }
}
