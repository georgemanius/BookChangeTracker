using BookChangeTracker.Domain.Abstractions;

namespace BookChangeTracker.Domain.Models.Events;

public record BookPropertyChangedEvent(
    int BookId,
    string FieldName,
    string? OldValue,
    string? NewValue,
    DateTime RaisedAt) : IBookChangeEvent
{
    public ChangeType ChangeType => ChangeType.PropertyChanged;
}
