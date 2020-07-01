namespace FeatureDBPortal.Shared
{
    public class CountryDTO : IOrderablePropertyName
    {
        public int? Id { get; set; }
        public string Name => $"{CountryName} ({Code})";
        public string CountryName { get; set; }
        public string Code { get; set; }

        string IOrderablePropertyName.OrderableProperty => this.Name;
    }
}
