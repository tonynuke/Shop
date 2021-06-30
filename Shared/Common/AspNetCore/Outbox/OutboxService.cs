using System.Threading.Tasks;
using Confluent.Kafka;
using DataAccess;
using DataAccess.Entities;
using MongoDB.Driver;

namespace Common.AspNetCore.Outbox
{
    /// <summary>
    /// 
    /// </summary>
    /// Look at MongoDb change streams https://debezium.io/
    /// https://docs.mongodb.com/kafka-connector/current/
    public class OutboxService
    {
        private readonly DbContext _dbContext;
        private readonly IProducer<Null, string> _producer;
        private readonly ProducerConfig _producerConfig = new ();

        public OutboxService(DbContext dbContext)
        {
            _dbContext = dbContext;
            _producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
        }

        //public async Task Send()
        //{
        //    var events = _dbContext.Events.Watch()
        //        .Find(FilterDefinition<DomainEventBase>.Empty)
        //        .SortBy(e => e.Id)
        //        .Limit(1000);

        //    foreach (var e in events)
        //    {
        //        await _producer.ProduceAsync("topicName", new Message<Null, string>()
        //        {
        //            Value = 
        //        });
        //    }
        //}
    }
}