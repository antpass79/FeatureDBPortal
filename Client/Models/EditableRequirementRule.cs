using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Client.Models
{
    public class EditableRequirementRule
    {
        public int? IncrementalVersion { get; set; }
        public OptionDTO Option { get; set; } = new OptionDTO();
        public ApplicationDTO Application { get; set; } = new ApplicationDTO();
        public ProbeDTO Probe { get; set; } = new ProbeDTO();
        public KitDTO Kit { get; set; } = new KitDTO();
        public ModelDTO Model { get; set; } = new ModelDTO();
        public PhysicalModelDTO PhysicalModel { get; set; } = new PhysicalModelDTO();

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
