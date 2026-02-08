namespace BookChangeTracker.Domain.Abstractions;

public interface IDomainEvent
{
    DateTime RaisedAt { get; }
}
