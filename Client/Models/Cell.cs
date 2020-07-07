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
            var classValue = AllowMode switch
            {
                Models.AllowMode.A => BACKGROUND_MODE_AVAILABLE,
                Models.AllowMode.Def => BACKGROUND_MODE_DEFAULT,
                Models.AllowMode.No => BACKGROUND_MODE_NO,
                _ => BACKGROUND_MODE_NULL
            };

            classValue = IsSelected ? SELECTED : classValue;

            classValue += this.IsActive ? $" {ACTIVE}" : string.Empty;

            return classValue;
        }
    }
}
