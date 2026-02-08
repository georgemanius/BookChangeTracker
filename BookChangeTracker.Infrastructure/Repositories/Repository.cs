using BookChangeTracker.Infrastructure.Abstractions;

namespace BookChangeTracker.Infrastructure.Repositories;

public abstract class Repository<TEntity>(ApplicationDbContext context) 
    : IRepository<TEntity> 
    where TEntity : class
{
    protected readonly ApplicationDbContext Context = context;

    public virtual async Task<TEntity?> GetByIdAsync(int id) => await Context.Set<TEntity>().FindAsync(id);

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await Context.Set<TEntity>().FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync();
        return true;
    }
}
