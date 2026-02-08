namespace BookChangeTracker.Domain.Abstractions;

public interface IFilterDto
{
    string? FieldName { get; }
    DateTime? FromDate { get; }
    DateTime? ToDate { get; }
    string? ChangedBy { get; }
    ChangeLogSortFields OrderBy { get; }
    SortOrder SortOrder { get; }
}
