using BookChangeTracker.Domain.Models.Entities;
using BookChangeTracker.Domain.Models.Events;

namespace BookChangeTracker.Infrastructure.EventHandlers;

public class AuthorRemovedEventHandler(ApplicationDbContext context) : IEventHandler<AuthorRemovedFromBookEvent>
{
    public async Task HandleAsync(AuthorRemovedFromBookEvent @event)
    {
        var changeLog = new BookChangeLog
        {
            BookId = @event.BookId,
            AuthorId = @event.AuthorId,
            FieldName = "Authors",
            OldValue = @event.AuthorName,
            Description = $"Author '{@event.AuthorName}' removed from book",
            ChangedAt = @event.RaisedAt,
            ChangedBy = "System",
            ChangeType = @event.ChangeType
        };

        context.BookChangeLogs.Add(changeLog);
        await context.SaveChangesAsync();
    }
}
