using Identity.Configuration.Models;
using Identity.Controllers.Auth.Payloads;
using Identity.Entities;
using Identity.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Controllers.Auth
{
    public class AuthController : ApiControllerBase
    {
        private readonly IIdentityContext _context;
        private readonly IAppSettings _appSettings;

        public AuthController(IIdentityContext context, IAppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        /// <summary>
        /// 유저 로그인
        /// </summary>
        /// <param name="userSigninRequest"></param>
        /// <returns></returns>
        [HttpPost("Signin")]
        [SwaggerResponse(200, "success", typeof(UserSigninResponse))]
        public async Task<IActionResult> SigninAsync(UserSigninRequest userSigninRequest)
        {
            if (userSigninRequest == null || string.IsNullOrWhiteSpace(userSigninRequest.UserId) || string.IsNullOrWhiteSpace(userSigninRequest.Password))
                throw new ArgumentNullException(nameof(userSigninRequest));

            var user = await _context.AdminUsers
                .Where(d => d.UserId != null && d.UserId.Equals(userSigninRequest.UserId) && d.Password != null && d.Password.Equals(userSigninRequest.Password))
                .SingleOrDefaultAsync();

            if (user is null)
                throw new UserNotExistException();

            var v = _appSettings?.Jwt?.GetType().GetProperties().All(d => d.GetValue(_appSettings.Jwt) != null);
            if (v.HasValue && !v.Value)
            {
                throw new NullReferenceException(nameof(_appSettings.Jwt));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _appSettings!.Jwt!.Subject!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", user.UserId is null ? string.Empty : user.UserId),
                new Claim("Name", user.Name is null ? string.Empty : user.Name),
                new Claim("Email", user.Email is null ? string.Empty : user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings!.Jwt!.SigningKey!));
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_appSettings.Jwt.Issuer, _appSettings.Jwt.Audience, claims, 
                expires: DateTime.UtcNow.AddMinutes(_appSettings.Jwt.ExpireMinutes), signingCredentials: signin);

            var result = new UserSigninResponse
            { 
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = Guid.NewGuid().ToString(),
                ExpiresIn = _appSettings.Jwt.ExpireMinutes * 60,
            };

            return Ok(result);
        }


        /// <summary>
        /// 사용자 정보 조회
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Users")]
        [SwaggerResponse(200, "success", typeof(OkResult))]
        public async Task<IActionResult> AuthTest()
        {
            var data = await _context.AdminUsers.ToArrayAsync();
            return new OkObjectResult(data);
        }

        [HttpGet("User/{id}")]
        [SwaggerResponse(200, "success", typeof(OkResult))]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _context.AdminUsers.FindAsync(id);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// 사용자 생성
        /// </summary>
        /// <param name="createUserRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPost("CreateUser")]
        [SwaggerResponse(200, "success", typeof(OkResult))]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            if (string.IsNullOrWhiteSpace(createUserRequest.UserId) || string.IsNullOrWhiteSpace(createUserRequest.Password))
                throw new ArgumentNullException();

            var isExist = await _context.AdminUsers.Where(d => d.UserId != null && d.UserId.Equals(createUserRequest.UserId)).AnyAsync();
            if (isExist)
                throw new Exception("User Exist");

            await _context.AdminUsers.AddAsync(new AdminUser
            {
                UserId = createUserRequest.UserId,
                Password = createUserRequest.Password,
                Name = createUserRequest.Name,
                Email = createUserRequest.Email,
                CreatedDate = DateTime.UtcNow
            });

            var result = await _context.SaveChangesAsync();
            return new OkObjectResult(result);
        }


    }
}
