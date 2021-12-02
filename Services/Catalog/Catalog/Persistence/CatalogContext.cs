using Catalog.Brands;
using Catalog.Items;
using Common.MongoDb;
using MongoDB.Driver;

namespace Catalog.Persistence
{
    /// <summary>
    /// Catalog context.
    /// </summary>
    public class CatalogContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogContext"/> class.
        /// </summary>
        /// <param name="database">Database.</param>
        public CatalogContext(IMongoDatabase database)
            : base(database)
        {
        }

        /// <summary>
        /// Gets brands collection.
        /// </summary>
        public virtual IMongoCollection<Brand> Brands => Database.GetCollection<Brand>("brands");

        /// <summary>
        /// Gets items collection.
        /// </summary>
        public virtual IMongoCollection<CatalogItem> Items => Database.GetCollection<CatalogItem>("items");
    }
}