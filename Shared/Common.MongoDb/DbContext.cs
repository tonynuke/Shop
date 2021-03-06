using Common.MongoDb.Migrations;
using Domain;
using MongoDB.Driver;

namespace Common.MongoDb
{
    /// <summary>
    /// Database context.
    /// </summary>
    public abstract class DbContext
    {
        private readonly IMongoClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        /// <param name="database">Database.</param>
        protected DbContext(IMongoDatabase database)
        {
            Database = database;
            _client = database.Client;
        }

        /// <summary>
        /// Gets events collection.
        /// </summary>
        public virtual IMongoCollection<DomainEventBase> Events => Database.GetCollection<DomainEventBase>("events");

        /// <summary>
        /// Gets migration history.
        /// </summary>
        internal IMongoCollection<History> History => Database.GetCollection<History>("migrationsHistory");

        /// <summary>
        /// Gets database.
        /// </summary>
        protected IMongoDatabase Database { get; }

        /// <summary>
        /// Returns migrations.
        /// </summary>
        /// <returns>Migrations.</returns>
        public IReadOnlyCollection<IMigration> GetAllMigrations()
        {
            var contextType = GetType();
            return contextType.Assembly
                .GetTypes()
                .Where(type => type.BaseType != null
                               && type.BaseType.IsGenericType
                               && type.BaseType.GetGenericArguments().Contains(contextType))
                .Select(type => Activator.CreateInstance(type, this))
                .Cast<IMigration>()
                .ToList();
        }

        /// <summary>
        /// Executes <paramref name="func"/> in transaction.
        /// </summary>
        /// <param name="func">Func.</param>
        /// <returns>Asynchronous operation.</returns>
        public virtual async Task ExecuteInTransaction(Func<Task> func)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                await func();
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }

            await session.CommitTransactionAsync();
        }

        /// <summary>
        /// Executes <paramref name="func"/> in transaction with domain events.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="entity">Entity.</param>
        /// <param name="func">Func.</param>
        /// <returns>Asynchronous operation.</returns>
        public virtual async Task ExecuteInTransaction<TEntity>(TEntity entity, Func<Task> func)
            where TEntity : DomainEntity
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                await func();
                await Events.InsertManyAsync(entity.DomainEvents);
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }

            await session.CommitTransactionAsync();
        }
    }
}