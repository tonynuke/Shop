using System;
using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// Domain entity interface.
    /// </summary>
    public interface IDomainEntity
    {
        /// <summary>
        /// Gets id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets domain events.
        /// </summary>
        public IReadOnlyCollection<DomainEventBase> DomainEvents { get; }
    }
}