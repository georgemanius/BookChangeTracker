using BookChangeTracker.Application.Models;

namespace BookChangeTracker.Application.Abstractions;

public interface IAuthorService
{
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<List<AuthorDto>> GetAllAsync();
    Task<Result> CreateAsync(CreateAuthorDto createAuthorDto);
    Task<bool> DeleteAsync(int id);
}
