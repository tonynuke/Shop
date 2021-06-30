using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebService.Controllers.V1
{
    /// <summary>
    /// Orders controller.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class OrdersController : ControllerBase
    {
    }
}
