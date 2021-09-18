using System.Threading.Tasks;
using DataAccess.Entities;
using MongoDB.Driver;

namespace DataAccess
{
    /// <summary>
    /// Optimistic concurrency control extensions.
    /// </summary>
    /// <remarks>May lead to false results if the entity has been deleted.</remarks>
    public static class OccExtensions
    {
        /// <summary>
        /// Replaces a single document with OCC.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="collection">Collection.</param>
        /// <param name="entity">Entity.</param>
        /// <returns>Asynchronous operation.</returns>
        /// <exception cref="MongoDbConcurrencyException">Exception.</exception>
        public static async Task ReplaceOneOcc<TEntity>(
            this IMongoCollection<TEntity> collection,
            TEntity entity)
            where TEntity : EntityBase
        {
            var filter = CreateFilter(entity);
            entity.UpdateOccVersion();

            var result = await collection.ReplaceOneAsync(filter, entity);
            if (result.ModifiedCount == 0)
            {
                throw new MongoDbConcurrencyException();
            }
        }

        /// <summary>
        /// Updates a single document with OCC.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="collection">Collection.</param>
        /// <param name="entity">Entity.</param>
        /// <param name="update">Update.</param>
        /// <returns>Asynchronous operation.</returns>
        /// <exception cref="MongoDbConcurrencyException">Exception.</exception>
        public static async Task UpdateOneOcc<TEntity>(
            this IMongoCollection<TEntity> collection,
            TEntity entity,
            UpdateDefinition<TEntity> update)
            where TEntity : EntityBase
        {
            var filter = CreateFilter(entity);
            entity.UpdateOccVersion();

            var internalUpdate = update.Set(e => e.OccVersion, entity.OccVersion);
            var result = await collection.UpdateOneAsync(filter, internalUpdate);
            if (result.ModifiedCount == 0)
            {
                throw new MongoDbConcurrencyException();
            }
        }

        private static FilterDefinition<TEntity> CreateFilter<TEntity>(TEntity entity)
            where TEntity : EntityBase
        {
            var previousVersion = entity.OccVersion;

            var versionFilter =
                Builders<TEntity>.Filter.Exists(e => e.OccVersion, false)
                | Builders<TEntity>.Filter.Where(e => e.OccVersion == previousVersion);

            var idFilter = Builders<TEntity>.Filter.Where(e => e.Id == entity.Id);

            return versionFilter & idFilter;
        }
    }
}