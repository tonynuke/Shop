using System.Threading.Tasks;
using Common.Outbox;
using Common.Outbox.Publisher;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebService.Controllers.V1
{
    /// <summary>
    /// Catalog items controller.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EventsPublisherController : ControllerBase
    {
        private readonly EventsPublisher _publisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsPublisherController"/> class.
        /// </summary>
        /// <param name="publisher">Publisher.</param>
        public EventsPublisherController(EventsPublisher publisher)
        {
            _publisher = publisher;
        }

        /// <summary>
        /// Publish events.
        /// </summary>
        /// <returns>Asynchronous operation.</returns>
        [HttpPost("process")]
        public async Task ProcessEvents()
        {
            await _publisher.Publish();
        }
    }
}
