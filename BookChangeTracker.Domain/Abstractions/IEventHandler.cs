using BookChangeTracker.Domain.Models.Events;

namespace BookChangeTracker.Domain.Abstractions;

public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event);
}
