namespace FeatureDBPortal.Server.Options
{
    public class DatabaseOptions
    {
        public DatabaseType DatabaseType { get; set; }
        public string DefaultSqlServerConnectionString { get; set; }
        public string DefaultSqliteConnectionString { get; set; }
    }
}
