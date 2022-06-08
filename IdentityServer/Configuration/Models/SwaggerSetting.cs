namespace Identity.Configuration.Models
{
    public record SwaggerSetting
    {
        public string? Title { get; init; }
        public string? Version { get; init; }
        public string? Description { get; init; }
        public string? Link { get; init; }
    }
}
