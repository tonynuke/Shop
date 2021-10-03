using System;
using CSharpFunctionalExtensions;

namespace Domain
{
    //public interface IDomainEvent
    //{
    //    /// <summary>
    //    /// Gets creation time.
    //    /// </summary>
    //    DateTime Created { get; }

    //    Guid Id { get; }
    //}

    /// <summary>
    /// Base domain event.
    /// </summary>
    public abstract class DomainEventBase : Entity<Guid>//, IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventBase"/> class.
        /// </summary>
        protected DomainEventBase()
        {
            Created = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets creation time.
        /// </summary>
        public DateTime Created { get; private set; }
    }
}