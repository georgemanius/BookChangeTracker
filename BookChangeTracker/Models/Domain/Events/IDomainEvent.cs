namespace BookChangeTracker.Models.Domain.Events;

public interface IDomainEvent
{
    DateTime RaisedAt { get; }
}
