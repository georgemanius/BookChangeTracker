using BookChangeTracker.Domain.Models.Events;
using BookChangeTracker.Infrastructure.EventHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace BookChangeTracker.Infrastructure;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent @event);
}

public class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent @event)
    {
        var eventType = @event.GetType();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
        
        using var scope = serviceProvider.CreateScope();
        if (scope.ServiceProvider.GetService(handlerType) is not { } handler) return;

        await (Task)handlerType
            .GetMethod(nameof(IEventHandler<IDomainEvent>.HandleAsync))!
            .Invoke(handler, [@event])!;
    }
}
