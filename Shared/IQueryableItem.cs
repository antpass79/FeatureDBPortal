namespace FeatureDBPortal.Shared
{
    public interface IQueryableItem
    {
        int? Id { get; }
        string Name { get; }
        bool IsFake
        {
            get { return false; }
        }
    }
}
