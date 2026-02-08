using BookChangeTracker.Domain.Abstractions;

namespace BookChangeTracker.Application.Models;

public record ChangeLogFilterDto(
    string? FieldName = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    string? ChangedBy = null,
    ChangeLogSortFields OrderBy = ChangeLogSortFields.ChangedAt,
    SortOrder SortOrder = SortOrder.Descending) : IFilterDto;
