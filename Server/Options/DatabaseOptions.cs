namespace FeatureDBPortal.Server.Options
{
    public class DatabaseOptions
    {
        public string DefaultSqlServerConnectionString { get; set; }
        public string DefaultSqliteConnectionString { get; set; }
        public DatabaseType DatabaseType { get; set; }
        public bool PerUser { get; set; }
    }
}
