namespace Identity.Controllers.Auth.Payloads
{
    public record CreateUserRequest
    {
        public string? UserId { get; init; }
        public string? Password { get; init; }
        public string? Name { get; init; }
        public string? Email { get; init; }
    }
}
