using BookChangeTracker.Application.Models;

namespace BookChangeTracker.Application.Abstractions;

public interface IChangeLogService
{
    Task<(List<BookChangeLogDto> Logs, int TotalCount)> GetBookChangeLogsAsync(
        int bookId, 
        ChangeLogFilterDto filter, 
        PaginationDto pagination);
}
