namespace BookChangeTracker.Infrastructure;

/// To keep the design simple and straightforward for the sake of this exercise - in-memory domain event bus.
/// Ideally, we would use a DI-aware publisher that resolves handlers from IServiceProvider.
/// TODO: make thread-safe
public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = [];

    public void Subscribe<T>(Func<T, Task> handler) where T : class
    {
        var eventType = typeof(T);
        if (!_subscribers.TryGetValue(eventType, out List<Delegate>? value))
        {
            value = [];
            _subscribers[eventType] = value;
        }

        value.Add(handler);
    }

    public async Task PublishAsync<T>(T @event) where T : class
    {
        if (_subscribers.TryGetValue(typeof(T), out var handlers))
        {
            foreach (var handler in handlers)
            {
                if (handler is Func<T, Task> func)
                {
                    await func(@event);
                }
            }
        }
    }
}
