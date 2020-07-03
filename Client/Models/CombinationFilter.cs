namespace FeatureDBPortal.Client.Models
{
    public class CombinationFilter
    {
        public bool KeepIfIdNotNull { get; set; }
        public bool KeepIfCellAllowModeNotNull { get; set; }
        public string KeepIfRowTitleContains { get; set; }
        public string KeepIfColumnTitleContains { get; set; }
    }
}
