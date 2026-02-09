using BookChangeTracker.Domain.Abstractions;
using BookChangeTracker.Domain.Models.Entities;
using BookChangeTracker.Domain.Models.Events;
using BookChangeTracker.Infrastructure.Abstractions;

namespace BookChangeTracker.Application.EventHandlers;

public class BookPropertyChangedEventHandler(IChangeLogRepository changeLogRepository) : IEventHandler<BookPropertyChangedEvent>
{
    public async Task HandleAsync(BookPropertyChangedEvent @event)
    {
        var changeLog = new BookChangeLog
        {
            BookId = @event.BookId,
            FieldName = @event.FieldName,
            OldValue = @event.OldValue,
            NewValue = @event.NewValue,
            Description = $"{@event.FieldName} changed from '{@event.OldValue}' to '{@event.NewValue}'",
            ChangedAt = @event.RaisedAt,
            ChangedBy = "System",
            ChangeType = @event.ChangeType
        };

        await changeLogRepository.CreateAsync(changeLog);
    }
}
