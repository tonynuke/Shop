using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MongoDB.Driver;

namespace DataAccess.Migrations.Services
{
    /// <summary>
    /// Migrations runner.
    /// </summary>
    /// <remarks>
    /// Unsafe if same migrations will be executed simultaneously.
    /// </remarks>
    public class MigrationsRunner
    {
        private readonly IMongoCollection<History> _history;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationsRunner"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public MigrationsRunner(DbContext dbContext)
        {
            _history = dbContext.History;
        }

        /// <summary>
        /// Returns last applied migration history.
        /// </summary>
        /// <returns>History.</returns>
        public Task<History> GetLastAppliedMigrationId()
        {
            return _history
                .Find(FilterDefinition<History>.Empty)
                .SortByDescending(history => history.FinishDate)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Applies migration.
        /// </summary>
        /// <param name="migration">Migration.</param>
        /// <returns>Asynchronous operation.</returns>
        public async Task<Result> ApplyMigration(IMigration migration)
        {
            var dbMigration = await _history.Find(h => h.MigrationId == migration.Id).SingleOrDefaultAsync();
            if (dbMigration != null)
            {
                return Result.Failure($"Migration {migration.Id} is already applied.");
            }

            await migration.Up();
            var history = new History(migration.Id);
            await _history.InsertOneAsync(history);

            return Result.Success();
        }

        /// <summary>
        /// Applies migrations.
        /// </summary>
        /// <param name="migrations">Migrations.</param>
        /// <returns>Asynchronous operation.</returns>
        /// <remarks>Applied migrations will be ignored.</remarks>
        public async Task ApplyMigrations(IEnumerable<IMigration> migrations)
        {
            foreach (var migration in migrations)
            {
                await ApplyMigration(migration);
            }
        }

        /// <summary>
        /// Reverts migration.
        /// </summary>
        /// <param name="migration">Migration.</param>
        /// <returns>Asynchronous operation.</returns>
        public async Task RevertMigration(IMigration migration)
        {
            // TODO:
            await migration.Down();
        }

        //private static string GetMigrationId(IMigration migration)
        //{
        //    var type = migration.GetType();
        //    var attribute = (MigrationAttribute)Attribute.GetCustomAttribute(type, typeof(MigrationAttribute));
        //    return attribute!.Id;
        //}
    }
}