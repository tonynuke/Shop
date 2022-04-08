using Domain;
using MassTransit;

namespace Common.Outbox.Publisher.MassTransit
{

    /// <summary>
    /// MassTransit kafka publisher.
    /// </summary>
    public class KafkaPublisher : IPublisher
    {
        private readonly ITopicProducer<EventEnvelope> _topicProducer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaPublisher"/> class.
        /// </summary>
        /// <param name="topicProducer">Publish endpoint.</param>
        public KafkaPublisher(ITopicProducer<EventEnvelope> topicProducer)
        {
            _topicProducer = topicProducer;
        }

        /// <inheritdoc/>
        public async Task Publish(IEnumerable<DomainEventBase> events)
        {
            // HACK: Cast to actual event types to make rabbit mq routing work.
            foreach (var @event in events)
            {
                var envelope = new EventEnvelope(@event);
                await _topicProducer.Produce(envelope);
            }
        }
    }
}