using BookChangeTracker.Application.Models;

namespace BookChangeTracker.Application.Abstractions;

public interface IAuthorService
{
    Task<Result> GetByIdAsync(int id);
    Task<Result> GetAllAsync();
    Task<Result> CreateAsync(CreateAuthorDto createAuthorDto);
    Task<Result> DeleteAsync(int id);
}
