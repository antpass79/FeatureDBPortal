namespace FeatureDBPortal.Client.Models
{
    abstract public class BaseCell
    {
        protected const string BACKGROUND_MODE_NULL = "background-mode-null";
        protected const string BACKGROUND_MODE_AVAILABLE = "background-mode-available";
        protected const string BACKGROUND_MODE_DEFAULT = "background-mode-default";
        protected const string BACKGROUND_MODE_NO = "background-mode-no";
        protected const string ACTIVE = "cell-active";
        protected const string SELECTED = "cell-selected";

        protected BaseCell()
        {
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                UpdateClassValue();
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                UpdateClassValue();
            }
        }

        public string ClassValue { get; private set; }

        virtual protected string OnUpdateClassValue()
        {
            return string.Empty;
        }

        protected void UpdateClassValue()
        {
            ClassValue = OnUpdateClassValue();
        }
    }
}
