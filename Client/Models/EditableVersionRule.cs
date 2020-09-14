namespace FeatureDBPortal.Client.Models
{
    public class EditableVersionRule
    {
        public int? Id { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int? Patch { get; set; }
        public int? BuildVersion { get; set; }

        public bool Valid
        {
            get
            {
                return Major.HasValue && Minor.HasValue && Patch.HasValue && BuildVersion.HasValue;
            }
        }

        public bool Invalid
        {
            get
            {
                return !Valid;
            }
        }
    }
}
