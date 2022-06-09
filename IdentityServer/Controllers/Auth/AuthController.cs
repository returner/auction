using Identity.Controllers.Auth.Dtos;
using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Identity.Controllers.Auth
{
    public class AuthController : ApiControllerBase
    {
        private readonly IIdentityContext _context;
        public AuthController(IIdentityContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 사용자 정보 조회
        /// </summary>
        /// <returns></returns>
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
