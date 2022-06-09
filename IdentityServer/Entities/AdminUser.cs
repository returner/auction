using System;

namespace Identity.Entities
{
    public record AdminUser
    {
        public int Id { get; set; }
        public string? ClientId { get; set; }
        public string? UserKey { get; set; }
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
