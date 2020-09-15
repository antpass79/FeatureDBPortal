using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class DatabaseService : IAsyncDatabaseService
    {
        public Task ConnectAsync(string databaseName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<string>> GetDatabaseNamesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task UploadAsync(byte[] database)
        {
            throw new System.NotImplementedException();
        }
    }
}
