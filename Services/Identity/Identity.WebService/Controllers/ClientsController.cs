using System.Threading.Tasks;
using Identity.Services;
using Identity.WebService.Dto;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebService.Controllers
{
    /// <summary>
    /// Clients controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientsService _clientsService;

        public ClientsController(ClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        /// <summary>
        /// Creates service client.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Asynchronous operation.</returns>
        [HttpPost("service")]
        public async Task CreateServiceClient(CreateServiceClientModel model)
        {
            await _clientsService.CreateServiceClient(model.Name, model.Secret, model.Scopes);
        }

        /// <summary>
        /// Creates client.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Asynchronous operation.</returns>
        [HttpPost]
        public async Task CreateClient(CreateClientModel model)
        {
            await _clientsService.CreateClient(
                model.Name,
                model.Scopes,
                new[] { "https://localhost:5001/swagger/oauth2-redirect.html" });
        }

        /// <summary>
        /// Returns client by id.
        /// </summary>
        /// <param name="clientId">Client id.</param>
        /// <returns>Client.</returns>
        [HttpGet("{clientId}")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClient(string clientId)
        {
            var client = await _clientsService.FindClientByIdAsync(clientId);
            return client != null
                ? Ok(client)
                : NotFound();
        }
    }
}