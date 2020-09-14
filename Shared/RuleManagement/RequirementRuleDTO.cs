namespace FeatureDBPortal.Shared.RuleManagement
{
    public class RequirementRuleDTO
    {
        public VersionDTO Version { get; set; }
        public OptionDTO Option { get; set; }
        public ApplicationDTO Application { get; set; }
        public ProbeDTO Probe { get; set; }
        public KitDTO Kit { get; set; }
        public ModelDTO Model { get; set; }
        public PhysicalModelDTO PhysicalModel { get; set; }
    }
}