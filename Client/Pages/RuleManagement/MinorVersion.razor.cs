using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Client.Services.RuleManagement;
using FeatureDBPortal.Shared.RuleManagement;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Pages.RuleManagement
{
    public class MinorVersionDataModel : ComponentBase
    {
        [Inject]
        protected IMinorVersionRuleService RuleService { get; set; }

        protected bool Busy { get; set; }

        protected IEnumerable<MinorVersionRuleDTO> VersionRules { get; set; }
        protected MinorVersionRuleDTO SelectedVersionRule { get; set; }
        protected EditableVersionRule EditableRule { get; private set; } = new EditableVersionRule();

        #region Protected Functions

        async protected override Task OnInitializedAsync()
        {
            Busy = true;

            VersionRules = await RuleService.GetAllAsync();

            Busy = false;
        }

        async protected Task OnConfirm()
        {
            await RuleService.InsertAsync(new MinorVersionRuleDTO
            {
                Id = EditableRule.Id.Value,
                Major = EditableRule.Major.Value,
                Minor = EditableRule.Minor.Value,
                Patch = EditableRule.Patch.Value,
                BuildVersion = EditableRule.BuildVersion.Value
            });
        }

        async protected Task OnEdit(MinorVersionRuleDTO rule)
        {
            SelectedVersionRule = rule;

            EditableRule.Id = SelectedVersionRule.Id;
            EditableRule.Major = SelectedVersionRule.Major;
            EditableRule.Minor = SelectedVersionRule.Minor;
            EditableRule.Patch = SelectedVersionRule.Patch;
            EditableRule.BuildVersion = SelectedVersionRule.BuildVersion;

            await Task.CompletedTask;
        }

        async protected Task OnNew()
        {
            EditableRule = new EditableVersionRule();
            await Task.CompletedTask;
        }

        #endregion
    }
}
