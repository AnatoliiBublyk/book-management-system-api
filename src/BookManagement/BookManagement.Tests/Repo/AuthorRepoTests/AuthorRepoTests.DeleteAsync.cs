using BookManagement.Domain.Entities;

namespace BookManagement.Tests.Repo.AuthorRepoTests;

public class AuthorRepoTests_DeleteAsync : AuthorRepoTestsBase
{
    [Fact]
    public async Task DeleteByIdAsync_ShouldDeleteAuthor()
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
        await _repo.DeleteByIdAsync(id);
        var result = await _context.Authors.FindAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldThrowKeyNotFoundException_WhenAuthorNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.DeleteByIdAsync(nonExistentId));
    }
}