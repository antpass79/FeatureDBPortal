namespace FeatureDBPortal.Shared
{
    public class DistributorDTO : IOrderablePropertyName
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        string IOrderablePropertyName.OrderableProperty => this.Name;
    }
}
