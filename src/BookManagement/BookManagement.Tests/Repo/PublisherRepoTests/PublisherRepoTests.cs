using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Database;
using BookManagement.Infrastructure.Repo;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Tests.Repo.PublisherRepoTests;

public class PublisherRepoTests : BaseEntityTests<Publisher>
{
    private readonly Random _rand = new();
    private static readonly Guid TestGuid = Guid.NewGuid();

    public PublisherRepoTests() : base(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(TestGuid.ToString()).Options,
        new PublisherRepo(new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(TestGuid.ToString()).Options)))
    { }

    protected override Publisher CreateEntity()
    {
        return new Publisher
        {
            Id = Guid.NewGuid(),
            Name = "Test Name",
            Address = "Test Addr",
            Phone = Enumerable.Range(0, 10).Select(_ => (char)_rand.Next(0, 9)).ToString()!,
            Email = "TestEmail@test.com"
        };
    }

    protected override void ModifyEntity(Publisher entity)
    {
        entity.Name = "Updated Publisher";
    }

    protected override void AssertEntityUpdated(Publisher originalEntity, Publisher updatedEntity)
    {
        Assert.Equal(originalEntity.Id, updatedEntity.Id);
        Assert.Equal("Updated Publisher", updatedEntity.Name);
        Assert.Equal(originalEntity.Address, updatedEntity.Address);
        Assert.Equal(originalEntity.Phone, updatedEntity.Phone);
        Assert.Equal(originalEntity.Email, updatedEntity.Email);
    }
}

