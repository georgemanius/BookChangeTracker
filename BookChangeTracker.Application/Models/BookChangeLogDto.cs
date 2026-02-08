namespace BookChangeTracker.Application.Models;

public record BookChangeLogDto(
    int Id,
    int BookId,
    string FieldName,
    string? OldValue,
    string? NewValue,
    DateTime ChangedAt,
    string Description,
    string ChangedBy,
    string ChangeType,
    int? AuthorId);
