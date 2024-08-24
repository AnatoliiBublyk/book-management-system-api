using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repo;

public class PublisherRepo(AppDbContext context) : IPublisherRepo
{
    public Task<IQueryable<Publisher>> GetAllAsync()
    {
        return Task.FromResult(context.Publishers.AsNoTracking());
    }

    public async Task<Publisher> GetByIdAsync(Guid id)
    {
        var publisher = await context.Publishers.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        if (publisher is null)
        {
            throw new KeyNotFoundException($"Publisher with ID {id} was not found.");
        }
        return publisher;
    }

    public async Task<Publisher> AddAsync(Publisher entity)
    {
        await context.Publishers.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<Publisher> UpdateAsync(Publisher entity)
    {
        context.Publishers.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await context.Publishers.FindAsync(id);
        if (entity is null)
        {
            throw new KeyNotFoundException($"Publisher with ID {id} was not found.");
        }
        context.Publishers.Remove(entity);
        await context.SaveChangesAsync();
    }
}