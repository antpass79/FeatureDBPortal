namespace FeatureDBPortal.Shared
{
    public class OptionDTO : IQueryableItem
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsFake { get; set; }
    }
}
