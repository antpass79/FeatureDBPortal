namespace FeatureDBPortal.Shared
{
    public class CertifierDTO : IOrderablePropertyName
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        string IOrderablePropertyName.OrderableProperty => this.Name;
    }
}
