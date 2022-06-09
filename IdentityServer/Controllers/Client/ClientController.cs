using Identity.Configuration.Models;
using Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Identity.Controllers.Client
{
    public class ClientController : ApiControllerBase
    {
        private readonly IIdentityContext _context;
        private readonly IAppSettings _appSettings;

        public ClientController(IIdentityContext context, IAppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        [HttpGet("Register")]
        public async Task<IActionResult> RegisterClientAsync()
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}
