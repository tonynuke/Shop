using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Common.Outbox;
using DataAccess;
using DataAccess.Entities;
using Domain;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Xunit;
using TestUtils.Integration;
using Xunit.Abstractions;

namespace Tests
{
    public class OutboxServiceTests : MongoClientFixture
    {
        private readonly Fixture _fixture = new();

        public OutboxServiceTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        [Fact]
        public async Task Send_events()
        {
            var context = new TestContext(Database);
            var events = new List<DomainEventBase>
            {
                _fixture.Create<IntegerEvent>(),
                _fixture.Create<StringEvent>(),
            };
            await context.Events.InsertManyAsync(events);

            var service = new OutboxPublisher(context);
            await service.Send();
        }
    }

    public class TestContext : DbContext
    {
        public TestContext(IMongoDatabase database) : base(database)
        {
        }
    }

    [BsonDiscriminator("integer")]
    public class IntegerEvent : DomainEventBase
    {
        [BsonElement("int")]
        public int Int { get; set; }
    }

    [BsonDiscriminator("string")]
    public class StringEvent : DomainEventBase
    {
        [BsonElement("string")]
        public string String { get; set; }
    }

    public class TestConsumer :
        IMessageHandler<IntegerEvent>,
        IMessageHandler<StringEvent>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestConsumer(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public Task Handle(IntegerEvent message)
        {
            _testOutputHelper.WriteLine(message.Int.ToString());
            return Task.CompletedTask;
        }

        public Task Handle(StringEvent message)
        {
            _testOutputHelper.WriteLine(message.String);
            return Task.CompletedTask;
        }
    }
}
