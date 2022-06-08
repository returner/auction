using Identity.Configuration.Models;

namespace Identity.Configuration
{
    public class ConnectionStringMapper
    {
        public static string GetConnectionString(IAppSettings appSettings)
        {
            if (appSettings.Database is null)
                throw new ArgumentNullException(nameof(appSettings.Database));

            switch (appSettings.Database.Type)
            {
                case DatabaseType.Postgres:
                    return BuildPostgresConnectionString(appSettings.Database);
                default:
                    return string.Empty;
            }
        }

        private static string BuildPostgresConnectionString(DatabaseSetting? database)
        {
            if (database is null)
                throw new ArgumentNullException(nameof(database));

            return $"Host={database.Hostname}:{database.Port};Database={database.DbName};Username={database.Username};Password={database.Password}";
        }
    }

}
