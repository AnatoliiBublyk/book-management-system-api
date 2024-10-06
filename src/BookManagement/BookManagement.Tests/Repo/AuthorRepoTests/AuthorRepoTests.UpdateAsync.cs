using BookManagement.Domain.Entities;

namespace BookManagement.Tests.Repo.AuthorRepoTests;

public class AuthorRepoTests_UpdateAsync : AuthorRepoTestsBase
{
    [Fact]
    public async Task UpdateAsync_ShouldUpdateAuthor()
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
        author.FirstName = "Updated";
        var updatedAuthor = await _repo.UpdateAsync(author);

        // Assert
        Assert.Equal("Updated", updatedAuthor.FirstName);
    }
}