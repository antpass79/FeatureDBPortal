namespace FeatureDBPortal.Shared
{
    public class CsvExportSettingsDTO
    {
        public string FileName { get; set; }
        public string DefaultFileName { get; set; } = "combination";
        public string Separator { get; set; }
    }
}
