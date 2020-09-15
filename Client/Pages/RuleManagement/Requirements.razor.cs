using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Client.Services.RuleManagement;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.RuleManagement;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Pages.RuleManagement
{
    public class RequirementsDataModel : ComponentBase
    {
        #region Properties

        [Inject]
        protected IRequirementRuleService RuleService { get; set; }

        protected bool FiltersBusy { get; set; }

        protected int? IncrementalVersion { get; set; }

        protected IEnumerable<ApplicationDTO> Applications { get; set; }
        protected ApplicationDTO SelectedApplication { get; private set; } = new ApplicationDTO();

        protected IEnumerable<ProbeDTO> Probes { get; set; }
        protected ProbeDTO SelectedProbe { get; private set; } = new ProbeDTO();

        protected IEnumerable<ModelDTO> Models { get; set; }
        protected ModelDTO SelectedModel { get; set; } = new ModelDTO();

        protected IEnumerable<PhysicalModelDTO> PhysicalModels { get; set; }
        protected PhysicalModelDTO SelectedPhysicalModel { get; set; } = new PhysicalModelDTO();

        protected IEnumerable<OptionDTO> Options { get; set; }
        protected OptionDTO SelectedOption { get; private set; } = new OptionDTO();

        protected IEnumerable<KitDTO> Kits { get; set; }
        protected KitDTO SelectedKit { get; private set; } = new KitDTO();

        protected EditableRequirementRule EditableRule { get; private set; } = new EditableRequirementRule();

        #endregion

        #region Protected Functions

        async protected override Task OnInitializedAsync()
        {
            FiltersBusy = true;

            Applications = await RuleService.GetApplicationsAsync();
            Probes = await RuleService.GetProbesAsync();
            Models = await RuleService.GetModelsAsync();
            PhysicalModels = await RuleService.GetPhysicalModelsAsync();
            Options = await RuleService.GetOptionsAsync();
            Kits = await RuleService.GetKitsAsync();

            FiltersBusy = false;
        }

        async protected Task OnConfirm()
        {
            await RuleService.InsertAsync(new RequirementRuleDTO
            {
                IncrementalVersion = IncrementalVersion.Value,
                Application = SelectedApplication,
                Kit = SelectedKit,
                Model = SelectedModel,
                PhysicalModel = SelectedPhysicalModel,
                Option = SelectedOption,
                Probe = SelectedProbe
            });
        }

        #endregion
    }
}