using FeatureDBPortal.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Pages
{
    public class LogoutDataModel : ComponentBase
    {
        [Inject]
        public IAuthService AuthService { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthService.Logout();
            UrlNavigationManager.NavigateTo("/");
        }
    }
}
