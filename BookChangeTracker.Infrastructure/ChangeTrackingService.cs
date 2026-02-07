using BookChangeTracker.Domain.Abstractions;
using BookChangeTracker.Domain.Models;
using BookChangeTracker.Domain.Models.Entities;

namespace BookChangeTracker.Infrastructure;

public class ChangeTrackingService(ApplicationDbContext context) : IChangeTrackingService
{
    public async Task RecordPropertyChangeAsync(int bookId, string fieldName, string? oldValue, string? newValue, string changedBy)
    {
        var changeLog = new BookChangeLog
        {
            BookId = bookId,
            FieldName = fieldName,
            OldValue = oldValue,
            NewValue = newValue,
            Description = $"{fieldName} changed to '{newValue}'",
            ChangedAt = DateTime.UtcNow,
            ChangedBy = changedBy,
            ChangeType = ChangeType.PropertyChanged
        };

        context.BookChangeLogs.Add(changeLog);
        await context.SaveChangesAsync();
    }

    public async Task RecordAuthorAddedAsync(int bookId, int authorId, string authorName, string changedBy)
    {
        var changeLog = new BookChangeLog
        {
            BookId = bookId,
            AuthorId = authorId,
            FieldName = "Authors",
            NewValue = authorName,
            Description = $"Author '{authorName}' added to book",
            ChangedAt = DateTime.UtcNow,
            ChangedBy = changedBy,
            ChangeType = ChangeType.AuthorAdded
        };

        context.BookChangeLogs.Add(changeLog);
        await context.SaveChangesAsync();
    }

    public async Task RecordAuthorRemovedAsync(int bookId, int authorId, string authorName, string changedBy)
    {
        var changeLog = new BookChangeLog
        {
            BookId = bookId,
            AuthorId = authorId,
            FieldName = "Authors",
            OldValue = authorName,
            Description = $"Author '{authorName}' removed from book",
            ChangedAt = DateTime.UtcNow,
            ChangedBy = changedBy,
            ChangeType = ChangeType.AuthorRemoved
        };

        context.BookChangeLogs.Add(changeLog);
        await context.SaveChangesAsync();
    }
}
