﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureDBPortal.Client.Services
{
    public interface IDatabaseService
    {
        bool Connected { get; }
        string CurrentDatabase { get; }
        Task<IEnumerable<string>> GetDatabaseNamesAsync();
        Task ConnectAsync(string databaseName);
        Task<string> UploadAsync(byte[] database);
        Task DisconnectAsync();
        event EventHandler<DatabaseConnectionEventArgs> DatabaseConnectionChanged;
    }
}
