namespace FeatureDBPortal.Client.Models
{
    public class Column : BaseCell
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        protected override string OnUpdateClassValue()
        {
            var classValue = IsSelected ? SELECTED : string.Empty;
            return this.IsActive ? ACTIVE : classValue;
        }
    }
}
