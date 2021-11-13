using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Common.Outbox.Publisher;
using Domain;
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

            var service = new EventsPublisher(context, new ConfluentKafkaPublisher());
            await service.Publish();

            var consumer = new TestConsumer(TestOutputHelper);
        }
    }
}
