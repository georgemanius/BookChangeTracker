using BookChangeTracker.Domain.Models.Entities;
using BookChangeTracker.Domain.Models.Events;

namespace BookChangeTracker.Infrastructure.EventHandlers;

public class AuthorAddedEventHandler(ApplicationDbContext context) : IEventHandler<AuthorAddedToBookEvent>
{
    public async Task HandleAsync(AuthorAddedToBookEvent @event)
    {
        var changeLog = new BookChangeLog
        {
            BookId = @event.BookId,
            AuthorId = @event.AuthorId,
            FieldName = "Authors",
            NewValue = @event.AuthorName,
            Description = $"Author '{@event.AuthorName}' added to book",
            ChangedAt = @event.RaisedAt,
            ChangedBy = "System",
            ChangeType = @event.ChangeType
        };

        context.BookChangeLogs.Add(changeLog);
        await context.SaveChangesAsync();
    }
}
