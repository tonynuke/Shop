using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using DataAccess;
using Domain;
using MongoDB.Driver;

namespace Common.AspNetCore.Outbox
{
    /// <summary>
    /// Sends events from events collection to queue.
    /// </summary>
    /// TODO: Look at MongoDb change streams
    /// https://debezium.io/
    /// https://docs.mongodb.com/kafka-connector/current/
    public class OutboxService : IDisposable
    {
        private const int BatchSize = 100;
        private const string TopicName = "topicName";
        private readonly DbContext _dbContext;
        private readonly IProducer<Null, string> _producer;
        private readonly ProducerConfig _producerConfig = new ()
        {
            // TODO: move to the settings class
            BootstrapServers = "127.0.0.1:29092",
            ClientId = "ASP.NET backend",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="OutboxService"/> class.
        /// </summary>
        /// <param name="dbContext">Context.</param>
        public OutboxService(DbContext dbContext)
        {
            _dbContext = dbContext;
            _producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
        }

        /// <summary>
        /// Sends events from events collection to queue.
        /// </summary>
        /// <returns>Asynchronous operation.</returns>
        public async Task Send()
        {
            var events = await _dbContext.Events
                .Find(FilterDefinition<DomainEventBase>.Empty)
                .SortBy(e => e.Created)
                .Limit(BatchSize)
                .ToListAsync();

            var messages = events.Select(@event =>
                new Message<Null, string>
                {
                    // specify object type to serialize child classes properties
                    Value = JsonSerializer.Serialize<object>(@event),
                });

            foreach (var message in messages)
            {
                await _producer.ProduceAsync(TopicName, message);
            }

            var ids = events.Select(e => e.Id);
            await _dbContext.Events.DeleteManyAsync(e => ids.Contains(e.Id));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}