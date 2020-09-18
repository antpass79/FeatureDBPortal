using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Client.Services.RuleManagement;
using FeatureDBPortal.Shared;
using FeatureDBPortal.Shared.RuleManagement;
using Microsoft.AspNetCore.Components;
using System;
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
        protected IEnumerable<ProbeDTO> Probes { get; set; }
        protected IEnumerable<ModelDTO> Models { get; set; }
        protected IEnumerable<PhysicalModelDTO> PhysicalModels { get; set; }
        protected IEnumerable<OptionDTO> Options { get; set; }
        protected IEnumerable<KitDTO> Kits { get; set; }

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
            try
            {
            await RuleService.InsertAsync(new RequirementRuleDTO
            {
                IncrementalVersion = EditableRule.IncrementalVersion.Value,
                Application = EditableRule.Application,
                Kit = EditableRule.Kit,
                Model = EditableRule.Model,
                PhysicalModel = EditableRule.PhysicalModel,
                Option = EditableRule.Option,
                Probe = EditableRule.Probe
            });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                this.EditableRule = new EditableRequirementRule();
            }
        }

        #endregion
    }
}