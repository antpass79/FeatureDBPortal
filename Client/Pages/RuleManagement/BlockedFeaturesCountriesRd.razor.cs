using FeatureDBPortal.Client.Services.RuleManagement;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.RuleManagement;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Pages.RuleManagement
{
    public class BlockedFeaturesCountriesRdDataModel : ComponentBase
    {
        #region Properties

        [Inject]
        protected IBlockedFeaturesCountriesRdRuleService RuleService { get; set; }

        protected bool FiltersBusy { get; set; }

        protected IEnumerable<ApplicationDTO> Applications { get; set; }
        protected ApplicationDTO SelectedApplication { get; private set; } = new ApplicationDTO();

        protected IEnumerable<CertifierDTO> Certifiers { get; set; }
        protected CertifierDTO SelectedCertifier { get; private set; } = new CertifierDTO();

        protected IEnumerable<ProbeDTO> Probes { get; set; }
        protected ProbeDTO SelectedProbe { get; private set; } = new ProbeDTO();

        protected IEnumerable<CountryDTO> Countries { get; set; }
        protected CountryDTO SelectedCountry { get; private set; } = new CountryDTO();

        protected IEnumerable<DistributorDTO> Distributors { get; set; }
        protected DistributorDTO SelectedDistributor { get; private set; } = new DistributorDTO();

        protected IEnumerable<VersionDTO> Versions { get; set; }
        protected VersionDTO SelectedVersion { get; private set; } = new VersionDTO();

        protected IEnumerable<ModelDTO> Models { get; set; }
        protected ModelDTO SelectedModel { get; set; } = new ModelDTO();

        protected IEnumerable<ModelFamilyDTO> ModelFamilies { get; set; }
        protected ModelFamilyDTO SelectedModelFamily { get; set; } = new ModelFamilyDTO();

        protected IEnumerable<OptionDTO> Options { get; set; }
        protected OptionDTO SelectedOption { get; private set; } = new OptionDTO();

        protected IEnumerable<KitDTO> Kits { get; set; }
        protected KitDTO SelectedKit { get; private set; } = new KitDTO();

        protected IEnumerable<UserLevelDTO> UserLevels { get; set; }
        protected UserLevelDTO SelectedUserLevel { get; set; }

        string _selectedUserLevelText;
        public string SelectedUserLevelText
        {
            get => _selectedUserLevelText;
            set
            {
                _selectedUserLevelText = value;
                if (Enum.TryParse<UserLevelDTO>(_selectedUserLevelText, out var userLevel))
                {
                    SelectedUserLevel = userLevel;
                }
            }
        }

        #endregion

        #region Protected Functions

        async protected override Task OnInitializedAsync()
        {
            FiltersBusy = true;

            Applications = await RuleService.GetApplicationsAsync();
            Certifiers = await RuleService.GetCertifiersAsync();
            Probes = await RuleService.GetProbesAsync();
            Countries = await RuleService.GetCountriesAsync();
            Distributors = await RuleService.GetDistributorsAsync();
            Models = await RuleService.GetModelsAsync();
            ModelFamilies = await RuleService.GetModelFamiliesAsync();
            Options = await RuleService.GetOptionsAsync();
            Kits = await RuleService.GetKitsAsync();
            UserLevels = Enum.GetValues(typeof(UserLevelDTO)).Cast<UserLevelDTO>().OrderBy(item => item.ToString());

            FiltersBusy = false;
        }

        async protected Task OnConfirm()
        {
            await RuleService.InsertAsync(new BlockedFeaturesCountriesRdRuleDTO
            {
                Application = SelectedApplication,
                Certifier = SelectedCertifier,
                Country = SelectedCountry,
                Distributor = SelectedDistributor,
                Kit = SelectedKit,
                Model = SelectedModel,
                ModelFamily = SelectedModelFamily,
                Option = SelectedOption,
                Probe = SelectedProbe,
                User = SelectedUserLevel
            });
        }

        #endregion
    }
}