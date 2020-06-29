namespace FeatureDBPortal.Server.Tests.Models
{
    public class ExpectedItem
    {
        public int? ForRowId { get; set; }
        public int? ForColumnId { get; set; }
        public int? ForItemId { get; set; }
        public string ForRowName { get; set; }
        public string ForColumnName { get; set; }
        public string ForItemName { get; set; }

        public string ExpectedName { get; set; }
    }
}