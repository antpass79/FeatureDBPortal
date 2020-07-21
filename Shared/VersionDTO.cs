namespace FeatureDBPortal.Shared
{
    public class VersionDTO : IOrderablePropertyName, IQueryableItem
    {
        public int? Id { get; set; }
        public string BuildVersion { get; set; }

        string IOrderablePropertyName.OrderableProperty => BuildVersion;

        int? IQueryableItem.Id => Id;
        string IQueryableItem.Name => BuildVersion;
    }
}
