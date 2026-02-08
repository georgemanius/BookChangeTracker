using BookChangeTracker.Domain.Abstractions;
using BookChangeTracker.Domain.Models.Entities;
using BookChangeTracker.Domain.Models.Events;
using BookChangeTracker.Infrastructure.Abstractions;

namespace BookChangeTracker.Infrastructure.EventHandling.Handlers;

public class AuthorRemovedEventHandler(IChangeLogRepository changeLogRepository) : IEventHandler<AuthorRemovedFromBookEvent>
{
    public async Task HandleAsync(AuthorRemovedFromBookEvent @event)
    {
        var changeLog = new BookChangeLog
        {
            BookId = @event.BookId,
            TargetAuthorId = @event.AuthorId,
            FieldName = "Authors",
            OldValue = @event.AuthorName,
            Description = $"Author '{@event.AuthorName}' removed from book",
            ChangedAt = @event.RaisedAt,
            ChangedBy = "System",
            ChangeType = @event.ChangeType
        };

        await changeLogRepository.CreateAsync(changeLog);
    }
}
