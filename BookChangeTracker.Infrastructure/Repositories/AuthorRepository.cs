using BookChangeTracker.Domain.Models.Entities;
using BookChangeTracker.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookChangeTracker.Infrastructure.Repositories;

public class AuthorRepository(ApplicationDbContext context) 
    : Repository<Author>(context), IAuthorRepository
{
    public async Task<List<Author>> GetAllAsync() => await Context.Authors.ToListAsync();
}
