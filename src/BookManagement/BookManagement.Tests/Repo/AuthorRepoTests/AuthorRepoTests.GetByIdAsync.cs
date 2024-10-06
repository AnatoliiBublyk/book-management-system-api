using BookManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Tests.Repo.AuthorRepoTests;

public class AuthorRepoTests_GetByIdAsync : AuthorRepoTestsBase
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectAuthor()
    {
        // Arrange
        var id = Guid.NewGuid();
        var author = new Author
        {
            Id = id,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            UserName = "john_doe",
            PasswordHash = "hashed_password"
        };
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenAuthorNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.GetByIdAsync(nonExistentId));
    }
}