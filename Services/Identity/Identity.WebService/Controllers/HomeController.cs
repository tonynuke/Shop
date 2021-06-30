using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;

        public HomeController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        [HttpGet("error")]
        public async Task<IActionResult> Error(string errorId)
        {
            var message = await _interaction.GetErrorContextAsync(errorId);
            return BadRequest(message);
        }
    }
}
