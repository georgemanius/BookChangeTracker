namespace BookChangeTracker.Models.Domain.Events;

public record AuthorAddedToBookEvent(
    int BookId,
    int AuthorId,
    string AuthorName,
    DateTime RaisedAt) : IDomainEvent;