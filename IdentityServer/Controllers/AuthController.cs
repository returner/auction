using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IIdentityContext _context;
        public AuthController(IIdentityContext context)
        {
            _context = context;
        }
        [HttpGet("AuthTest")]
        [SwaggerResponse(200, "success", typeof(OkResult))]
        public async Task<IActionResult> AuthTest()
        {
            //await Task.CompletedTask;
            //return new OkObjectResult("good");

            var data = await _context.AdminUsers.ToArrayAsync();
            return new OkObjectResult(data);
        }

        [HttpGet("CreateDefault")]
        [SwaggerResponse(200, "success", typeof(OkResult))]
        public async Task<IActionResult> CreateUser()
        {
            await _context.AdminUsers.AddAsync(new AdminUser
            {
                UserId = "admin",
                Password = "1111",
                Name = "administrator",
                Email = "test@test.com",
                CreatedDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return new OkObjectResult("good");

        }
    }
}
