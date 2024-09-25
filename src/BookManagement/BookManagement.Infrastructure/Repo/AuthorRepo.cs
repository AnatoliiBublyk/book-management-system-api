using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repo;

public class AuthorRepo(AppDbContext context) : IAuthorRepo
{
    public Task<IQueryable<Author>> GetAllAsync()
    {
        return Task.FromResult(context.Set<Author>().AsNoTracking());
    }

    public async Task<Author> GetByIdAsync(Guid id)
    {
        var entity = await context.Set<Author>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        if (entity is null)
        {
            throw new KeyNotFoundException($"Entity of type <{nameof(Author)}> with ID {id} was not found.");
        }
        return entity;
    }

    public async Task<Author> UpdateAsync(Author entity)
    {
        context.Set<Author>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await context.Set<Author>().FindAsync(id);
        if (entity is null)
        {
            throw new KeyNotFoundException($"Entity of type <{nameof(Author)}> with ID {id} was not found.");
        }
        context.Set<Author>().Remove(entity);
        await context.SaveChangesAsync();
    }
}