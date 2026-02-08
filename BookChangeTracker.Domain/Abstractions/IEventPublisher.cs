namespace BookChangeTracker.Domain.Abstractions;

public interface IEventPublisher
{
    void Subscribe<T>(Func<T, Task> handler) where T : class;
    Task PublishAsync<T>(T @event) where T : class;
}
