using System;
using CSharpFunctionalExtensions;

namespace Domain
{
    /// <summary>
    /// Base domain event.
    /// </summary>
    public abstract class DomainEventBase : Entity<Guid>
    {
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