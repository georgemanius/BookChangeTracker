using BookChangeTracker.Application.Abstractions;
using BookChangeTracker.Application.Extensions;
using BookChangeTracker.Application.Models;
using BookChangeTracker.Infrastructure.Abstractions;

namespace BookChangeTracker.Application.Services;

public class BookService(IBookRepository bookRepository, IAuthorRepository authorRepository) : IBookService
{
    public async Task<Result> GetByIdAsync(int id)
    {
        var book = await bookRepository.GetByIdWithAuthorsAsync(id);
        if (book is null)
            return Result.Failure(new Error(ErrorCodes.BookNotFound, $"Book with id {id} does not exist"));
        
        return Result.Success(book.ToBookDto());
    }

    public async Task<Result> GetAllAsync()
    {
        var books = await bookRepository.GetAllWithAuthorsAsync();
        return Result.Success(books.Select(b => b.ToBookDto()).ToList());
    }

    public async Task<Result> CreateAsync(CreateBookDto createBookDto)
    {
        var book = createBookDto.ToBook();
        var created = await bookRepository.CreateAsync(book);
        return Result.Success(created.ToBookDto());
    }

    public async Task<Result> UpdateAsync(int id, UpdateBookDto updateBookDto)
    {
        var book = await bookRepository.GetByIdAsync(id);
        if (book is null)
            return Result.Failure(new Error(ErrorCodes.BookNotFound, $"Book with id {id} does not exist"));

        updateBookDto.ApplyToBook(book);
        var updated = await bookRepository.UpdateAsync(book);
        return Result.Success(updated.ToBookDto());
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var deleted = await bookRepository.DeleteAsync(id);
        if (!deleted)
            return Result.Failure(new Error(ErrorCodes.BookNotFound, $"Book with id {id} does not exist"));
        
        return Result.Success();
    }

    public async Task<Result> AddAuthorAsync(int bookId, int authorId)
    {
        var book = await bookRepository.GetByIdWithAuthorsAsync(bookId);
        if (book is null)
            return Result.Failure(new Error(ErrorCodes.BookNotFound, $"Book with id {bookId} does not exist"));

        var author = await authorRepository.GetByIdAsync(authorId);
        if (author is null)
            return Result.Failure(new Error(ErrorCodes.AuthorNotFound, $"Author with id {authorId} does not exist"));

        if (book.BookAuthors.Any(ba => ba.AuthorId == authorId))
            return Result.Failure(new Error(ErrorCodes.AuthorAlreadyAssigned, $"Author with id {authorId} is already assigned to this book"));

        book.AddAuthor(author);
        var updated = await bookRepository.UpdateAsync(book);
        return Result.Success(updated.ToBookDto());
    }

    public async Task<Result> RemoveAuthorAsync(int bookId, int authorId)
    {
        var book = await bookRepository.GetByIdWithAuthorsAsync(bookId);
        if (book is null)
            return Result.Failure(new Error(ErrorCodes.BookNotFound, $"Book with id {bookId} does not exist"));

        var bookAuthor = book.BookAuthors.FirstOrDefault(ba => ba.AuthorId == authorId);
        if (bookAuthor is null)
            return Result.Failure(new Error(ErrorCodes.AuthorNotAssigned, $"Author with id {authorId} is not assigned to this book"));

        var authorName = bookAuthor.Author.Name;
        book.RemoveAuthor(authorId, authorName);
        
        var updated = await bookRepository.UpdateAsync(book);
        return Result.Success(updated.ToBookDto());
    }
}
