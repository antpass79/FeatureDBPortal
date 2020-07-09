using FeatureDBPortal.Shared;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public interface IAsyncLoginService
    {
        Task<LoginResultDTO> LoginAsync(CredentialsDTO credentials);
        Task LogoutAsync();
    }
}
