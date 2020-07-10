namespace FeatureDBPortal.Client.Models
{
    public class ButtonAction
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public bool Enabled { get; set; } = true;
        public string IconName { get; set; }
    }
}
