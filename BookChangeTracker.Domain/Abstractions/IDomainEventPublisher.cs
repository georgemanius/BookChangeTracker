namespace BookChangeTracker.Domain.Abstractions;

public interface IDomainEventPublisher
{
    void Subscribe<T>(Func<T, Task> handler) where T : class;
    Task PublishAsync<T>(T @event) where T : class;
}
