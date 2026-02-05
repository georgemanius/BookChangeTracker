using System.Linq.Expressions;
using BookChangeTracker.Models.Domain;
using BookChangeTracker.Models.Requests;
using BookChangeTracker.Models.Enums;

namespace BookChangeTracker.Infrastructure;

public static class PaginationExtensions
{
    public static IQueryable<T> ApplyPagination<T>(
        this IQueryable<T> query,
        PaginationRequest pagination)
    {
        return query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize);
    }
}

public static class BookChangeLogQueryExtensions
{
    private static readonly Dictionary<ChangeLogSortFields, Expression<Func<BookChangeLog, object>>> SortColumnMap = new()
    {
        [ChangeLogSortFields.ChangedAt] = log => log.ChangedAt,
        [ChangeLogSortFields.FieldName] = log => log.FieldName
    };

    public static IQueryable<BookChangeLog> ApplyFiltering(
        this IQueryable<BookChangeLog> query,
        int bookId,
        ChangeLogFilterRequest filter)
    {
        query = query.Where(cl => cl.BookId == bookId);

        if (!string.IsNullOrEmpty(filter.FieldName))
            query = query.Where(cl => cl.FieldName.Contains(filter.FieldName));

        if (filter.FromDate.HasValue)
            query = query.Where(cl => cl.ChangedAt >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            query = query.Where(cl => cl.ChangedAt <= filter.ToDate.Value);

        if (!string.IsNullOrEmpty(filter.ChangedBy))
            query = query.Where(cl => cl.ChangedBy.Contains(filter.ChangedBy));

        return query;
    }

    public static IQueryable<BookChangeLog> ApplySorting(
        this IQueryable<BookChangeLog> query,
        ChangeLogSortFields orderBy,
        SortOrder sortOrder)
    {
        var keySelector = SortColumnMap[orderBy];
        var isDescending = sortOrder is SortOrder.Descending;

        return isDescending
            ? query.OrderByDescending(keySelector)
            : query.OrderBy(keySelector);
    }
}