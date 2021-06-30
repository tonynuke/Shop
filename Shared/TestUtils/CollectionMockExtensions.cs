using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace TestUtils
{
    /// <summary>
    /// Mongo collection mock extensions.
    /// </summary>
    public static class CollectionMockExtensions
    {
        /// <summary>
        /// Makes <paramref name="collectionMock"/> return <paramref name="entity"/>.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="collectionMock">Collection mock.</param>
        /// <param name="entity">Entity.</param>
        public static void SetupCursorResponse<TEntity>(
            this Mock<IMongoCollection<TEntity>> collectionMock,
            params TEntity[] entity)
        {
            var asyncCursor = new Mock<IAsyncCursor<TEntity>>();
            asyncCursor
                .Setup(cursor => cursor.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            asyncCursor
                .SetupGet(cursor => cursor.Current)
                .Returns(entity);

            collectionMock
                .Setup(collection => collection.FindAsync(
                    It.IsAny<FilterDefinition<TEntity>>(),
                    It.IsAny<FindOptions<TEntity>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(asyncCursor.Object);
        }

        public static void SetupUpdateOneResult<TEntity>(this Mock<IMongoCollection<TEntity>> collectionMock)
        {
            var updateResult = new UpdateResult.Acknowledged(1, 1, new BsonDocument());

            collectionMock.Setup(r => r.UpdateOneAsync(
                    It.IsAny<FilterDefinition<TEntity>>(),
                    It.IsAny<UpdateDefinition<TEntity>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(updateResult);
        }
    }
}