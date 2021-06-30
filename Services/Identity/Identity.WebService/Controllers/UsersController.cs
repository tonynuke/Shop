using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Domain;
using Identity.WebService.Dto;
using IdentityServer4;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.WebService.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IdentityServerTools _identityServerTools;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UsersController(
            IdentityServerTools identityServerTools,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<UsersController> logger)
        {
            _identityServerTools = identityServerTools;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Signs up a new user.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Asynchronous operation.</returns>
        [HttpPost("sign-up")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var user = new User
            {
                UserName = model.Email,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                var token = await GenerateToken(user);
                return Ok(new UserModel
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Token = token
                });
            }

            var errors = result.Errors.ToDictionary(
                error => error.Code,
                error => new[] { error.Description });
            var validationProblemDetails = new ValidationProblemDetails(errors);
            return ValidationProblem(validationProblemDetails);
        }

        /// <summary>
        /// Signs in a user.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Asynchronous operation.</returns>
        [HttpPost("log-in")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogIn(SignInModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            var token = await GenerateToken(user);
            return result.Succeeded
                ? Ok(new UserModel
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Token = token
                })
                : BadRequest();
        }

        /// <summary>
        /// Signs out a user.
        /// </summary>
        /// <returns>Asynchronous operation.</returns>
        [HttpPost("log-out")]
        public new async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        private async Task<string> GenerateToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            userClaims.Add(new Claim("sub", user.Id));
            return await _identityServerTools.IssueJwtAsync(300, userClaims);
        }

        //private void AddTokenToCookies(string token)
        //{
        //    Response.Cookies.Append(
        //        CookiesToTokenMiddleware.CookiesName, token, new CookieOptions()
        //        {
        //            HttpOnly = true,
        //        });
        //}
    }
}
