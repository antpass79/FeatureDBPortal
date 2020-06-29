using System.Collections.Generic;

namespace FeatureDBPortal.Client.Models
{
    public class CombinationCell
    {
        public CombinationCell()
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
        public IEnumerable<CombinationItem> Items { get; set; }

        public int? RowId { get; set; }
        public int? ColumnId { get; set; }

        private bool _isHighlight;
        public bool IsHighlight
        {
            get { return _isHighlight; }
            set
            {
                _isHighlight = value;
                UpdateClassValue();
            }
        }
        public string ClassValue { get; private set; }

        private void UpdateClassValue()
        {
            this.ClassValue = this.AllowMode == Models.AllowMode.A ? "available-background" : "no-background";
            this.ClassValue += this.IsHighlight ? " highlight" : string.Empty;
        }
    }
}
