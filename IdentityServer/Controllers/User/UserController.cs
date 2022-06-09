using Identity.Configuration.Models;
using Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Identity.Controllers.User
{
    public class UserController : ApiControllerBase
    {
        private readonly IIdentityContext _context;
        private readonly IAppSettings _appSettings;

        public UserController(IIdentityContext context, IAppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
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
    }
}
