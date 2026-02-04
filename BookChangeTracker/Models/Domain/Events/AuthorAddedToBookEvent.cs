namespace BookChangeTracker.Models.Domain.Events;

public class AuthorAddedToBookEvent(int bookId, int authorId, string authorName, DateTime raisedAt)
    : IDomainEvent
{
    public int BookId { get; } = bookId;
    public int AuthorId { get; } = authorId;
    public string AuthorName { get; } = authorName;
    public DateTime RaisedAt { get; } = raisedAt;
}