namespace FeatureDBPortal.Shared
{
    public class ModelDTO : IOrderablePropertyName, IQueryableItem
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        string IOrderablePropertyName.OrderableProperty => this.Name;

        int? IQueryableItem.Id => Id;
        string IQueryableItem.Name => Name;
    }
}
