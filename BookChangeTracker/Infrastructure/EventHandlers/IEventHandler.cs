using BookChangeTracker.Models.Domain.Events;

namespace BookChangeTracker.Infrastructure.EventHandlers;

public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event);
}
