namespace BookChangeTracker.Models.Domain.Events;

public record AuthorRemovedFromBookEvent(
    int BookId,
    int AuthorId,
    string AuthorName,
    DateTime RaisedAt) : IDomainEvent;