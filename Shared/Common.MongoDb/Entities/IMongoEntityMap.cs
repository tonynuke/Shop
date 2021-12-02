using MongoDB.Bson.Serialization;

namespace Common.MongoDb.Entities
{
    /// <summary>
    /// Mongo entity mapping interface.
    /// </summary>
    /// <typeparam name="TEntity">Entity.</typeparam>
    public interface IMongoEntityMap<TEntity>
    {
        /// <summary>
        /// Performs mapping.
        /// </summary>
        /// <param name="map">Mapping between a class and a BSON document.</param>
        void Map(BsonClassMap<TEntity> map);
    }
}