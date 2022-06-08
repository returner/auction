namespace Identity.Configuration.Models
{
    public record DatabaseSetting
    {
        public DatabaseType Type { get; init; }
        public string? Hostname { get; init; }
        public int Port { get; init; }
        public string? Username { get; init; }
        public string? Password { get; init; }
        public string? DbName { get; init; }
    }

}
