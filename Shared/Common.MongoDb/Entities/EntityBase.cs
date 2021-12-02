using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.MongoDb.Entities
{
    /// <summary>
    /// Base entity.
    /// </summary>
    public abstract class EntityBase : Entity<Guid>, IDomainEntity
    {
        private List<DomainEventBase> _domainEvents = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>
        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets domain events.
        /// </summary>
        [BsonIgnore]
        public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents;

        /// <summary>
        /// Gets optimistic concurrency control version.
        /// </summary>
        [BsonIgnoreIfDefault]
        [BsonElement("occVersion")]
        internal Guid OccVersion { get; private set; }

        private List<DomainEventBase> Events => _domainEvents ??= new List<DomainEventBase>();

        /// <summary>
        /// Adds domain event.
        /// </summary>
        /// <param name="domainEvent">Domain event.</param>
        public void AddEvent(DomainEventBase domainEvent)
        {
            Events.Add(domainEvent);
        }

        /// <summary>
        /// Updates occ version.
        /// </summary>
        internal void UpdateOccVersion()
        {
            OccVersion = Guid.NewGuid();
        }
    }
}