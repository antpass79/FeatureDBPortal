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
        protected EditableVersionRule EditableVersionRule { get; private set; } = new EditableVersionRule();

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
                Id = EditableVersionRule.Id.Value,
                Major = EditableVersionRule.Major.Value,
                Minor = EditableVersionRule.Minor.Value,
                Patch = EditableVersionRule.Patch.Value,
                BuildVersion = EditableVersionRule.BuildVersion.Value
            });
        }

        async protected Task OnEdit(MinorVersionRuleDTO rule)
        {
            SelectedVersionRule = rule;

            EditableVersionRule.Id = SelectedVersionRule.Id;
            EditableVersionRule.Major = SelectedVersionRule.Major;
            EditableVersionRule.Minor = SelectedVersionRule.Minor;
            EditableVersionRule.Patch = SelectedVersionRule.Patch;
            EditableVersionRule.BuildVersion = SelectedVersionRule.BuildVersion;

            await Task.CompletedTask;
        }

        async protected Task OnNew()
        {
            EditableVersionRule = new EditableVersionRule();
            await Task.CompletedTask;
        }

        #endregion
    }
}
