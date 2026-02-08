using BookChangeTracker.Domain.Models.Entities;

namespace BookChangeTracker.Infrastructure.Abstractions;

public interface IAuthorRepository : IRepository<Author>
{
    Task<List<Author>> GetAllAsync();
}
