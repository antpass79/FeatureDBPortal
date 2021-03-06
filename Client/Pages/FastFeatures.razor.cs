﻿using FeatureDBPortal.Client.Extensions;
using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Client.Services;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.Utilities;
using GrpcCombination;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Pages
{
    public class FastFeaturesDataModel : ComponentBase
    {
        const string BUTTON_ACTION_EXPORT_TO_CSV = "BUTTON_ACTION_EXPORT_TO_CSV";
        const string BUTTON_ACTION_SYNC_RA = "BUTTON_ACTION_SYNC_RA";

        [Inject]
        Filter.FilterClient Filter { get; set; }
        [Inject]
        Combiner.CombinerClient Combiner { get; set; }
        [Inject]
        protected ToolbarButtonsService ButtonsService { get; set; }
        [Inject]
        protected ICsvExportService CsvExportService { get; set; }


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

        protected SearchFilters SearchFilters = new SearchFilters();
        private CombinationSearchGRPC LastSearch { get; set; }

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
                LastSearch = new CombinationSearchGRPC
                {                    
                    ApplicationId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Application) ? null : SearchFilters.Application.Id,
                    ProbeId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Probe) ? null : SearchFilters.Probe.Id,
                    CountryId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Country) ? null : SearchFilters.Country.Id,
                    VersionId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Version) ? null : SearchFilters.Version.Id,
                    ModelId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Model) ? null : SearchFilters.Model.Id,
                    OptionId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Option) ? null : SearchFilters.Option.Id,
                    KitId = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Kit) ? null : SearchFilters.Kit.Id,
                    UserLevel = SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.UserLevel) ? UserLevelGRPC.None : (UserLevelGRPC)SearchFilters.UserLevel,
                    RowLayout = (LayoutTypeGRPC)SearchFilters.RowLayout,
                    ColumnLayout = (LayoutTypeGRPC)SearchFilters.ColumnLayout,
                    CellLayout = (LayoutTypeGRPC)SearchFilters.CellLayout
                };
                var combinationGRPC = await Combiner.GetCombinationAsync(LastSearch);

                Combination combination;

                using (var innerWatcher = new Watcher("ToModel"))
                {
                    combination = combinationGRPC.ToModel();
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
                        CsvExportSettings.FileName = string.Empty;
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
                Search = new CombinationSearchDTO
                {
                    ModelId = LastSearch.ModelId,
                    CountryId = LastSearch.CountryId,
                    UserLevel = (UserLevelDTO)LastSearch.UserLevel,
                    ApplicationId = LastSearch.ApplicationId,
                    ProbeId = LastSearch.ProbeId,
                    KitId = LastSearch.KitId,
                    OptionId = LastSearch.OptionId,
                    VersionId = LastSearch.VersionId,
                    RowLayout = (LayoutTypeDTO)LastSearch.RowLayout,
                    ColumnLayout = (LayoutTypeDTO)LastSearch.ColumnLayout,
                    CellLayout = (LayoutTypeDTO)LastSearch.CellLayout,
                },
                Combination = Combination.ToDTO(),
                Settings = CsvExportSettings
            });
        }
    }
}
