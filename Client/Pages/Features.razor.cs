using FeatureDBPortal.Client.Components;
using FeatureDBPortal.Client.Services;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Pages
{
    public class FeaturesDataModel : ComponentBase
    {
        [Inject]
        protected IFilterService FilterService { get; set; }
        [Inject]
        protected IAvailabilityCombinationService AvailabilityCombinationService { get; set; }

        protected bool FiltersBusy { get; set; }

        protected IEnumerable<ApplicationDTO> Applications { get; set; }
        protected ApplicationDTO SelectedApplication { get; private set; } = new ApplicationDTO();

        protected IEnumerable<ProbeDTO> Probes { get; set; }
        protected ProbeDTO SelectedProbe { get; private set; } = new ProbeDTO();

        protected IEnumerable<CountryDTO> Countries { get; set; }
        protected CountryDTO SelectedCountry { get; private set; } = new CountryDTO();

        protected IEnumerable<VersionDTO> Versions { get; set; }
        protected VersionDTO SelectedVersion { get; private set; } = new VersionDTO();

        protected IEnumerable<ModelDTO> Models { get; set; }
        protected ModelDTO SelectedModel { get; private set; } = new ModelDTO();

        protected IEnumerable<OptionDTO> Options { get; set; }
        protected OptionDTO SelectedOption { get; private set; } = new OptionDTO();

        protected IEnumerable<KitDTO> Kits { get; set; }
        protected KitDTO SelectedKit { get; private set; } = new KitDTO();

        protected IEnumerable UserLevels { get; set; }
        protected UserLevelDTO SelectedUserLevel { get; set; }

        protected IEnumerable LayoutViews { get; set; }
        protected LayoutTypeDTO SelectedRowLayout { get; set; }
        protected LayoutTypeDTO SelectedColumnLayout { get; set; }
        protected LayoutTypeDTO SelectedCellLayout { get; set; }

        protected override async Task OnInitializedAsync()
        {
            FiltersBusy = true;

            Applications = await FilterService.GetApplications();
            Probes = await FilterService.GetProbes();
            Countries = await FilterService.GetCountries();
            Versions = await FilterService.GetVersions();
            Models = await FilterService.GetModels();
            Options = await FilterService.GetOptions();
            Kits = await FilterService.GetKits();
            UserLevels = Enum.GetValues(typeof(UserLevelDTO));
            LayoutViews = Enum.GetValues(typeof(LayoutTypeDTO));

            SelectedModel = Models.FirstOrDefault();
            SelectedCountry = Countries.FirstOrDefault();
            SelectedUserLevel = UserLevels.Cast<UserLevelDTO>().FirstOrDefault();

            FiltersBusy = false;
        }

        async protected Task OnSearch()
        {
            var combinations = await AvailabilityCombinationService.GetCombinations(new CombinationSearchDTO
            {
                Application = SelectedApplication,
                Probe = SelectedProbe,
                Country = SelectedCountry,
                Version = SelectedVersion,
                Model = SelectedModel,
                Option = SelectedOption,
                Kit = SelectedKit,
                UserLevel = SelectedUserLevel,
                RowLayout = SelectedRowLayout,
                ColumnLayout = SelectedColumnLayout,
                CellLayout = SelectedCellLayout
            });
        }
    }
}
