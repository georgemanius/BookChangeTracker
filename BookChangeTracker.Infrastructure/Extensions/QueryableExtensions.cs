using BookChangeTracker.Domain.Models.Entities;
using BookChangeTracker.Infrastructure.Abstractions;
using BookChangeTracker.Infrastructure.Models.Enums;
using BookChangeTracker.Infrastructure.Repositories;

namespace BookChangeTracker.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPagination<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize)
        where T : class
    {
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}

public static class BookChangeLogRepositoryExtensions
{
    public static IQueryable<BookChangeLog> ApplyFiltering(
        this IQueryable<BookChangeLog> query,
        int bookId,
        IFilterDto filter)
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
        var isDescending = sortOrder is SortOrder.Descending;

        return orderBy switch
        {
            ChangeLogSortFields.FieldName => isDescending 
                ? query.OrderByDescending(cl => cl.FieldName)
                : query.OrderBy(cl => cl.FieldName),
            _ => isDescending 
                ? query.OrderByDescending(cl => cl.ChangedAt)
                : query.OrderBy(cl => cl.ChangedAt)
        };
    }
}
