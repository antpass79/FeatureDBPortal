using System.Collections.Generic;

namespace FeatureDBPortal.Client.Models
{
    public class Cell : BaseCell
    {
        public Cell()
        {
            RowId = -1;
            ColumnId = -1;
            Name = string.Empty;
            Available = null;
        }

        public string Name { get; set; }
        public bool? Available { get; set; }
        public bool? Visible { get; set; }
        private AllowMode? _allowMode;
        public AllowMode? AllowMode
        {
            get { return _allowMode; }
            set
            {
                _allowMode = value;
                UpdateClassValue();
            }
        }
        public string AggregateItems { get; set; }
        public IEnumerable<CellItem> Items { get; set; }

        public int? RowId { get; set; }
        public int? ColumnId { get; set; }

        override protected string OnUpdateClassValue()
        {
            var classValue = this.AllowMode == Models.AllowMode.A ? "available-background" : "no-background";
            classValue += this.IsHighlight ? " highlight" : string.Empty;

            return classValue;
        }
    }
}
