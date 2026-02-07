namespace BookChangeTracker.Domain.Abstractions;

public interface IChangeTrackingService
{
    Task RecordPropertyChangeAsync(int bookId, string fieldName, string? oldValue, string? newValue, string changedBy);
    Task RecordAuthorAddedAsync(int bookId, int authorId, string authorName, string changedBy);
    Task RecordAuthorRemovedAsync(int bookId, int authorId, string authorName, string changedBy);
}
