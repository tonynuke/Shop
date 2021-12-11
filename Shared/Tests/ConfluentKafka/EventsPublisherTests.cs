using AutoFixture;
using Common.Outbox.Consumer;
using Common.Outbox.Publisher;
using Common.Outbox.Publisher.Confluent;
using Confluent.Kafka;
using Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestUtils.Integration;
using Xunit;
using Xunit.Abstractions;

namespace Tests.ConfluentKafka
{
    public class EventsPublisherTests : MongoClientFixture
    {
        private readonly Fixture _fixture = new();

        public EventsPublisherTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        [Fact]
        public async Task Publish_events()
        {
            var context = new TestContext(Database);
            var events = new List<DomainEventBase>
            {
                _fixture.Create<IntegerEvent>(),
                _fixture.Create<StringEvent>(),
            };
            await context.Events.InsertManyAsync(events);

            var topic = "topic";
            var producerConfig = new ProducerConfig
            {
                // TODO: move to the settings class
                BootstrapServers = "127.0.0.1:29092",
                ClientId = "ASP.NET backend",
            };

            var service = new EventsPublisher(context, new KafkaPublisher(producerConfig, topic));
            await service.Publish();

            var config = new ConsumerConfig
            {
                BootstrapServers = "127.0.0.1:29092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            var services = new ServiceCollection();
            services.AddMediatR(typeof(IntegerEvent));
            services.AddSingleton<ITestOutputHelper>(TestOutputHelper);
            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            var typeMap = new Dictionary<string, Type>()
            {
                {typeof(IntegerEvent).FullName, typeof(IntegerEvent)},
                {typeof(StringEvent).FullName, typeof(StringEvent)},
            };
            var eventsConsumer = new EventsConsumer(config, topic, mediator, typeMap);
            await Task.Run(() => eventsConsumer.Consume(System.Threading.CancellationToken.None));
        }
    }
}
