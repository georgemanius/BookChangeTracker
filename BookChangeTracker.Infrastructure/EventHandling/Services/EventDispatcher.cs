using BookChangeTracker.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BookChangeTracker.Infrastructure.EventHandling.Services;

public class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent @event)
    {
        var eventType = @event.GetType();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
        
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetService(handlerType);
        if (handler is null) return;

        await (Task)handlerType
            .GetMethod(nameof(IEventHandler<IDomainEvent>.HandleAsync))!
            .Invoke(handler, [@event])!;
    }
}
