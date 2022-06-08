using System.Collections.Generic;

namespace Identity.Configuration.Models
{
    public interface IAppSettings
    {
        IEnumerable<string>? CorsOrigins { get; init; }
        DatabaseSetting? Database { get; init; }
        SwaggerSetting? Swagger { get; init; }
    }
}
