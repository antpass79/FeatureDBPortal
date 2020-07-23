using FeatureDBPortal.Server.Data.Models.RD;

namespace FeatureDBPortal.Server.Models
{
    public class QueryableEntity : IQueryableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFake { get; }
    }
}
