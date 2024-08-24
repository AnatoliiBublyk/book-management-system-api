using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repo;

public class AuthorRepo(AppDbContext context) : IAuthorRepo
{
    public Task<IQueryable<Author>> GetAllAsync()
    {
        return Task.FromResult(context.Authors.AsNoTracking());
    }

    public async Task<Author> GetByIdAsync(Guid id)
    {
        var author = await context.Authors.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        if (author is null)
        {
            throw new KeyNotFoundException($"Author with ID {id} was not found.");
        }
        return author;
    }

    public async Task<Author> AddAsync(Author entity)
    {
        await context.Authors.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<Author> UpdateAsync(Author entity)
    {
        context.Authors.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await context.Authors.FindAsync(id);
        if (entity is null)
        {
            throw new KeyNotFoundException($"Author with ID {id} was not found.");
        }
        context.Authors.Remove(entity);
        await context.SaveChangesAsync();
    }
}