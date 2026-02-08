using BookChangeTracker.Domain.Abstractions;
using BookChangeTracker.Domain.Models.Entities;
using BookChangeTracker.Domain.Models.Events;
using BookChangeTracker.Infrastructure.Abstractions;

namespace BookChangeTracker.Infrastructure.EventHandling.Handlers;

public class AuthorAddedEventHandler(IChangeLogRepository changeLogRepository) : IEventHandler<AuthorAddedToBookEvent>
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

        await changeLogRepository.CreateAsync(changeLog);
    }
}
