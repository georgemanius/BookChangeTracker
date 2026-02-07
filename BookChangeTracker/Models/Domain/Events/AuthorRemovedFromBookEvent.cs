namespace BookChangeTracker.Models.Domain.Events;

public record AuthorRemovedFromBookEvent(
    int BookId,
    int AuthorId,
    string AuthorName,
    DateTime RaisedAt) : IBookChangeEvent
{
    public ChangeType ChangeType => ChangeType.AuthorRemoved;
}