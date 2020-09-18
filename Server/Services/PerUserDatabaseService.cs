using FeatureDBPortal.Server.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    public class PerUserDatabaseService : IAsyncPerUserDatabaseService
    {
        ConcurrentDictionary<string, string> _userToDatabaseMapping = new ConcurrentDictionary<string, string>();

        public string this[string userName]
        {
            get
            {
                if (!_userToDatabaseMapping.ContainsKey(userName))
                    return null;

                return _userToDatabaseMapping[userName];
            }
        }

        async public Task ConnectAsync(ClaimsPrincipal user, string databaseName)
        {
            var userName = user.FindFirstValue(ClaimTypes.Name);

            if (!_userToDatabaseMapping.ContainsKey(userName))
            {
                if (!_userToDatabaseMapping.TryAdd(userName, databaseName))
                    throw new Exception("TryAdd Failed");
            }
            else if (_userToDatabaseMapping.ContainsKey(userName) && _userToDatabaseMapping[userName] != databaseName)
            {
                await DisconnectAsync(user);
                if (!_userToDatabaseMapping.TryAdd(userName, databaseName))
                    throw new Exception("TryAdd Failed");
            }
        }

        async public Task<IEnumerable<string>> GetDatabaseNamesAsync()
        {
            var databases = Directory
                .EnumerateFiles(FeaturesPortalConstants.DATABASES_FOLDER)
                .Select(path => Path.GetFileName(path))
                .OrderBy(item => item)
                .ToList();

            databases.Insert(0, FeaturesPortalConstants.DEFAULT_DATABASE_NAME);

            return await Task.FromResult(databases);
        }

        async public Task<string> UploadAsync(ClaimsPrincipal user, byte[] database)
        {
            var userName = user
                .FindFirstValue(ClaimTypes.Name)
                .Replace(@"\", "-");

            string databasesFolder = Path.Combine(Directory.GetCurrentDirectory(), FeaturesPortalConstants.DATABASES_FOLDER);
            string generatedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH_mm_ssZ");
            string generatedFileName = $"{userName}-{generatedDate}.db";

            await File.WriteAllBytesAsync($@"{databasesFolder}\{generatedFileName}", database);

            return generatedFileName;
        }

        async public Task DisconnectAsync(ClaimsPrincipal user)
        {
            var userName = user.FindFirstValue(ClaimTypes.Name);
            if (_userToDatabaseMapping.ContainsKey(userName))
            {
                string value;
                if (!_userToDatabaseMapping.TryRemove(userName, out value))
                    throw new Exception("TryRemove Failed");
            }

            await Task.CompletedTask;
        }
    }
}
