using BlazorInputFile;
using FeatureDBPortal.Client.Extensions;
using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Client.Services;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Pages
{
    public class FeaturesDataModel : ComponentBase, IDisposable
    {
        const string BUTTON_ACTION_CONNECT_TO_DATABASE = "BUTTON_ACTION_CONNECT_TO_DATABASE";
        const string BUTTON_ACTION_DISCONNECT_FROM_DATABASE = "BUTTON_ACTION_DISCONNECT_FROM_DATABASE";
        const string BUTTON_ACTION_UPLOAD_DATABASE = "BUTTON_ACTION_UPLOAD_DATABASE";
        const string BUTTON_ACTION_EXPORT_TO_CSV = "BUTTON_ACTION_EXPORT_TO_CSV";
        const string BUTTON_ACTION_SYNC_RA = "BUTTON_ACTION_SYNC_RA";

        MemoryStream _localDatabase;

        [Inject]
        protected IAvailabilityCombinationService AvailabilityCombinationService { get; set; }
        [Inject]
        protected ICsvExportService CsvExportService { get; set; }
        [Inject]
        protected IDatabaseService DatabaseService { get; set; }
        [Inject]
        protected ToolbarButtonsService ButtonsService { get; set; }

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

        protected bool ShowUploadDatabaseDialog { get; set; }
        protected bool ShowConnectToDatabaseDialog { get; set; }
        protected IEnumerable<string> Databases { get; set; }
        protected string SelectedDatabase { get; set; }
        protected string UploadedDatabase { get; set; }        
        protected SearchFilters SearchFilters = new SearchFilters();
        private CombinationSearchDTO LastSearch { get; set; }

        protected Combination Combination { get; private set; }

        protected CombinationFilters CombinationFilters = new CombinationFilters
        {
            KeepIfIdNotNull = true,
            KeepIfCellModeNotNull = true,
            KeepIfCellModeA = true,
            KeepIfCellModeDef = true,
            KeepIfCellModeNo = true
        };

        protected override async Task OnInitializedAsync()
        {
            BuildToolbar();

            DatabaseService.DatabaseConnectionChanged += (sender, args) =>
            {
                Console.WriteLine($"HEREEE");
                Combination = null;
            };

            await UpdateDatabaseNames();

            await Task.CompletedTask;
        }

        async protected Task OnSearch()
        {
            ErrorMessage = string.Empty;
            Combination = null;

            using var watcher = new Watcher("CLIENT-SERVER ROUNDTRIP");

            CombinationsBusy = true;

            try
            {
                LastSearch = new CombinationSearchDTO
                {
                    ApplicationId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Application) ? null : SearchFilters.Application.Id,
                    ProbeId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Probe) ? null : SearchFilters.Probe.Id,
                    CountryId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Country) ? null : SearchFilters.Country.Id,
                    VersionId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Version) ? null : SearchFilters.Version.Id,
                    ModelId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Model) ? null : SearchFilters.Model.Id,
                    OptionId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Option) ? null : SearchFilters.Option.Id,
                    KitId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Kit) ? null : SearchFilters.Kit.Id,
                    UserLevel = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.UserLevel) ? UserLevelDTO.None : SearchFilters.UserLevel,
                    RowLayout = SearchFilters.RowLayout,
                    ColumnLayout = SearchFilters.ColumnLayout,
                    CellLayout = SearchFilters.CellLayout
                };
                var combinationDTO = await AvailabilityCombinationService.GetCombination(LastSearch);

                Combination combination;

                using (var innerWatcher = new Watcher("ToModel"))
                {
                    combination = combinationDTO.ToModel();
                }

                using (var innerWatcher = new Watcher("ApplyFilters"))
                {
                    combination.ApplyFilters(CombinationFilters);
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
                Id = BUTTON_ACTION_CONNECT_TO_DATABASE,
                Label = "Connect to database",
                IconName = "data_usage"
            });
            ButtonsService.Actions.Add(new ButtonAction
            {
                Id = BUTTON_ACTION_DISCONNECT_FROM_DATABASE,
                Label = "Disconnect from database",
                IconName = "data_usage"
            });
            ButtonsService.Actions.Add(new ButtonAction
            {
                Id = BUTTON_ACTION_UPLOAD_DATABASE,
                Label = "Upload database",
                IconName = "data_usage"
            });
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

            ButtonsService.FireAction = async (action) =>
            {
                switch (action.Id)
                {
                    case BUTTON_ACTION_CONNECT_TO_DATABASE:
                        ShowConnectToDatabaseDialog = true;
                        break;
                    case BUTTON_ACTION_DISCONNECT_FROM_DATABASE:
                        await DatabaseService.DisconnectAsync();
                        break;
                    case BUTTON_ACTION_UPLOAD_DATABASE:
                        UploadedDatabase = string.Empty;
                        ShowUploadDatabaseDialog = true;
                        break;
                    case BUTTON_ACTION_EXPORT_TO_CSV:
                        CsvExportSettings.FileName = string.Empty;
                        ShowCsvExportDialog = true;
                        break;
                    case BUTTON_ACTION_SYNC_RA:
                        break;
                }

                StateHasChanged();
            };
        }

        public void Dispose()
        {
            ButtonsService.Actions.Clear();
            DatabaseService.DisconnectAsync();
        }

        async protected Task OnDownload()
        {
            ShowCsvExportDialog = false;
            await CsvExportService.DownloadCsv(new CsvExportDTO
            {
                Search = LastSearch,
                Combination = Combination.ToDTO(),
                Settings = CsvExportSettings
            });
        }

        async protected Task OnDatabaseUpload()
        {
            UploadedDatabase = await DatabaseService.UploadAsync(_localDatabase.GetBuffer());
            await UpdateDatabaseNames();
        }

        async protected Task HandleSelection(IFileListEntry[] files)
        {
            var file = files.FirstOrDefault();
            if (file != null)
            {
                _localDatabase = new MemoryStream();
                await file.Data.CopyToAsync(_localDatabase);
            }
        }

        async protected Task OnDatabaseConnect()
        {
            ShowConnectToDatabaseDialog = false;
            await DatabaseService.ConnectAsync(SelectedDatabase);
        }

        private async Task UpdateDatabaseNames()
        {
            try
            {
                Databases = await DatabaseService.GetDatabaseNamesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
