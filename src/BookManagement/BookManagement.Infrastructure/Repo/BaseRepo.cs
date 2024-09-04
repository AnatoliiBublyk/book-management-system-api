using BookManagement.Application.Repo;
using BookManagement.Domain.Entities.Abstractions;
using BookManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repo;

public class BaseRepo<T>(AppDbContext context) : IBaseRepo<T>
where T : BaseEntity
{
    public Task<IQueryable<T>> GetAllAsync()
    {
        return Task.FromResult(context.Set<T>().AsNoTracking());
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        var entity = await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        if (entity is null)
        {
            throw new KeyNotFoundException($"Entity of type <{typeof(T).Name}> with ID {id} was not found.");
        }
        return entity;
    }

    public async Task<T> AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await context.Set<T>().FindAsync(id);
        if (entity is null)
        {
            throw new KeyNotFoundException($"Entity of type <{typeof(T).Name}> with ID {id} was not found.");
        }
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }
}