using BookChangeTracker.Application.Abstractions;
using BookChangeTracker.Application.Extensions;
using BookChangeTracker.Application.Models;
using BookChangeTracker.Infrastructure.Abstractions;

namespace BookChangeTracker.Application.Services;

public class AuthorService(IAuthorRepository authorRepository) : IAuthorService
{
    public async Task<Result> GetByIdAsync(int id)
    {
        var author = await authorRepository.GetByIdAsync(id);
        if (author is null)
            return Result.Failure(new Error(ErrorCodes.AuthorNotFound, $"Author with id {id} does not exist"));
        
        return Result.Success(author.ToAuthorDto());
    }

    public async Task<Result> GetAllAsync()
    {
        var authors = await authorRepository.GetAllAsync();
        return Result.Success(authors.Select(a => a.ToAuthorDto()).ToList());
    }

    public async Task<Result> CreateAsync(CreateAuthorDto createAuthorDto)
    {
        var author = createAuthorDto.ToAuthor();
        var created = await authorRepository.CreateAsync(author);
        return Result.Success(created.ToAuthorDto());
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var deleted = await authorRepository.DeleteAsync(id);
        if (!deleted)
            return Result.Failure(new Error(ErrorCodes.AuthorNotFound, $"Author with id {id} does not exist"));
        
        return Result.Success();
    }
}
