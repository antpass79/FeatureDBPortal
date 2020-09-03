namespace FeatureDBPortal.Shared
{
    public class ModelFamilyDTO : IOrderablePropertyName
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        string IOrderablePropertyName.OrderableProperty => this.Name;
    }
}
