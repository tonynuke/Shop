using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Order.WebService.Dto;

namespace Order.WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        /// <summary>
        /// Makes an order.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns></returns>
        [HttpPost("make-an-order")]
        public async Task<IActionResult> MakeAnOrder(MakeAnOrder model)
        {
            throw new NotImplementedException();
        }
    }
}
