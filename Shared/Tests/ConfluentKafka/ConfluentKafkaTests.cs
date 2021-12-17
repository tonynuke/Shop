using AutoFixture;
using Common.Outbox.Consumer;
using Common.Outbox.Consumer.Handlers;
using Common.Outbox.Publisher.Confluent;
using Confluent.Kafka;
using Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
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
                ClientId = "ASP.NET backend"
            };

            var producer = new ProducerBuilder<string, string>(producerConfig).Build();
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
        public async Task Consume_events_with_confluent_consumer()
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
            var services = new ServiceCollection();
            services.AddMediatR(typeof(IntegerEvent));
            services.AddSingleton(_testOutputHelper);
            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            var typeMap = new Dictionary<string, Type>()
            {
                {typeof(IntegerEvent).FullName, typeof(IntegerEvent)},
                {typeof(StringEvent).FullName, typeof(StringEvent)},
            };

            var config = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:29092",
                GroupId = "mediator-handler-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                Acks = Acks.All,
                AutoCommitIntervalMs = 5000,
            };

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var handler = new MediatorHandler(typeMap, mediator);
            var eventsConsumer = new EventsConsumer(config, TopicName, handler, Mock.Of<ILogger<EventsConsumer>>());
            eventsConsumer.Consume(cancellationToken);

            await Task.Delay(1000);
            cts.Cancel();
        }

        [Fact]
        public async Task Consume_single_event_type_with_custom_consumer_wrapper()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:29092",
                GroupId = "single-type-handler-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                Acks = Acks.All,
                AutoCommitIntervalMs = 5000,
            };

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var handler = new IntegerEventHandler(_testOutputHelper);
            var eventsConsumer = new EventsConsumer(config, TopicName, handler, Mock.Of<ILogger<EventsConsumer>>());
            eventsConsumer.Consume(cancellationToken);
        }

        public class IntegerEventHandler : SingleTypeHandler<IntegerEvent>
        {
            private readonly ITestOutputHelper _testOutputHelper;

            public IntegerEventHandler(ITestOutputHelper testOutputHelper)
            {
                _testOutputHelper = testOutputHelper;
            }

            protected override Task HandleInternal(IntegerEvent message)
            {
                _testOutputHelper.WriteLine(message.Int.ToString());
                return Task.CompletedTask;
            }
        }
    }
}
