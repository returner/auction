namespace Identity.Controllers.Auth.Payloads
{
    public record IdentityJwtTokenPayload(string AccessToken, string IdToken, string RefreshToken);
}
