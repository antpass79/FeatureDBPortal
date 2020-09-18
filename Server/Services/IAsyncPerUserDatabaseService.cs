using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public interface IAsyncPerUserDatabaseService
    {
        string this[string userName] { get; }
        Task<IEnumerable<string>> GetDatabaseNamesAsync();
        Task ConnectAsync(ClaimsPrincipal user, string databaseName);
        Task<string> UploadAsync(ClaimsPrincipal user, byte[] database);
        Task DisconnectAsync(ClaimsPrincipal user);
    }
}
