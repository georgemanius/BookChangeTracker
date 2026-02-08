using BookChangeTracker.Domain.Models;

namespace BookChangeTracker.Domain.Abstractions;

public interface IBookChangeEvent : IDomainEvent
{
    ChangeType ChangeType { get; }
}
