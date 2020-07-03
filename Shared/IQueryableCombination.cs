namespace FeatureDBPortal.Shared
{
    public interface IQueryableCombination
    {
        int Id { get; }
        string Name { get; }
        bool IsFake
        {
            get { return false; }
        }
    }
}
