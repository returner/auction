using Identity.Configuration.Models;
using Identity.Controllers.Auth.Payloads;
using Identity.Entities;
using Identity.Helper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Controllers.Auth
{
    public class JwtTokenBuilder
    {
        private readonly IAppSettings _appSettings;
        public JwtTokenBuilder(IAppSettings appSettings) => _appSettings = appSettings;
        
        public IdentityJwtTokenPayload Generate(TokenGeneratePayload tokenUserInfo)
        {
            var accessToken = AccessToken(tokenUserInfo.ClientId, tokenUserInfo.UserKey);
            var idToken = IdToken(tokenUserInfo);
            var refreshToken = RefreshToken(tokenUserInfo.UserKey, _appSettings.Jwt.RefreshIntervalMinutes);

            return new IdentityJwtTokenPayload(accessToken, idToken, refreshToken);
        }

        private string AccessToken(string clientId, string authUserKey)
        {
            if (string.IsNullOrWhiteSpace(authUserKey))
                throw new ArgumentNullException(nameof(authUserKey));
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentNullException(nameof(clientId));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _appSettings.Jwt.Subject!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserKey", authUserKey),
                new Claim("ClientId", clientId)
            };

            return BuildToken(claims, _appSettings.Jwt.ExpireMinutes);
        }

        private string IdToken(TokenGeneratePayload adminUser)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _appSettings!.Jwt!.Subject!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserKey", adminUser.UserId is null ? string.Empty : adminUser.UserId),
                new Claim("Name", adminUser.Name is null ? string.Empty : adminUser.Name),
                new Claim("Email", adminUser.Email is null ? string.Empty : adminUser.Email)
            };
            
            return BuildToken(claims, _appSettings.Jwt.ExpireMinutes);
        }

        private string BuildToken(Claim[] claims, int expiresInMinutes)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings!.Jwt!.AccessTokenSigningKey!));
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_appSettings.Jwt.Issuer, _appSettings.Jwt.Audience, claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes), signingCredentials: signin);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string RefreshToken(string userKey, int refreshIntervalMinutes)
        {
            var refreshTokenPayload = new RefreshTokenPayload 
            { 
                UserKey = userKey,
                ExpiresInDate = DateTime.UtcNow.AddMinutes(refreshIntervalMinutes)
            };

            var serialize = JsonConvert.SerializeObject(refreshTokenPayload);

            var aes = new AESEncryption(_appSettings.Jwt.RefreshTokenDecryptKey);

            return aes.Encrypt(serialize);
        }

        private bool IsExpiresRefreshToken(string payloadMessage)
        {
            var aes = new AESEncryption(_appSettings.Jwt.RefreshTokenDecryptKey);
            var message = aes.Decrypt(payloadMessage);

            var refreshTokenPayload = JsonConvert.DeserializeObject<RefreshTokenPayload>(message);

            if (refreshTokenPayload is null)
                throw new Exception();

            return DateTime.UtcNow <= refreshTokenPayload.ExpiresInDate;
        }
        
    }
}
