using System.Collections.Concurrent;
using System.Collections.Immutable;
using BookChangeTracker.Domain.Abstractions;

namespace BookChangeTracker.Infrastructure.EventHandling.Services;

public class EventPublisher : IEventPublisher
{
    private readonly ConcurrentDictionary<Type, ImmutableList<Delegate>> _subscribers = new();

    public void Subscribe<T>(Func<T, Task> handler) where T : class
    {
        ArgumentNullException.ThrowIfNull(handler);

        _subscribers.AddOrUpdate(
            typeof(T),
            _ => [handler],
            (_, existingHandlers) => existingHandlers.Add(handler));
    }

    public async Task PublishAsync<T>(T @event) where T : class
    {
        ArgumentNullException.ThrowIfNull(@event);

        if (!_subscribers.TryGetValue(typeof(T), out var handlers)) return;

        var exceptions = new ConcurrentBag<Exception>();

        var tasks = handlers
            .OfType<Func<T, Task>>()
            .Select(async handler =>
            {
                try
                {
                    await handler(@event);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            });

        await Task.WhenAll(tasks);

        if (!exceptions.IsEmpty)
        {
            throw new AggregateException(
                $"One or more handlers failed while processing event {typeof(T).Name}",
                exceptions);
        }
    }
}
