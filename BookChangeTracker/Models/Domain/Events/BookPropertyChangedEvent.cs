namespace BookChangeTracker.Models.Domain.Events;

public class BookPropertyChangedEvent(
    int bookId,
    string fieldName,
    string? oldValue,
    string? newValue,
    DateTime raisedAt)
    : IDomainEvent
{
    public int BookId { get; } = bookId;
    public string FieldName { get; } = fieldName;
    public string? OldValue { get; } = oldValue;
    public string? NewValue { get; } = newValue;
    public DateTime RaisedAt { get; } = raisedAt;
}