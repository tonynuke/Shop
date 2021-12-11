using MongoDB.Driver;
using Specifications;

namespace Common.MongoDb
{
    /// <summary>
    /// Specifications extensions.
    /// </summary>
    public static class SpecificationsExtensions
    {
        /// <summary>Begins a fluent find interface.</summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="specification">The specification.</param>
        /// <param name="options">The options.</param>
        /// <returns>A fluent find interface.</returns>
        public static IFindFluent<TDocument, TDocument> FindBySpecification<TDocument>(
            this IMongoCollection<TDocument> collection,
            SpecificationBase<TDocument> specification,
            FindOptions? options = null)
        {
            return collection.Find(specification.ToExpression(), options);
        }

        /// <summary>
        /// Finds single entity.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="specification">The specification.</param>
        /// <param name="options">The options.</param>
        /// <returns>Entity or null.</returns>
        public static Task<TDocument> FindSingleOrDefault<TDocument>(
            this IMongoCollection<TDocument> collection,
            SpecificationBase<TDocument> specification,
            FindOptions options = null)
        {
            return collection.FindBySpecification(specification, options).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Finds many entities.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="specification">The specification.</param>
        /// <param name="options">The options.</param>
        /// <returns>Entities.</returns>
        public static async Task<IReadOnlyCollection<TDocument>> FindMany<TDocument>(
            this IMongoCollection<TDocument> collection,
            SpecificationBase<TDocument> specification,
            FindOptions options = null)
        {
            return await collection.FindBySpecification(specification, options).ToListAsync();
        }
    }
}