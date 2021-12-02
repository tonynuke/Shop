using System;
using System.Text.Json;
using System.Threading;
using Confluent.Kafka;

namespace Common.Outbox.Consumer
{
    /// <summary>
    /// Consumer.
    /// </summary>
    public class EventsConsumer : IDisposable
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsConsumer"/> class.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <param name="groupId">Group id.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public EventsConsumer(string topic, string groupId, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var config = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:29092",
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe(topic);
        }

        public void Consume(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var body = consumeResult.Message.Value;

                var @event = JsonSerializer.Deserialize<object>(body);
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