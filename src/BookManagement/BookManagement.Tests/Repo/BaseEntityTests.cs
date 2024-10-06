using BookManagement.Application.Repo;
using BookManagement.Domain.Entities.Abstractions;
using BookManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Tests.Repo;

public abstract class BaseEntityTests<T> : IDisposable
    where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly IBaseRepo<T> _repo;

    protected BaseEntityTests(DbContextOptions<AppDbContext> options, IBaseRepo<T> repo)
    {
        _context = new AppDbContext(options);
        _repo = repo;
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEntities()
    {
        // Arrange
        var entity = CreateEntity();
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(await result.ToListAsync());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity_WhenExists()
    {
        // Arrange
        var entity = CreateEntity();
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var savedEntity = await _repo.GetByIdAsync(entity.Id);

        // Assert
        Assert.NotNull(savedEntity);
        Assert.Equal(entity.Id, savedEntity.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_WhenEntityNotFound()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var entity = CreateEntity();

        // Act
        var addedEntity = await _repo.AddAsync(entity);

        // Assert
        var dbEntity = await _context.Set<T>().FindAsync(addedEntity.Id);
        Assert.NotNull(dbEntity);
        Assert.Equal(entity.Id, dbEntity.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entity = CreateEntity();
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();

        // Modify the entity
        ModifyEntity(entity);

        // Act
        var updatedEntity = await _repo.UpdateAsync(entity);

        // Assert
        var dbEntity = await _context.Set<T>().FindAsync(updatedEntity.Id);
        Assert.Equal(updatedEntity.Id, dbEntity.Id);
        AssertEntityUpdated(dbEntity, updatedEntity);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldDeleteEntity_WhenExists()
    {
        // Arrange
        var entity = CreateEntity();
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();

        // Act
        await _repo.DeleteByIdAsync(entity.Id);

        // Assert
        var dbEntity = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);
        Assert.Null(dbEntity);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldThrowException_WhenEntityNotFound()
    {
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.DeleteByIdAsync(Guid.NewGuid()));
    }


    // Method to create a new instance of the entity (needs to be implemented in derived tests)
    protected abstract T CreateEntity();

    // Method to modify an entity (needs to be implemented in derived tests)
    protected abstract void ModifyEntity(T entity);

    // Method to assert entity was updated (optional but useful for UpdateAsync)
    protected abstract void AssertEntityUpdated(T originalEntity, T updatedEntity);

    public void Dispose()
    {
        _context.Set<T>().RemoveRange(_context.Set<T>());
        _context.SaveChanges();

    }
}
