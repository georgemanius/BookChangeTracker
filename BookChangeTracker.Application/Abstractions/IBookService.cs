using BookChangeTracker.Application.Models;

namespace BookChangeTracker.Application.Abstractions;

public interface IBookService
{
    Task<Result> GetByIdAsync(int id);
    Task<List<BookDto>> GetAllAsync();
    Task<Result> CreateAsync(CreateBookDto createBookDto);
    Task<Result> UpdateAsync(int id, UpdateBookDto updateBookDto);
    Task<bool> DeleteAsync(int id);
    Task<Result> AddAuthorAsync(int bookId, int authorId);
    Task<Result> RemoveAuthorAsync(int bookId, int authorId);
}
