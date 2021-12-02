using Domain;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Outbox.Publisher.MassTransit
{
    /// <summary>
    /// MassTransit publisher.
    /// </summary>
    public class RabbitMqPublisher : IPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqPublisher"/> class.
        /// </summary>
        /// <param name="publishEndpoint">Publish endpoint.</param>
        public RabbitMqPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        /// <inheritdoc/>
        public async Task Publish(IEnumerable<DomainEventBase> events)
        {
            // HACK: cast to actual event types to make rabbit mq routing work.
            foreach (var @event in events)
            {
                var actualEventType = @event.GetType();
                var actualEvent = Convert.ChangeType(@event, actualEventType);
                await _publishEndpoint.Publish(actualEvent);
            }
        }
    }
}