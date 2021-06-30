using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notifications.WebService.Dto;

namespace Notifications.WebService.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpPost("link")]
        public async Task LinkToDevice(LinkUserToDeviceDto model)
        {
            //var command = new LinkUserToDeviceCommand();
            //await _mediator.Send(command);
        }
    }
}