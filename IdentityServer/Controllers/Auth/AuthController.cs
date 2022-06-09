using Identity.Configuration.Models;
using Identity.Controllers.Auth.Dtos;
using Identity.Controllers.Auth.Payloads;
using Identity.Entities;
using Identity.Exceptions;
using Identity.Helper;
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
            try
            {
                if (userSigninRequest == null || string.IsNullOrWhiteSpace(userSigninRequest.ClientId)
                    || string.IsNullOrWhiteSpace(userSigninRequest.UserId) || string.IsNullOrWhiteSpace(userSigninRequest.Password))
                    throw new ArgumentNullException(nameof(userSigninRequest));

                var user = await _context.AdminUsers
                    .Where(d => d.UserId != null && d.UserId.Equals(userSigninRequest.UserId) 
                    && d.Password != null && d.Password.Equals(userSigninRequest.Password))
                    .SingleOrDefaultAsync();

                if (user is null)
                    throw new UserNotExistException();

                var jwtTokenBuilder = new JwtTokenBuilder(_appSettings);

                var token = jwtTokenBuilder.Generate(new TokenGeneratePayload(userSigninRequest.ClientId, user));
                var result = new UserSigninResponse
                {
                    AccessToken = token.AccessToken,
                    IdToken = token.IdToken,
                    RefreshToken = token.RefreshToken,
                    ExpiresIn = _appSettings.Jwt.ExpireMinutes * 60,
                };

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
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
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createUserRequest.UserId) || string.IsNullOrWhiteSpace(createUserRequest.Password))
                    throw new ArgumentNullException();

                var isExist = await _context.AdminUsers.Where(d => d.UserId != null && d.UserId.Equals(createUserRequest.UserId)).AnyAsync();
                if (isExist)
                    throw new Exception("User Exist");

                await _context.AdminUsers.AddAsync(new AdminUser
                {
                    UserKey = UserKeyGenerate.Create(),
                    UserId = createUserRequest.UserId,
                    Password = createUserRequest.Password,
                    Name = createUserRequest.Name,
                    Email = createUserRequest.Email,
                    CreatedDate = DateTime.UtcNow
                });

                var result = await _context.SaveChangesAsync();
                return new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// 토큰 리프래시
        /// </summary>
        /// <returns></returns>
        [HttpPost("Refresh")]
        [SwaggerResponse(200, "success", typeof(OkResult))]
        public async Task<IActionResult> RefreshTokenAsync([FromBody]RefreshTokenRequest refreshTokenRequest)
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}
