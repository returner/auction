namespace Identity.Controllers.Auth.Dtos
{
    public record CreateUserRequest
    {
        public string? UserId { get; }
        public string? Password { get; }
        public string? Name { get; }
        public string? Email { get; }
    }
}
