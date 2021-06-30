using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Domain;
using Identity.WebService.Dto;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly ConfigurationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ResourcesController(
            ConfigurationDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("api-resource")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreateApiResource()
        {
            //var apiResource = new ApiResource()
            //{
            //    Name = "identity",
            //    Scopes = new List<ApiResourceScope>(),
            //};
            //var apiResourceScope = new ApiResourceScope()
            //{
            //    ApiResource = apiResource,
            //    ApiResourceId = apiResource.Id,
            //    Scope = "full",
            //};
            var apiScope = new ApiScope()
            {
                Name = "identity"
            };
            await _context.ApiScopes.AddAsync(apiScope);

            var apiScope1 = new ApiScope()
            {
                Name = "system"
            };
            await _context.ApiScopes.AddAsync(apiScope1);

            //apiResource.Scopes.Add(apiResourceScope);
            //await _context.ApiResources.AddAsync(apiResource);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("create-role")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreateRole(CreateRoleModel model)
        {
            var identityRole = new IdentityRole()
            {
                Name = model.RoleName
            };
            await _roleManager.CreateAsync(identityRole);

            return NoContent();
        }

        [HttpPost("add-claim-to-role")]
        public async Task<IActionResult> AddClaimToRole(string role, string type, string value)
        {
            var r = await _roleManager.FindByNameAsync(role);
            var claim = new Claim(type, value);
            await _roleManager.AddClaimAsync(r, claim);

            return Ok();
        }

        [HttpPost("add-user-to-role")]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            await _userManager.AddToRoleAsync(user, model.Role);

            return Ok();
        }
    }
}
