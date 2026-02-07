namespace BookChangeTracker.Models.Domain.Events;

public record AuthorAddedToBookEvent(
    int BookId,
    int AuthorId,
    string AuthorName,
    DateTime RaisedAt) : IBookChangeEvent
{
    public ChangeType ChangeType => ChangeType.AuthorAdded;
}