using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Client.Models
{
    public class EditableRequirementRule
    {
        public int? IncrementalVersion { get; set; }
        public OptionDTO Option { get; set; }
        public ApplicationDTO Application { get; set; }
        public ProbeDTO Probe { get; set; }
        public KitDTO Kit { get; set; }
        public ModelDTO Model { get; set; }
        public PhysicalModelDTO PhysicalModel { get; set; }

        public bool Valid
        {
            get
            {
                return IncrementalVersion.HasValue;
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
