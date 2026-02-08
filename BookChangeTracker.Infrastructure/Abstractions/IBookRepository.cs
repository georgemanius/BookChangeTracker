using BookChangeTracker.Domain.Models.Entities;

namespace BookChangeTracker.Infrastructure.Abstractions;

public interface IBookRepository : IRepository<Book>
{
    Task<Book?> GetByIdWithAuthorsAsync(int id);
    Task<List<Book>> GetAllWithAuthorsAsync();
}
