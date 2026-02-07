using BookChangeTracker.Models.Domain;
using BookChangeTracker.Models.Domain.Events;

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
            ChangeType = ChangeType.AuthorAdded
        };

        context.BookChangeLogs.Add(changeLog);
        await context.SaveChangesAsync();
    }
}
