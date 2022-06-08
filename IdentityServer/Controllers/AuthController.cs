using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class AuthController : ControllerBase
    {
        [HttpGet("AuthTest")]
        [SwaggerResponse(200, "success", typeof(OkResult))]
        public async Task<IActionResult> AuthTest()
        {
            await Task.CompletedTask;
            return new OkObjectResult("good");
        }
    }
}
