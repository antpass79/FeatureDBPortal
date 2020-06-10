namespace FeatureDBPortal.Shared
{
    public class VersionDTO : IOrderablePropertyName
    {
        public int? Id { get; set; }
        public string BuildVersion { get; set; }

        string IOrderablePropertyName.OrderableProperty => this.BuildVersion;
    }
}
