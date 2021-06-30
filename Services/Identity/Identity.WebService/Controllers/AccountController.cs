using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebService.Controllers
{
    /// <summary>
    /// Account controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interactionService;

        public AccountController(IIdentityServerInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await _interactionService.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                throw new NotImplementedException("External login is not implemented!");
            }

            return LocalRedirect($"/Identity/Account/Login?returnUrl={returnUrl}");
        }

        //[HttpGet("logout")]
        //public async Task<IActionResult> LogOut(string logoutId)
        //{
        //    return LocalRedirect("/Identity/Account/Logout");
        //}
    }
}
