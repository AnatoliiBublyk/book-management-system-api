using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repo;

public class BookRepo(AppDbContext context) : IBookRepo
{
    public Task<IQueryable<Book>> GetAllAsync()
    {
        return Task.FromResult(context.Books.AsNoTracking());
    }

    public async Task<Book> GetByIdAsync(Guid id)
    {
        var book = await context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        if (book is null)
        {
            throw new KeyNotFoundException($"Author with ID {id} was not found.");
        }
        return book;
    }

    public async Task<Book> AddAsync(Book entity)
    {
        await context.Books.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<Book> UpdateAsync(Book entity)
    {
        context.Books.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await context.Books.FindAsync(id);
        if (entity is null)
        {
            throw new KeyNotFoundException($"Book with ID {id} was not found.");
        }
        context.Books.Remove(entity);
        await context.SaveChangesAsync();
    }
}