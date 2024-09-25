using BookManagement.Domain.Entities;

namespace BookManagement.Application.Repo;

public interface IAuthorRepo
{
    public Task<IQueryable<Author>> GetAllAsync();
    public Task<Author> GetByIdAsync(Guid id);
    public Task<Author> UpdateAsync(Author entity);
    public Task DeleteByIdAsync(Guid id);
}