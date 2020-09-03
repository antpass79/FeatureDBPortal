using FeatureDBPortal.Client.Extensions;
using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Client.Services;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Pages
{
    public class FeaturesDataModel : ComponentBase, IDisposable
    {
        const string BUTTON_ACTION_EXPORT_TO_CSV = "BUTTON_ACTION_EXPORT_TO_CSV";
        const string BUTTON_ACTION_SYNC_RA = "BUTTON_ACTION_SYNC_RA";

        [Inject]
        protected IFilterService FilterService { get; set; }
        [Inject]
        protected IAvailabilityCombinationService AvailabilityCombinationService { get; set; }
        [Inject]
        protected ToolbarButtonsService ButtonsService { get; set; }
        [Inject]
        protected ICsvExportService CsvExportService { get; set; }

        protected bool FiltersBusy { get; set; }
        protected bool CombinationsBusy { get; set; }

        bool _filtersOpened = true;
        protected bool FiltersOpened
        {
            get => _filtersOpened;
            set
            {
                _filtersOpened = value;
                CombinationContainerClass = FiltersOpened ? "feature-combination-filters-opened" : "feature-combination-filters-closed";
            }
        }
        protected bool KeepOpen { get; set; }
        protected string CombinationContainerClass { get; set; } = "feature-combination-filters-opened";

        protected bool ShowCsvExportDialog { get; set; }
        protected CsvExportSettingsDTO CsvExportSettings = new CsvExportSettingsDTO();

        protected IEnumerable<ApplicationDTO> Applications { get; set; }
        protected ApplicationDTO SelectedApplication { get; private set; } = new ApplicationDTO();

        protected IEnumerable<ProbeDTO> Probes { get; set; }
        protected ProbeDTO SelectedProbe { get; private set; } = new ProbeDTO();

        protected IEnumerable<CountryDTO> Countries { get; set; }
        protected CountryDTO SelectedCountry { get; private set; } = new CountryDTO();

        protected IEnumerable<VersionDTO> Versions { get; set; }
        protected VersionDTO SelectedVersion { get; private set; } = new VersionDTO();

        protected IEnumerable<ModelDTO> Models { get; set; }
        protected ModelDTO SelectedModel { get; set; } = new ModelDTO();

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

        protected IEnumerable<LayoutTypeDTO> LayoutViews { get; set; }
        protected LayoutTypeDTO SelectedRowLayout { get; set; }
        string _selectedRowLayoutText;
        public string SelectedRowLayoutText
        {
            get => _selectedRowLayoutText;
            set
            {
                _selectedRowLayoutText = value;
                if (Enum.TryParse<LayoutTypeDTO>(_selectedRowLayoutText, out var layoutType))
                {
                    SelectedRowLayout = layoutType;
                }
            }
        }

        protected LayoutTypeDTO SelectedColumnLayout { get; set; }
        string _selectedColumnLayoutText;
        public string SelectedColumnLayoutText
        {
            get => _selectedColumnLayoutText;
            set
            {
                _selectedColumnLayoutText = value;
                if (Enum.TryParse<LayoutTypeDTO>(_selectedColumnLayoutText, out var layoutType))
                {
                    SelectedColumnLayout = layoutType;
                }
            }
        }

        protected LayoutTypeDTO SelectedCellLayout { get; set; }
        string _selectedCellLayoutText;
        public string SelectedCellLayoutText
        {
            get => _selectedCellLayoutText;
            set
            {
                _selectedCellLayoutText = value;
                if (Enum.TryParse<LayoutTypeDTO>(_selectedCellLayoutText, out var layoutType))
                {
                    SelectedCellLayout = layoutType;
                }
            }
        }

        protected Combination Combination { get; private set; }

        public CombinationFilters Filters = new CombinationFilters
        {
            KeepIfIdNotNull = true,
            KeepIfCellModeNotNull = true,
            KeepIfCellModeA = true,
            KeepIfCellModeDef = true,
            KeepIfCellModeNo = true
        };

        protected LayoutTypeDTO CurrentHeader { get; set; }

        protected bool DisableApplication => SelectedRowLayout == LayoutTypeDTO.Application || SelectedColumnLayout == LayoutTypeDTO.Application || SelectedCellLayout == LayoutTypeDTO.Application;
        protected bool DisableProbe => SelectedRowLayout == LayoutTypeDTO.Probe || SelectedColumnLayout == LayoutTypeDTO.Probe || SelectedCellLayout == LayoutTypeDTO.Probe;
        protected bool DisableModel => SelectedRowLayout == LayoutTypeDTO.Model || SelectedColumnLayout == LayoutTypeDTO.Model || SelectedCellLayout == LayoutTypeDTO.Model;
        protected bool DisableKit => SelectedRowLayout == LayoutTypeDTO.Kit || SelectedColumnLayout == LayoutTypeDTO.Kit || SelectedCellLayout == LayoutTypeDTO.Kit;
        protected bool DisableOption => SelectedRowLayout == LayoutTypeDTO.Option || SelectedColumnLayout == LayoutTypeDTO.Option || SelectedCellLayout == LayoutTypeDTO.Option;
        protected bool DisableVersion => SelectedRowLayout == LayoutTypeDTO.Version || SelectedColumnLayout == LayoutTypeDTO.Version || SelectedCellLayout == LayoutTypeDTO.Version;
        protected bool DisableCountry => SelectedRowLayout == LayoutTypeDTO.Country || SelectedColumnLayout == LayoutTypeDTO.Country || SelectedCellLayout == LayoutTypeDTO.Country;
        protected bool DisableUserLevel => SelectedRowLayout == LayoutTypeDTO.UserLevel || SelectedColumnLayout == LayoutTypeDTO.UserLevel || SelectedCellLayout == LayoutTypeDTO.UserLevel;

        private bool IsOutputLayoutTypeSelected(LayoutTypeDTO layoutType) => SelectedRowLayout == layoutType || SelectedColumnLayout == layoutType || SelectedCellLayout == layoutType;


        async protected override Task OnInitializedAsync()
        {
            FiltersBusy = true;

            BuildToolbar();

            Applications = await FilterService.GetApplicationsAsync();
            Probes = await FilterService.GetProbesAsync();
            Countries = await FilterService.GetCountriesAsync();
            Versions = await FilterService.GetVersionsAsync();
            Models = await FilterService.GetModelsAsync();
            Options = await FilterService.GetOptionsAsync();
            Kits = await FilterService.GetKitsAsync();
            UserLevels = Enum.GetValues(typeof(UserLevelDTO)).Cast<UserLevelDTO>().OrderBy(item => item.ToString());
            LayoutViews = Enum.GetValues(typeof(LayoutTypeDTO)).Cast<LayoutTypeDTO>();

            SelectedModel = Models.FirstOrDefault();
            SelectedCountry = Countries.FirstOrDefault();
            SelectedUserLevel = UserLevels.FirstOrDefault();
            SelectedUserLevelText = SelectedUserLevel.ToString();

            FiltersBusy = false;
        }

        async protected Task OnSearch()
        {
            using var watcher = new Watcher("CLIENT-SERVER ROUNDTRIP");

            CombinationsBusy = true;

            var combinationDTO = await AvailabilityCombinationService.GetCombinations(new CombinationSearchDTO
            {
                Application = IsOutputLayoutTypeSelected(LayoutTypeDTO.Application) ? null : SelectedApplication,
                Probe = IsOutputLayoutTypeSelected(LayoutTypeDTO.Probe) ? null : SelectedProbe,
                Country = IsOutputLayoutTypeSelected(LayoutTypeDTO.Country)  ? null : SelectedCountry,
                Version = IsOutputLayoutTypeSelected(LayoutTypeDTO.Version) ? null : SelectedVersion,
                Model = IsOutputLayoutTypeSelected(LayoutTypeDTO.Model) ? null : SelectedModel,
                Option = IsOutputLayoutTypeSelected(LayoutTypeDTO.Option) ? null : SelectedOption,
                Kit = IsOutputLayoutTypeSelected(LayoutTypeDTO.Kit) ? null : SelectedKit,
                UserLevel = IsOutputLayoutTypeSelected(LayoutTypeDTO.UserLevel) ? UserLevelDTO.None : SelectedUserLevel,
                RowLayout = SelectedRowLayout,
                ColumnLayout = SelectedColumnLayout,
                CellLayout = SelectedCellLayout
            });

            Combination combination;

            using (var innerWatcher = new Watcher("ToModel"))
            {
                combination = combinationDTO.ToModel();
            }

            using (var innerWatcher = new Watcher("ApplyFilters"))
            {
                combination.ApplyFilters(Filters);
            }

            Combination = combination;

            CurrentHeader = SelectedRowLayout;

            CombinationsBusy = false;

            FiltersOpened = KeepOpen || false;

            StateHasChanged();
        }

        private void BuildToolbar()
        {
            ButtonsService.Actions.Add(new ButtonAction
            {
                Id = BUTTON_ACTION_EXPORT_TO_CSV,
                Label = "Export to CSV",
                IconName = "import_export"
            });
            ButtonsService.Actions.Add(new ButtonAction
            {
                Id = BUTTON_ACTION_SYNC_RA,
                Label = "Sync RA",
                IconName = "sync"
            });

            ButtonsService.FireAction = (action) =>
            {
                switch (action.Id)
                {
                    case BUTTON_ACTION_EXPORT_TO_CSV:
                        ShowCsvExportDialog = true;
                        StateHasChanged();
                        break;
                    case BUTTON_ACTION_SYNC_RA:
                        break;
                }
            };
        }

        public void Dispose()
        {
            ButtonsService.Actions.Clear();
        }

        async protected Task OnDownload()
        {
            ShowCsvExportDialog = false;
            await CsvExportService.DownloadCsv(new CsvExportDTO
            {
                Combination = Combination.ToDTO(),
                Settings = CsvExportSettings
            });
        }
    }
}
