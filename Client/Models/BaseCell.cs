namespace FeatureDBPortal.Client.Models
{
    abstract public class BaseCell
    {
        protected BaseCell()
        {
        }

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
