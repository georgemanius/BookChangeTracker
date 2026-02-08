namespace BookChangeTracker.Domain.Abstractions;

public interface IEventDispatcher
{
    Task DispatchAsync(IDomainEvent @event);
}
