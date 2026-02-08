using BookChangeTracker.Application.Models;

namespace BookChangeTracker.Application.Abstractions;

public interface IChangeLogService
{
    Task<Result> GetBookChangeLogsAsync(
        int bookId, 
        ChangeLogFilterDto filter, 
        PaginationDto pagination);
}
