using System;

namespace Identity.Entities
{
    public record AuthClient
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ClientId { get; set; }
        public bool IsUse { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
