using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Database;
using BookManagement.Infrastructure.Repo;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Tests.Repo.BookRepoTests;

public class BookRepoTests : BaseEntityTests<Book>
{
    private readonly Random _rand = new();
    private static readonly Guid TestGuid = Guid.NewGuid();

    public BookRepoTests() : base(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(TestGuid.ToString()).Options,
        new BookRepo(new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(TestGuid.ToString()).Options)))
    { }

    protected override Book CreateEntity()
    {
        return new Book
        {
            Id = Guid.NewGuid(),
            Title = "Test Book",
            Description = "Test Description",
            Isbn = Enumerable.Range(0, 13).Select(_ => (char)_rand.Next(0, 9)).ToString()!,
            PublicationDate = DateTime.Now,
            AuthorId = Guid.NewGuid(),
            PublisherId = Guid.NewGuid()
        };
    }

    protected override void ModifyEntity(Book entity)
    {
        entity.Title = "Updated Book";
    }

    protected override void AssertEntityUpdated(Book originalEntity, Book updatedEntity)
    {
        Assert.Equal(originalEntity.Id, updatedEntity.Id);
        Assert.Equal("Updated Book", updatedEntity.Title);
        Assert.Equal(originalEntity.Isbn, updatedEntity.Isbn);
        Assert.Equal(originalEntity.PublicationDate, updatedEntity.PublicationDate);
        Assert.Equal(originalEntity.AuthorId, updatedEntity.AuthorId);
        Assert.Equal(originalEntity.PublisherId, updatedEntity.PublisherId);
    }
}

