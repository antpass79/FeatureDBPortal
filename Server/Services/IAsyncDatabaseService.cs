using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public interface IAsyncDatabaseService
    {
        Task<IEnumerable<string>> GetDatabaseNamesAsync();
        Task ConnectAsync(string databaseName);
        Task UploadAsync(byte[] database);
    }
}
