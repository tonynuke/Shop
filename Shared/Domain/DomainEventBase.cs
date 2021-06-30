using System;
using CSharpFunctionalExtensions;

namespace Domain
{
    /// <summary>
    /// Base domain event.
    /// </summary>
    public abstract class DomainEventBase : Entity<Guid>
    {
    }
}