namespace FeatureDBPortal.Shared
{
    public class CountryDTO : IQueryableItem
    {
        public int? Id { get; set; }
        public string Name => $"{CountryName} ({Code})";
        public string CountryName { get; set; }
        public string Code { get; set; }
    }
}
