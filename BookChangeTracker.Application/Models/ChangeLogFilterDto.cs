namespace BookChangeTracker.Application.Models;

public record ChangeLogFilterDto(
    string? FieldName = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    string? ChangedBy = null,
    string OrderBy = "ChangedAt",
    string SortOrder = "Descending");
