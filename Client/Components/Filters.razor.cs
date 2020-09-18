using FeatureDBPortal.Client.Extensions;
using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Client.Services;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Components
{
    public class FiltersDataModel : ComponentBase
    {        
        [Inject]
        protected IDatabaseService DatabaseService { get; set; }
        [Inject]
        protected IFilterService FilterService { get; set; }
        [Parameter]
        public SearchFilters SearchFilters { get; set; } = new SearchFilters();

        protected bool Busy { get; set; }

        protected IEnumerable<ApplicationDTO> Applications { get; set; }

        protected IEnumerable<ProbeDTO> Probes { get; set; }

        protected IEnumerable<CountryDTO> Countries { get; set; }

        protected IEnumerable<VersionDTO> Versions { get; set; }

        protected IEnumerable<ModelDTO> Models { get; set; }

        protected IEnumerable<OptionDTO> Options { get; set; }

        protected IEnumerable<KitDTO> Kits { get; set; }

        protected IEnumerable<UserLevelDTO> UserLevels { get; set; }

        string _selectedUserLevelText;
        public string SelectedUserLevelText
        {
            get => _selectedUserLevelText;
            set
            {
                _selectedUserLevelText = value;
                if (Enum.TryParse<UserLevelDTO>(_selectedUserLevelText, out var userLevel))
                {
                    SearchFilters.UserLevel = userLevel;
                }
            }
        }

        protected IEnumerable<LayoutTypeDTO> LayoutViews { get; set; }
        string _selectedRowLayoutText;
        public string SelectedRowLayoutText
        {
            get => _selectedRowLayoutText;
            set
            {
                _selectedRowLayoutText = value;
                if (Enum.TryParse<LayoutTypeDTO>(_selectedRowLayoutText, out var layoutType))
                {
                    SearchFilters.RowLayout = layoutType;
                }
            }
        }

        string _selectedColumnLayoutText;
        public string SelectedColumnLayoutText
        {
            get => _selectedColumnLayoutText;
            set
            {
                _selectedColumnLayoutText = value;
                if (Enum.TryParse<LayoutTypeDTO>(_selectedColumnLayoutText, out var layoutType))
                {
                    SearchFilters.ColumnLayout = layoutType;
                }
            }
        }

        string _selectedCellLayoutText;
        public string SelectedCellLayoutText
        {
            get => _selectedCellLayoutText;
            set
            {
                _selectedCellLayoutText = value;
                if (Enum.TryParse<LayoutTypeDTO>(_selectedCellLayoutText, out var layoutType))
                {
                    SearchFilters.CellLayout = layoutType;
                }
            }
        }

        protected bool DisableApplication => SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Application);
        protected bool DisableProbe => SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Probe);
        protected bool DisableModel => SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Model);
        protected bool DisableKit => SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Kit);
        protected bool DisableOption => SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Option);
        protected bool DisableVersion => SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Version);
        protected bool DisableCountry => SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.Country);
        protected bool DisableUserLevel => SearchFilters.IsOutputLayoutTypeSelected(LayoutTypeDTO.UserLevel);

        async protected override Task OnInitializedAsync()
        {
            await UpdateDataAsync();

            DatabaseService.DatabaseConnectionChanged += async (sender, args) =>
            {
                if (args.Connected)
                    Console.WriteLine($"Connected to database {args.CurrentDatabase}");
                else
                    Console.WriteLine($"Disconnected from database");

                await UpdateDataAsync();
            };
        }

        async private Task UpdateDataAsync()
        {
            Console.WriteLine($"UpdateData {DatabaseService.Connected}");

            Busy = true;

            if (DatabaseService.Connected)
            {
                Applications = await FilterService.GetApplicationsAsync();
                Probes = await FilterService.GetProbesAsync();
                Countries = await FilterService.GetCountriesAsync();
                Versions = await FilterService.GetVersionsAsync();
                Models = await FilterService.GetModelsAsync();
                Options = await FilterService.GetOptionsAsync();
                Kits = await FilterService.GetKitsAsync();
                UserLevels = Enum.GetValues(typeof(UserLevelDTO)).Cast<UserLevelDTO>().OrderBy(item => item.ToString());
                LayoutViews = Enum.GetValues(typeof(LayoutTypeDTO)).Cast<LayoutTypeDTO>();

                SearchFilters.Reset();
                SearchFilters.Model = Models.FirstOrDefault();
                SearchFilters.Country = Countries.FirstOrDefault();
                SearchFilters.UserLevel = UserLevels.FirstOrDefault();
                SelectedUserLevelText = SearchFilters.UserLevel.ToString();
                SelectedRowLayoutText = SearchFilters.RowLayout.ToString();
                SelectedColumnLayoutText = SearchFilters.ColumnLayout.ToString();
                SelectedCellLayoutText = SearchFilters.CellLayout.ToString();
            }
            else
            {
                Applications = null;
                Probes = null;
                Countries = null;
                Versions = null;
                Models = null;
                Options = null;
                Kits = null;
                UserLevels = null;
                LayoutViews = null;

                SearchFilters.Reset();
                SelectedUserLevelText = UserLevelDTO.None.ToString();
                SelectedRowLayoutText = LayoutTypeDTO.None.ToString();
                SelectedColumnLayoutText = LayoutTypeDTO.None.ToString();
                SelectedCellLayoutText = LayoutTypeDTO.None.ToString();
            }

            Busy = false;

            StateHasChanged();
        }
    }
}
