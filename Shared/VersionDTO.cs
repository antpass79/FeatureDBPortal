namespace FeatureDBPortal.Shared
{
    public class VersionDTO : IQueryableItem
    {
        public int? Id { get; set; }
        public string BuildVersion { get; set; }

        string IQueryableItem.Name => BuildVersion;
    }
}
