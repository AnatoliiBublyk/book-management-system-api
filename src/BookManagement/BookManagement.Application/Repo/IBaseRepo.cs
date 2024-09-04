namespace BookManagement.Application.Repo;

public interface IBaseRepo<T>
where T : class
{
    public Task<IQueryable<T>> GetAllAsync();
    public Task<T> GetByIdAsync(Guid id);
    public Task<T> AddAsync(T entity);
    public Task<T> UpdateAsync(T entity);
    public Task DeleteByIdAsync(Guid id);
}