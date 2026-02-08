using BookChangeTracker.Domain.Models.Entities;
using BookChangeTracker.Infrastructure.Repositories;

namespace BookChangeTracker.Infrastructure.Abstractions;

public interface IChangeLogRepository
{
    Task<BookChangeLog> CreateAsync(BookChangeLog changeLog);

    Task<(List<BookChangeLog> Logs, int TotalCount)> GetByBookIdAsync(
        int bookId,
        IFilterDto filter,
        IPaginationDto pagination);
}
