using Common.MongoDb;
using MongoDB.Driver;

namespace Basket.Persistence
{
    /// <summary>
    /// Basket context.
    /// </summary>
    public class BasketContext : DbContext
    {
        public BasketContext(IMongoDatabase database) : base(database)
        {
        }

        public virtual IMongoCollection<Domain.Basket> Baskets => Database.GetCollection<Domain.Basket>("baskets");
    }
}