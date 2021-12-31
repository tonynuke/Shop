using AutoFixture;
using Common.Outbox.Consumer;
using Common.Outbox.Consumer.Handlers;
using Common.Outbox.Publisher.Confluent;
using Confluent.Kafka;
using Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tests.ConfluentKafka.Events;
using Xunit;
using Xunit.Abstractions;

namespace Tests.ConfluentKafka
{
    public class ConfluentKafkaTests : IAsyncLifetime
    {
        private const string TopicName = "topic";
        private readonly Fixture _fixture = new();
        private readonly KafkaPublisher _kafkaPublisher;
        private readonly ITestOutputHelper _testOutputHelper;

        public ConfluentKafkaTests(ITestOutputHelper testOutputHelper)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "127.0.0.1:29092",
                ClientId = "ASP.NET backend",
            };

            var producer = new ProducerBuilder<string, string>(producerConfig)
                .Build();
            _kafkaPublisher = new KafkaPublisher(producer, TopicName);

            _testOutputHelper = testOutputHelper;
        }

        public Task DisposeAsync()
        {
            _kafkaPublisher.Dispose();
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            var events = new List<DomainEventBase>
            {
                _fixture.Create<IntegerEvent>(),
                //_fixture.Create<StringEvent>(),
            };

            await _kafkaPublisher.Publish(events);
        }

        [Fact]
        public void Consume_events_with_confluent_consumer()
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var config = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:29092",
                GroupId = "confluent-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                Acks = Acks.All,
                AutoCommitIntervalMs = 5000,
            };

            var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(TopicName);

            var consumeResult = consumer.Consume(cancellationToken);
            _testOutputHelper.WriteLine(consumeResult.Message.Value);
            consumer.Commit(consumeResult);

            consumer.Close();
        }

        [Fact]
        public async Task Consume_events_with_custom_consumer_wrapper()
        {
            var provider = new ServiceCollection()
                .AddLogging(config => config.AddXunit(_testOutputHelper))
                .AddMediatR(typeof(StringEvent))
                .BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            var config = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:29092",
                GroupId = "mediator-handler-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                Acks = Acks.All,
            };

            var handler = new MediatorHandler(mediator);
            var eventsConsumer = new CosnumerBackgroundService(
                config, TopicName, handler, provider.GetRequiredService<ILogger<CosnumerBackgroundService>>());

            await ExecuteService(eventsConsumer);
        }

        [Fact]
        public async Task Consume_single_event_type_with_custom_consumer_wrapper()
        {
            var provider = new ServiceCollection()
                .AddLogging(config => config.AddXunit(_testOutputHelper))
                .BuildServiceProvider();

            var config = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:29092",
                GroupId = "single-type-handler-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                Acks = Acks.All,
            };

            var handler = new IntegerEventHandler(_testOutputHelper);
            var eventsConsumer = new CosnumerBackgroundService(
                config, TopicName, handler, provider.GetRequiredService<ILogger<CosnumerBackgroundService>>());

            await ExecuteService(eventsConsumer);
        }

        private static async Task ExecuteService(CosnumerBackgroundService service)
        {
            await service.StartAsync(CancellationToken.None);
            await Task.Delay(2000);
            await service.StopAsync(CancellationToken.None);
        }
    }
}
