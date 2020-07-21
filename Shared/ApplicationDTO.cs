namespace FeatureDBPortal.Shared
{
    public class ApplicationDTO : IOrderablePropertyName, IQueryableItem
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsFake { get; set; }

        string IOrderablePropertyName.OrderableProperty => this.Name;

        int? IQueryableItem.Id => Id;
        string IQueryableItem.Name => Name;
        bool IQueryableItem.IsFake => IsFake;
    }
}
