using System.Collections.Generic;

namespace Identity.Configuration.Models
{
    public interface IAppSettings
    {
        IEnumerable<string>? CorsOrigins { get; init; }
        DatabaseSetting? Database { get; init; }
        SwaggerSetting? Swagger { get; init; }
        JwtParamter Jwt { get; init; }
    }

    public record JwtParamter
    {
        public string Subject { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public int ExpireMinutes { get; init; }
        public string SigningKey { get; set; } = null!;
    }

}
