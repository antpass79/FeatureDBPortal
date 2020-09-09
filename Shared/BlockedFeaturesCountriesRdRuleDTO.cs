namespace FeatureDBPortal.Shared
{
    public class BlockedFeaturesCountriesRdRuleDTO
    {
        public OptionDTO Option { get; set; }
        public ApplicationDTO Application { get; set; }
        public ProbeDTO Probe { get; set; }
        public KitDTO Kit { get; set; }
        public ModelDTO Model { get; set; }
        public ModelFamilyDTO ModelFamily { get; set; }
        public CountryDTO Country { get; set; }
        public DistributorDTO Distributor { get; set; }
        public CertifierDTO Certifier { get; set; }
        public UserLevelDTO User { get; set; }
    }
}