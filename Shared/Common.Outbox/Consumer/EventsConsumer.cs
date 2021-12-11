using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.Outbox.Consumer
{
    /// <summary>
    /// Consumer.
    /// </summary>
    public class EventsConsumer : IDisposable
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IMediator _mediator;
        private readonly IReadOnlyDictionary<string, Type> _typeMap;
        private readonly ILogger<EventsConsumer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsConsumer"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="topic">Topic.</param>
        /// <param name="mediator">Service provider.</param>
        public EventsConsumer(
            ConsumerConfig config, string topic, IMediator mediator, IReadOnlyDictionary<string, Type> typeMap)
        {
            _mediator = mediator;
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe(topic);
            _typeMap = typeMap;
        }

        public async Task Consume(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var typeHeader = consumeResult.Message.Headers.SingleOrDefault(x => x.Key == "Type");

                if (typeHeader == null)
                {
                    // TODO: add error handling.
                    _consumer.Commit();
                    continue;
                }

                var typeHeaderBytes = typeHeader.GetValueBytes();
                var typeName = Encoding.UTF8.GetString(typeHeaderBytes);
                var eventType = _typeMap[typeName];
                if (eventType == null)
                {
                    // TODO: add error handling.
                    _consumer.Commit();
                    continue;
                }

                var body = consumeResult.Message.Value;
                var @event = JsonSerializer.Deserialize(body, eventType);

                await _mediator.Publish(@event, cancellationToken);

                _consumer.Commit();
            }

            _consumer.Close();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _consumer?.Dispose();
        }
    }
}