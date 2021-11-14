using Domain;
using MassTransit.KafkaIntegration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Outbox.Publisher.MassTransit
{

    /// <summary>
    /// MassTransit kafka publisher.
    /// </summary>
    public class KafkaPublisher : IPublisher
    {
        private readonly ITopicProducer<KafkaEventEnvelope> _topicProducer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaPublisher"/> class.
        /// </summary>
        /// <param name="topicProducer">Publish endpoint.</param>
        public KafkaPublisher(ITopicProducer<KafkaEventEnvelope> topicProducer)
        {
            _topicProducer = topicProducer;
        }

        /// <inheritdoc/>
        public async Task Publish(IEnumerable<DomainEventBase> events)
        {
            // HACK: cast to actual event types to make rabbit mq routing work.
            foreach (var @event in events)
            {
                var envelope = new KafkaEventEnvelope(@event);
                await _topicProducer.Produce(envelope);
            }
        }
    }
}