using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebService.Controllers.V1
{
    /// <summary>
    /// Catalog controller.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
    }
}
