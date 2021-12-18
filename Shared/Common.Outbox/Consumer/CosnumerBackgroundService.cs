using Common.Outbox.Consumer.Handlers;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common.Outbox.Consumer
{
    public class CosnumerBackgroundService : BackgroundService
    {
        private readonly TimeSpan _recoveryInterval = TimeSpan.FromSeconds(1);
        private readonly IReadOnlyCollection<string> _topics;
        private readonly ConsumerConfig _config;
        private readonly IConsumeResultHandler<string, string> _handler;
        private readonly ILogger<CosnumerBackgroundService> _logger;

        public CosnumerBackgroundService(
            ConsumerConfig config,
            string topic,
            IConsumeResultHandler<string, string> handler,
            ILogger<CosnumerBackgroundService> logger)
        {
            _topics = new[] { topic };
            _handler = handler;
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            _logger.LogInformation("Execution started");

            using (var consumer = new ConsumerBuilder<string, string>(_config).Build())
            {
                // Doesn't throw exception if Kafka is unavailable.
                consumer.Subscribe(_topics);
                _logger.LogInformation("{consumerGroup} subscribed to topic(s) : {topic}", consumer.Name, _topics);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            _logger.LogInformation("{consumerGroup} begin consume", consumer.Name);
                            var consumeResult = consumer.Consume(cancellationToken);
                            _logger.LogInformation("Message offset {offset} consumed", consumeResult.Offset);

                            _logger.LogInformation("{consumerGroup} begin handle", consumer.Name);
                            await _handler.Handle(consumeResult, cancellationToken);
                            _logger.LogInformation("Message offset {offset} handled", consumeResult.Offset);

                            consumer.Commit(consumeResult);
                        }
                        catch (KafkaException ex)
                        {
                            // TODO: Add polly.
                            _logger.LogError("Temporary error {error}. Recovering in {recoveryInterval}", ex, _recoveryInterval);
                            await Task.Delay(_recoveryInterval);
                        }
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogInformation("Execution cacneled", ex);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.Message, ex);
                }

                consumer.Close();
            }

            _logger.LogInformation("Execution stopped");
        }
    }
}
