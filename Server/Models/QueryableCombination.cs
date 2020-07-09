namespace FeatureDBPortal.Server.Models
{
    public class QueryableCombination : IQueryableCombination
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFake { get; }
    }
}
