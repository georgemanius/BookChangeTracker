using BookChangeTracker.Application.Abstractions;
using BookChangeTracker.Application.Extensions;
using BookChangeTracker.Application.Models;
using BookChangeTracker.Infrastructure.Abstractions;

namespace BookChangeTracker.Application.Services;

public class AuthorService(IAuthorRepository authorRepository) : IAuthorService
{
    public async Task<AuthorDto?> GetByIdAsync(int id)
    {
        var author = await authorRepository.GetByIdAsync(id);
        return author?.ToAuthorDto();
    }

    public async Task<List<AuthorDto>> GetAllAsync()
    {
        var authors = await authorRepository.GetAllAsync();
        return authors.Select(a => a.ToAuthorDto()).ToList();
    }

    public async Task<Result> CreateAsync(CreateAuthorDto createAuthorDto)
    {
        var author = createAuthorDto.ToAuthor();
        var created = await authorRepository.CreateAsync(author);
        return Result.Success(created.ToAuthorDto());
    }

    public async Task<bool> DeleteAsync(int id) => await authorRepository.DeleteAsync(id);
}
