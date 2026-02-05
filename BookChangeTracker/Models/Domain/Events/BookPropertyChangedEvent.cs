namespace BookChangeTracker.Models.Domain.Events;

public record BookPropertyChangedEvent(
    int BookId,
    string FieldName,
    string? OldValue,
    string? NewValue,
    DateTime RaisedAt) : IDomainEvent;