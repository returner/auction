namespace Identity.Controllers.Auth.Payloads
{
    public record UserSigninResponse
    { 
        public string? AccessToken { get; init; }
        public string? RefreshToken { get; init; }
        public int ExpiresIn { get; init; }
    }
}
