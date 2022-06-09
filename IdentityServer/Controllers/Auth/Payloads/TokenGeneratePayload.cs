using Identity.Entities;
using System;

namespace Identity.Controllers.Auth.Payloads
{
    public record TokenGeneratePayload
    {
        public string UserKey { get; }
        public string UserId { get; }
        public string Email { get; }
        public string Name { get; }
        public DateTime CreatedDate { get; }
        public string ClientId { get; }

        public TokenGeneratePayload(string clientId, AdminUser adminUser)
        {
            ClientId = clientId;

            UserKey = adminUser.UserKey ?? string.Empty;
            UserId = adminUser.UserId ?? string.Empty;
            Email = adminUser.Email ?? string.Empty;
            Name = adminUser.Name ?? string.Empty;
            CreatedDate = adminUser.CreatedDate;
        }
    }
}
