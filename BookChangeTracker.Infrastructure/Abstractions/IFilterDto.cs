using BookChangeTracker.Infrastructure.Models.Enums;

namespace BookChangeTracker.Infrastructure.Abstractions;

public interface IFilterDto
{
    string? FieldName { get; }
    DateTime? FromDate { get; }
    DateTime? ToDate { get; }
    string? ChangedBy { get; }
    ChangeLogSortFields OrderBy { get; }
    SortOrder SortOrder { get; }
    int PageNumber { get; }
    int PageSize { get; }
}

public interface IPaginationDto
{
    int PageNumber { get; }
    int PageSize { get; }
}
