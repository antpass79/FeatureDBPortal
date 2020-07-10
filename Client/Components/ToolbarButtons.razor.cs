using FeatureDBPortal.Client.Models;
using FeatureDBPortal.Client.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Components
{
    public class ToolbarButtonsDataModel : ComponentBase, IDisposable
    {
        [Inject]
        protected ToolbarButtonsService ButtonsService { get; set; }

        protected override void OnInitialized()
        {
            ButtonsService.Actions.CollectionChanged += Actions_CollectionChanged;
            base.OnInitialized();
        }

        async protected Task OnActionClick(ButtonAction action)
        {
            ButtonsService.FireAction.Invoke(action);
            await Task.CompletedTask;
        }

        private void Actions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StateHasChanged();
        }

        public void Dispose()
        {
            ButtonsService.Actions.CollectionChanged -= Actions_CollectionChanged;
        }
    }
}
