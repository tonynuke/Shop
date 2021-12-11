using Domain;

namespace Common.MongoDb.Entities
{
    /// <summary>
    /// Base entity.
    /// </summary>
    public abstract class EntityBase : DomainEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>
        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets optimistic concurrency control version.
        /// </summary>
        internal Guid OccVersion { get; private set; }

        /// <summary>
        /// Updates occ version.
        /// </summary>
        internal void UpdateOccVersion()
        {
            OccVersion = Guid.NewGuid();
        }
    }
}