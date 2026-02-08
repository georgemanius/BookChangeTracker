using BookChangeTracker.Application.Models;

namespace BookChangeTracker.Application.Abstractions;

public interface IBookService
{
    Task<BookDto?> GetByIdAsync(int id);
    Task<List<BookDto>> GetAllAsync();
    Task<BookDto> CreateAsync(CreateBookDto createBookDto);
    Task<BookDto?> UpdateAsync(int id, UpdateBookDto updateBookDto);
    Task<bool> DeleteAsync(int id);
    Task<BookDto?> AddAuthorAsync(int bookId, int authorId);
    Task<BookDto?> RemoveAuthorAsync(int bookId, int authorId);
}
