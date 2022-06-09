namespace Identity.Configuration.Models
{
    public record JwtParamter
    {
        public string Subject { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public int ExpireMinutes { get; init; }
        public int RefreshIntervalMinutes { get; init; }
        public string AccessTokenSigningKey { get; init; } = null!;
        public string RefreshTokenDecryptKey { get; init; } = null!;
    }

}
