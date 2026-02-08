using BookChangeTracker.Domain.Models.Entities;
using BookChangeTracker.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookChangeTracker.Infrastructure.Repositories;

public class BookRepository(ApplicationDbContext context) 
    : Repository<Book>(context), IBookRepository
{
    public async Task<Book?> GetByIdWithAuthorsAsync(int id) =>
        await Context.Books
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .SingleOrDefaultAsync(b => b.Id == id);

    public async Task<List<Book>> GetAllWithAuthorsAsync() =>
        await Context.Books
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .ToListAsync();
}
