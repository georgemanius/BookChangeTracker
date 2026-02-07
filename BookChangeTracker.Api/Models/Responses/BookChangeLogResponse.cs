namespace BookChangeTracker.Api.Models.Responses;

public record BookChangeLogResponse(
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
