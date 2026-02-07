namespace BookChangeTracker.Models.Domain.Events;

public interface IBookChangeEvent : IDomainEvent
{
    ChangeType ChangeType { get; }
}
