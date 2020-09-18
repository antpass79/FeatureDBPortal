using System;

namespace FeatureDBPortal.Client.Services
{
    public class DatabaseConnectionEventArgs : EventArgs
    {
        public string CurrentDatabase { get; }
        public bool Connected { get; }

        public DatabaseConnectionEventArgs(string currentDatabase)
        {
            CurrentDatabase = currentDatabase;
            Connected = string.IsNullOrWhiteSpace(currentDatabase) ? false : true;
        }
    }
}
