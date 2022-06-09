﻿using System.Collections.Generic;

namespace Identity.Configuration.Models
{
    public record AppSettings : IAppSettings
    {
        public IEnumerable<string>? CorsOrigins { get; init; }
        public DatabaseSetting? Database { get; init; }
        public SwaggerSetting? Swagger { get; init; }
        public JwtParamter Jwt { get; init; } = null!;
    }
}
