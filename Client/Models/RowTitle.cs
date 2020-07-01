namespace FeatureDBPortal.Client.Models
{
    public class RowTitle : BaseCell
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        protected override string OnUpdateClassValue()
        {
            return this.IsHighlight ? "highlight" : string.Empty;
        }
    }
}
