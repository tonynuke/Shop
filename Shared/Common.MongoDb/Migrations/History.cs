using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.MongoDb.Migrations
{
    /// <summary>
    /// History about migration.
    /// </summary>
    public class History
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class.
        /// </summary>
        /// <param name="migrationId">Migration identifier.</param>
        public History(string migrationId)
        {
            MigrationId = migrationId ?? throw new ArgumentNullException(nameof(migrationId));
            FinishDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets migration identifier.
        /// </summary>
        [BsonId]
        public string MigrationId { get; private set; }

        /// <summary>
        /// Gets finish date.
        /// </summary>
        [BsonElement("finishDate")]
        public DateTime FinishDate { get; private set; }
    }
}