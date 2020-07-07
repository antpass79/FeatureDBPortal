namespace FeatureDBPortal.Client.Models
{
    public class CombinationFilters
    {
        public bool KeepIfIdNotNull { get; set; }
        public bool KeepIfCellModeNotNull { get; set; }
        public bool KeepIfCellModeA { get; set; }
        public bool KeepIfCellModeDef { get; set; }
        public bool KeepIfCellModeNo { get; set; }
        public string KeepIfRowTitleContains { get; set; }
        public string KeepIfColumnTitleContains { get; set; }
    }
}
