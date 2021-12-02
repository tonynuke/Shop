using Common.MongoDb;
using MongoDB.Driver;
using Notifications.Domain;

namespace Notifications.Persistence
{
    /// <summary>
    /// Notifications db context.
    /// </summary>
    public class NotificationsContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsContext"/> class.
        /// </summary>
        /// <param name="database">Database.</param>
        public NotificationsContext(IMongoDatabase database)
            : base(database)
        {
        }

        /// <summary>
        /// Gets users.
        /// </summary>
        public virtual IMongoCollection<User> Users => Database.GetCollection<User>("users");
    }
}