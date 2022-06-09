using System;

namespace Identity.Controllers.Auth.Payloads
{
    public record RefreshTokenPayload
    {
        public string? UserKey { get; set; }
        public DateTime ExpiresInDate { get; set; }
    }
}
