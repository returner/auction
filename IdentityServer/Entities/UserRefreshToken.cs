using System;

namespace Identity.Entities
{
    public class UserRefreshToken
    {
        public int Id { get; set; }
        public string? ClientId { get; set; }
        public string? UserKey {get;set;}
        public string? Token { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
