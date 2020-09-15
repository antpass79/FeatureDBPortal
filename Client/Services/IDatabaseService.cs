using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public interface IDatabaseService
    {
        public Task ConnectAsync(string databaseName);
        public Task<IEnumerable<string>> GetDatabaseNamesAsync();
        public Task UploadAsync(byte[] database);
    }
}
