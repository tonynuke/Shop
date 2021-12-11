using MediatR;

namespace Domain
{
    /// <summary>
    /// Base domain event.
    /// </summary>
    public abstract record DomainEventBase : INotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventBase"/> class.
        /// </summary>
        protected DomainEventBase()
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventBase"/> class.
        /// </summary>
        protected DomainEventBase(Guid entityId)
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
            ParentEntityId = entityId.ToString();
        }

        /// <summary>
        /// Id.
        /// </summary>
        /// <remarks>For deduplication purposes.</remarks>
        public Guid Id { get; private set; }

        /// <summary>
        /// Parent entity id.
        /// </summary>
        public string ParentEntityId { get; private set; }

        /// <summary>
        /// Gets creation time.
        /// </summary>
        public DateTime Created { get; private set; }
    }
}