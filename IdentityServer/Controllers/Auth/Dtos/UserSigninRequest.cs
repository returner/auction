namespace Identity.Controllers.Auth.Dtos
{
    public record UserSigninRequest(string ClientId, string UserId, string Password);
}
