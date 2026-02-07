using BookChangeTracker.Models.Domain;
using BookChangeTracker.Models.Domain.Events;

namespace BookChangeTracker.Infrastructure.EventHandlers;

public class BookPropertyChangedEventHandler(ApplicationDbContext context) : IEventHandler<BookPropertyChangedEvent>
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
            ChangeType = ChangeType.PropertyChanged
        };

        context.BookChangeLogs.Add(changeLog);
        await context.SaveChangesAsync();
    }
}
