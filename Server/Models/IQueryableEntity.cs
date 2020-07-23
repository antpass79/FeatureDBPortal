namespace FeatureDBPortal.Server.Models
{
    public interface IQueryableEntity
    {
        int Id { get; }
        string Name { get; }
        bool IsFake
        {
            get { return false; }
        }
    }
}
