using BookChangeTracker.Application.Abstractions;
using BookChangeTracker.Application.Extensions;
using BookChangeTracker.Application.Models;
using BookChangeTracker.Infrastructure.Abstractions;

namespace BookChangeTracker.Application.Services;

public class BookService(IBookRepository bookRepository, IAuthorRepository authorRepository) : IBookService
{
    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await bookRepository.GetByIdWithAuthorsAsync(id);
        return book?.ToBookDto();
    }

    public async Task<List<BookDto>> GetAllAsync()
    {
        var books = await bookRepository.GetAllWithAuthorsAsync();
        return books.Select(b => b.ToBookDto()).ToList();
    }

    public async Task<BookDto> CreateAsync(CreateBookDto createBookDto)
    {
        var book = createBookDto.ToBook();
        var created = await bookRepository.CreateAsync(book);
        return created.ToBookDto();
    }

    public async Task<BookDto?> UpdateAsync(int id, UpdateBookDto updateBookDto)
    {
        var book = await bookRepository.GetByIdAsync(id);
        if (book is null)
            return null;

        updateBookDto.ApplyToBook(book);
        var updated = await bookRepository.UpdateAsync(book);
        return updated.ToBookDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await bookRepository.DeleteAsync(id);
    }

    public async Task<BookDto?> AddAuthorAsync(int bookId, int authorId)
    {
        var book = await bookRepository.GetByIdWithAuthorsAsync(bookId);
        if (book is null)
            return null;

        var author = await authorRepository.GetByIdAsync(authorId);
        if (author is null)
            return null;

        if (book.BookAuthors.Any(ba => ba.AuthorId == authorId))
            return null;

        book.AddAuthor(author);
        var updated = await bookRepository.UpdateAsync(book);
        return updated.ToBookDto();
    }

    public async Task<BookDto?> RemoveAuthorAsync(int bookId, int authorId)
    {
        var book = await bookRepository.GetByIdWithAuthorsAsync(bookId);
        if (book is null)
            return null;

        var bookAuthor = book.BookAuthors.FirstOrDefault(ba => ba.AuthorId == authorId);
        if (bookAuthor is null)
            return null;

        var authorName = bookAuthor.Author.Name;
        book.RemoveAuthor(authorId, authorName);
        
        var updated = await bookRepository.UpdateAsync(book);
        return updated.ToBookDto();
    }
}
