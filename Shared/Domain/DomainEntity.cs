using CSharpFunctionalExtensions;

namespace Domain
{
    /// <summary>
    /// Domain entity.
    /// </summary>
    public abstract class DomainEntity : Entity<Guid>
    {
        private List<DomainEventBase> _domainEvents = new();

        // HACK: investigate this behaviour. Maybe mongodb serialization problem.
        protected List<DomainEventBase> DomainEventsInternal => _domainEvents ??= new List<DomainEventBase>();

        /// <summary>
        /// Gets domain events.
        /// </summary>
        public IReadOnlyCollection<DomainEventBase> DomainEvents => DomainEventsInternal;

        /// <summary>
        /// Adds domain event.
        /// </summary>
        /// <param name="domainEvent">Domain event.</param>
        public void AddEvent(DomainEventBase domainEvent)
        {
            DomainEventsInternal.Add(domainEvent);
        }
    }
}