using FeatureDBPortal.Client.Extensions;
using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Client.Services;
using FeatureDBPortal.Shared;
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
        [Inject]
        Filter.FilterClient Filter { get; set; }
        [Inject]
        Combiner.CombinerClient Combiner { get; set; }

        protected bool FiltersBusy { get; set; }
        protected bool CombinationsBusy { get; set; }

        protected IEnumerable<ApplicationGRPC> Applications { get; set; }
        protected ApplicationGRPC SelectedApplication { get; private set; } = new ApplicationGRPC();

        protected IEnumerable<ProbeGRPC> Probes { get; set; }
        protected ProbeGRPC SelectedProbe { get; private set; } = new ProbeGRPC();

        protected IEnumerable<CountryGRPC> Countries { get; set; }
        protected CountryGRPC SelectedCountry { get; private set; } = new CountryGRPC();

        protected IEnumerable<VersionGRPC> Versions { get; set; }
        protected VersionGRPC SelectedVersion { get; private set; } = new VersionGRPC();

        protected IEnumerable<ModelGRPC> Models { get; set; }
        protected ModelGRPC SelectedModel { get; private set; } = new ModelGRPC();

        protected IEnumerable<OptionGRPC> Options { get; set; }
        protected OptionGRPC SelectedOption { get; private set; } = new OptionGRPC();

        protected IEnumerable<KitGRPC> Kits { get; set; }
        protected KitGRPC SelectedKit { get; private set; } = new KitGRPC();

        protected IEnumerable UserLevels { get; set; }
        protected UserLevelGRPC SelectedUserLevel { get; set; }

        protected IEnumerable LayoutViews { get; set; }
        protected LayoutTypeGRPC SelectedRowLayout { get; set; } = LayoutTypeGRPC.None;
        protected LayoutTypeGRPC SelectedColumnLayout { get; set; } = LayoutTypeGRPC.None;
        protected LayoutTypeGRPC SelectedCellLayout { get; set; } = LayoutTypeGRPC.None;

        protected Combination Combination { get; private set; }

        protected LayoutTypeGRPC CurrentHeader { get; set; }

        protected bool DisableApplication => SelectedRowLayout == LayoutTypeGRPC.Application || SelectedColumnLayout == LayoutTypeGRPC.Application || SelectedCellLayout == LayoutTypeGRPC.Application;
        protected bool DisableProbe => SelectedRowLayout == LayoutTypeGRPC.Probe || SelectedColumnLayout == LayoutTypeGRPC.Probe || SelectedCellLayout == LayoutTypeGRPC.Probe;
        protected bool DisableModel => SelectedRowLayout == LayoutTypeGRPC.Model || SelectedColumnLayout == LayoutTypeGRPC.Model || SelectedCellLayout == LayoutTypeGRPC.Model;
        protected bool DisableKit => SelectedRowLayout == LayoutTypeGRPC.Kit || SelectedColumnLayout == LayoutTypeGRPC.Kit || SelectedCellLayout == LayoutTypeGRPC.Kit;
        protected bool DisableOption => SelectedRowLayout == LayoutTypeGRPC.Option || SelectedColumnLayout == LayoutTypeGRPC.Option || SelectedCellLayout == LayoutTypeGRPC.Option;
        protected bool DisableVersion => SelectedRowLayout == LayoutTypeGRPC.Version || SelectedColumnLayout == LayoutTypeGRPC.Version || SelectedCellLayout == LayoutTypeGRPC.Version;
        protected bool DisableCountry => SelectedRowLayout == LayoutTypeGRPC.Country || SelectedColumnLayout == LayoutTypeGRPC.Country || SelectedCellLayout == LayoutTypeGRPC.Country;
        protected bool DisableUserLevel => SelectedRowLayout == LayoutTypeGRPC.UserLevel || SelectedColumnLayout == LayoutTypeGRPC.UserLevel || SelectedCellLayout == LayoutTypeGRPC.UserLevel;

        private bool IsOutputLayoutTypeSelected(LayoutTypeGRPC layoutType) => SelectedRowLayout == layoutType || SelectedColumnLayout == layoutType || SelectedCellLayout == layoutType;

        protected override async Task OnInitializedAsync()
        {
            FiltersBusy = true;

            Applications = (await Filter.GetApplicationsAsync(new EmptyParam())).Applications;
            Probes = (await Filter.GetProbesAsync(new EmptyParam())).Probes;
            Countries = (await Filter.GetCountriesAsync(new EmptyParam())).Countries;
            Versions = (await Filter.GetVersionsAsync(new EmptyParam())).Versions;
            Models = (await Filter.GetModelsAsync(new EmptyParam())).Models;
            Options = (await Filter.GetOptionsAsync(new EmptyParam())).Options;
            Kits = (await Filter.GetKitsAsync(new EmptyParam())).Kits;
            UserLevels = Enum.GetValues(typeof(UserLevelGRPC));
            LayoutViews = Enum.GetValues(typeof(LayoutTypeGRPC));

            SelectedModel = Models.FirstOrDefault();
            SelectedCountry = Countries.FirstOrDefault();
            SelectedUserLevel = UserLevels.Cast<UserLevelGRPC>().FirstOrDefault();

            FiltersBusy = false;

            await Task.CompletedTask;
        }

        async protected Task OnSearch()
        {
            var start = DateTime.Now;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CombinationsBusy = true;

            var combinationGRPC = await Combiner.GetCombinationAsync(new CombinationSearchGRPC
            {
                Application = IsOutputLayoutTypeSelected(LayoutTypeGRPC.Application) ? null : SelectedApplication,
                Probe = IsOutputLayoutTypeSelected(LayoutTypeGRPC.Probe) ? null : SelectedProbe,
                Country = IsOutputLayoutTypeSelected(LayoutTypeGRPC.Country) || SelectedCellLayout == LayoutTypeGRPC.Country ? null : SelectedCountry,
                Version = IsOutputLayoutTypeSelected(LayoutTypeGRPC.Version) ? null : SelectedVersion,
                Model = IsOutputLayoutTypeSelected(LayoutTypeGRPC.Model) ? null : SelectedModel,
                Option = IsOutputLayoutTypeSelected(LayoutTypeGRPC.Option) ? null : SelectedOption,
                Kit = IsOutputLayoutTypeSelected(LayoutTypeGRPC.Kit) ? null : SelectedKit,
                UserLevel = IsOutputLayoutTypeSelected(LayoutTypeGRPC.UserLevel) ? UserLevelGRPC.None : SelectedUserLevel,
                RowLayout = SelectedRowLayout,
                ColumnLayout = SelectedColumnLayout,
                CellLayout = SelectedCellLayout
            });

            Combination = combinationGRPC.ToModel();
            CurrentHeader = SelectedRowLayout;

            CombinationsBusy = false;

            Trace.WriteLine(string.Empty);
            Trace.WriteLine($"FAST FEATURE: Process starts at {start} and stops at {DateTime.Now} with duration of {stopwatch.Elapsed}");
        }
    }
}
