namespace Identity.Controllers.Auth.Payloads
{
    public record UserSigninRequest(string UserId, string Password);
}
