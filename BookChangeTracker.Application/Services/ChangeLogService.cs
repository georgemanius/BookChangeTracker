using BookChangeTracker.Application.Abstractions;
using BookChangeTracker.Application.Extensions;
using BookChangeTracker.Application.Models;
using BookChangeTracker.Infrastructure.Abstractions;

namespace BookChangeTracker.Application.Services;

public class ChangeLogService(
    IChangeLogRepository changeLogRepository,
    IBookRepository bookRepository) : IChangeLogService
{
    public async Task<Result> GetBookChangeLogsAsync(
        int bookId,
        ChangeLogFilterDto filter,
        PaginationDto pagination)
    {
        var book = await bookRepository.GetByIdAsync(bookId);
        if (book is null)
            return Result.Failure(new Error(ErrorCodes.BookNotFound, $"Book with id {bookId} does not exist"));

        var (logs, totalCount) = await changeLogRepository.GetByBookIdAsync(bookId, filter, pagination);
        var result = (logs.Select(l => l.ToBookChangeLogDto()).ToList(), totalCount);
        return Result.Success(result);
    }
}
