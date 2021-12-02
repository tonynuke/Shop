using Common.MongoDb;
using MongoDB.Driver;

namespace Tests.ConfluentKafka
{
    public class TestContext : DbContext
    {
        public TestContext(IMongoDatabase database) : base(database)
        {
        }
    }
}