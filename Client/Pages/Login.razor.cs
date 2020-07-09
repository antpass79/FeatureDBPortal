using FeatureDBPortal.Client.Services;
using FeatureDBPortal.Shared;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Pages
{
    public class LoginDataModel : ComponentBase
    {
        [Inject]
        public IAuthService AuthService { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }

        protected CredentialsDTO credentials = new CredentialsDTO();

        protected bool ShowErrors;
        protected string Error = string.Empty;

        protected async Task HandleLogin()
        {
            ShowErrors = false;

            var result = await AuthService.Login(credentials);

            if (result.Successful)
            {
                UrlNavigationManager.NavigateTo("/");
            }
            else
            {
                Error = result.Error;
                ShowErrors = true;
            }
        }

        protected void Cancel()
        {
            UrlNavigationManager.NavigateTo("/");
        }
    }
}
