using System.Linq;
using System.Threading.Tasks;
using Common.MongoDb;
using Domain;
using MongoDB.Driver;

namespace Common.Outbox.Publisher
{
    /// <summary>
    /// Sends events from events collection to queue.
    /// </summary>
    public class EventsPublisher
    {
        private const int BatchSize = 100;
        private readonly DbContext _dbContext;
        private readonly IPublisher _publisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsPublisher"/> class.
        /// </summary>
        /// <param name="dbContext">Context.</param>
        /// <param name="publisher">Publisher.</param>
        public EventsPublisher(DbContext dbContext, IPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }

        /// <summary>
        /// Sends events from events collection to queue.
        /// </summary>
        /// <returns>Asynchronous operation.</returns>
        public async Task Publish()
        {
            var events = await _dbContext.Events
                .Find(FilterDefinition<DomainEventBase>.Empty)
                .SortBy(e => e.Created)
                .Limit(BatchSize)
                .ToListAsync();

            await _publisher.Publish(events);

            var ids = events.Select(e => e.Id);
            await _dbContext.Events.DeleteManyAsync(e => ids.Contains(e.Id));
        }
    }
}