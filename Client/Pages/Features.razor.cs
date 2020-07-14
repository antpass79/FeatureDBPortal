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
        protected string ErrorMessage { get; private set; }

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

        protected bool DisableApplication => IsOutputLayoutTypeSelected(LayoutTypeDTO.Application);
        protected bool DisableProbe => IsOutputLayoutTypeSelected(LayoutTypeDTO.Probe);
        protected bool DisableModel => IsOutputLayoutTypeSelected(LayoutTypeDTO.Model);
        protected bool DisableKit => IsOutputLayoutTypeSelected(LayoutTypeDTO.Kit);
        protected bool DisableOption => IsOutputLayoutTypeSelected(LayoutTypeDTO.Option);
        protected bool DisableVersion => IsOutputLayoutTypeSelected(LayoutTypeDTO.Version);
        protected bool DisableCountry => IsOutputLayoutTypeSelected(LayoutTypeDTO.Country);
        protected bool DisableUserLevel => IsOutputLayoutTypeSelected(LayoutTypeDTO.UserLevel);

        private bool IsOutputLayoutTypeSelected(LayoutTypeDTO layoutType) => SelectedRowLayout == layoutType || SelectedColumnLayout == layoutType || SelectedCellLayout == layoutType;


        protected override async Task OnInitializedAsync()
        {
            FiltersBusy = true;

            BuildToolbar();

            Applications = await FilterService.GetApplications();
            Probes = await FilterService.GetProbes();
            Countries = await FilterService.GetCountries();
            Versions = await FilterService.GetVersions();
            Models = await FilterService.GetModels();
            Options = await FilterService.GetOptions();
            Kits = await FilterService.GetKits();
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
            ErrorMessage = string.Empty;
            Combination = null;

            using var watcher = new Watcher("CLIENT-SERVER ROUNDTRIP");

            CombinationsBusy = true;

            try
            {
                var combinationDTO = await AvailabilityCombinationService.GetCombinations(new CombinationSearchDTO
                {
                    ApplicationId = IsOutputLayoutTypeSelected(LayoutTypeDTO.Application) ? null : SelectedApplication.Id,
                    ProbeId = IsOutputLayoutTypeSelected(LayoutTypeDTO.Probe) ? null : SelectedProbe.Id,
                    CountryId = IsOutputLayoutTypeSelected(LayoutTypeDTO.Country) ? null : SelectedCountry.Id,
                    VersionId = IsOutputLayoutTypeSelected(LayoutTypeDTO.Version) ? null : SelectedVersion.Id,
                    ModelId = IsOutputLayoutTypeSelected(LayoutTypeDTO.Model) ? null : SelectedModel.Id,
                    OptionId = IsOutputLayoutTypeSelected(LayoutTypeDTO.Option) ? null : SelectedOption.Id,
                    KitId = IsOutputLayoutTypeSelected(LayoutTypeDTO.Kit) ? null : SelectedKit.Id,
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ErrorMessage = "Unsupported Combination";
                Combination = null;
            }
            finally
            {
                CombinationsBusy = false;
                FiltersOpened = KeepOpen || false;

                StateHasChanged();
            }
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
