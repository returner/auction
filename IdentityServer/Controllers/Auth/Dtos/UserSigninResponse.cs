using Identity.Interfaces;

namespace Identity.Controllers.Auth.Dtos
{
    public record UserSigninResponse : IResponseBase
    {
        public string? AccessToken { get; init; }
        public string? IdToken { get; init; }
        public string? RefreshToken { get; init; }
        public int ExpiresIn { get; init; }
    }
}
