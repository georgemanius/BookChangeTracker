namespace BookChangeTracker.Domain.Models.Events;

public interface IBookChangeEvent : IDomainEvent
{
    ChangeType ChangeType { get; }
}
