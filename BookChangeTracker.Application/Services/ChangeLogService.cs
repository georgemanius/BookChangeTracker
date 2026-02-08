using BookChangeTracker.Application.Abstractions;
using BookChangeTracker.Application.Extensions;
using BookChangeTracker.Application.Models;
using BookChangeTracker.Infrastructure.Abstractions;

namespace BookChangeTracker.Application.Services;


public class ChangeLogService(IChangeLogRepository changeLogRepository) : IChangeLogService
{
    public async Task<(List<BookChangeLogDto> Logs, int TotalCount)> GetBookChangeLogsAsync(
        int bookId,
        ChangeLogFilterDto filter,
        PaginationDto pagination)
    {
        var (logs, totalCount) = await changeLogRepository.GetByBookIdAsync(bookId, filter, pagination);
        return (logs.Select(l => l.ToBookChangeLogDto()).ToList(), totalCount);
    }
}
