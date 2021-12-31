using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notifications.WebService.Dto;

namespace Notifications.WebService.Controllers.Integration.V1
{
    /// <summary>
    /// Users controller.
    /// </summary>
    /// <remarks>Only for testing purpose.</remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/internal/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="mediator">Mediator.</param>
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Links user to device.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Asynchronous operation.</returns>
        [HttpPost("link")]
        [MapToApiVersion("1.0")]
        public async Task LinkToDevice(LinkUserToDeviceDto model)
        {
            //var command = new LinkUserToDeviceCommand();
            //await _mediator.Send(command);
        }

        /// <summary>
        /// Unlinks user to device.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Asynchronous operation.</returns>
        [HttpPost("unlink")]
        public async Task UnlinkFromDevice(UnlinkUserFromDeviceDto model)
        {
            //var command = new UnlinkUserFromDeviceCommand();
            //await _mediator.Send(command);
        }
    }
}
