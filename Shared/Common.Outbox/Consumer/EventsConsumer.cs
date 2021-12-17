using Common.Outbox.Consumer.Handlers;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Common.Outbox.Consumer
{
    /// <summary>
    /// Consumer.
    /// </summary>
    public class EventsConsumer : IDisposable
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IConsumeResultHandler<string, string> _handler;
        private readonly ILogger<EventsConsumer> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsConsumer"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="topic">Topic.</param>
        /// <param name="handler">Handler.</param>
        /// <param name="logger">Logger.</param>
        public EventsConsumer(
            ConsumerConfig config,
            string topic,
            IConsumeResultHandler<string, string> handler,
            ILogger<EventsConsumer> logger)
        {
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe(topic);
            _handler = handler;
            _logger = logger;
        }

        public async Task Consume(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                try
                {
                    await _handler.Handle(consumeResult, cancellationToken);
                    _consumer.Commit(consumeResult);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    throw;
                }
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