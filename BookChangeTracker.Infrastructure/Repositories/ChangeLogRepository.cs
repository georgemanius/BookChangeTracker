using BookChangeTracker.Domain.Models.Entities;
using BookChangeTracker.Infrastructure.Abstractions;
using BookChangeTracker.Infrastructure.Extensions;
using BookChangeTracker.Infrastructure.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookChangeTracker.Infrastructure.Repositories;

public class ChangeLogRepository(ApplicationDbContext context) : IChangeLogRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<BookChangeLog> CreateAsync(BookChangeLog changeLog)
    {
        _context.BookChangeLogs.Add(changeLog);
        await _context.SaveChangesAsync();
        return changeLog;
    }

    public async Task<(List<BookChangeLog> Logs, int TotalCount)> GetByBookIdAsync(
        int bookId,
        IFilterDto filter,
        IPaginationDto pagination)
    {
        var query = _context.BookChangeLogs.AsQueryable();

        query = query
            .ApplyFiltering(bookId, filter)
            .ApplySorting(filter.OrderBy, filter.SortOrder);

        var totalCount = await query.CountAsync();

        var logs = await query
            .ApplyPagination(pagination.PageNumber, pagination.PageSize)
            .ToListAsync();

        return (logs, totalCount);
    }
}
