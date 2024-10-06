using BookManagement.Infrastructure.Database;
using BookManagement.Infrastructure.Repo;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Tests.Repo.AuthorRepoTests;

public class AuthorRepoTestsBase : IDisposable
{
    protected readonly AppDbContext _context;
    protected readonly AuthorRepo _repo;

    public AuthorRepoTestsBase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repo = new AuthorRepo(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}