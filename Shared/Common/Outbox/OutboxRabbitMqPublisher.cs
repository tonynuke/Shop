using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Domain;
using MassTransit;
using MongoDB.Driver;

namespace Common.Outbox
{
    /// <summary>
    /// Sends events from events collection to queue.
    /// </summary>
    public class OutboxRabbitMqPublisher
    {
        private const int BatchSize = 100;
        private readonly DbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutboxRabbitMqPublisher"/> class.
        /// </summary>
        /// <param name="dbContext">Context.</param>
        /// <param name="publishEndpoint">Publish endpoint.</param>
        public OutboxRabbitMqPublisher(DbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Publish events from events collection to queue.
        /// </summary>
        /// <returns>Asynchronous operation.</returns>
        public async Task Publish()
        {
            var events = await _dbContext.Events
                .Find(FilterDefinition<DomainEventBase>.Empty)
                .SortBy(e => e.Created)
                .Limit(BatchSize)
                .ToListAsync();

            // HACK: cast to actual event types to make rabbit mq routing work.
            await Task.WhenAll(events.Select(@event =>
            {
                var actualEventType = @event.GetType();
                var actualEvent = Convert.ChangeType(@event, actualEventType);
                return _publishEndpoint.Publish(actualEvent);
            }));

            var ids = events.Select(e => e.Id);
            await _dbContext.Events.DeleteManyAsync(e => ids.Contains(e.Id));
        }
    }
}