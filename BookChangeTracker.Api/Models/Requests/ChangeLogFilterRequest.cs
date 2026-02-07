using BookChangeTracker.Api.Models.Enums;

namespace BookChangeTracker.Api.Models.Requests;

public record ChangeLogFilterRequest(
    string? FieldName = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    string? ChangedBy = null,
    ChangeLogSortFields OrderBy = ChangeLogSortFields.ChangedAt,
    SortOrder SortOrder = SortOrder.Descending);
