namespace BookChangeTracker.Domain.Models.Events;

public interface IDomainEvent
{
    DateTime RaisedAt { get; }
}
