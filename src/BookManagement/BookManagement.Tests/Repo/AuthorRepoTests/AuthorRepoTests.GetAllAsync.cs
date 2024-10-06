using BookManagement.Domain.Entities;

namespace BookManagement.Tests.Repo.AuthorRepoTests;

public class AuthorRepoTests_GetAllAsync : AuthorRepoTestsBase
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAuthors()
    {
        // Arrange
        _context.Authors.Add(new Author
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            UserName = "john_doe",
            PasswordHash = "hashed_password"
        });
        _context.Authors.Add(new Author
        {
            Id = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            UserName = "jane_doe",
            PasswordHash = "hashed_password"
        });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }
}
