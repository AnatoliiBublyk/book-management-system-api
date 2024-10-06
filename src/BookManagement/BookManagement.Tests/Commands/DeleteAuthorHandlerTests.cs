
using BookManagement.Application.Commands.Handlers;
using BookManagement.Application.Commands;
using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using Moq;

namespace BookManagement.Tests.Commands;

public class DeleteAuthorHandlerTests
{
    private readonly Mock<IAuthorRepo> _authorRepoMock;
    private readonly DeleteAuthorHandler _handler;

    public DeleteAuthorHandlerTests()
    {
        _authorRepoMock = new Mock<IAuthorRepo>();
        _handler = new DeleteAuthorHandler(_authorRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenAuthorIsDeleted()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var author = new Author { Id = authorId };

        _authorRepoMock.Setup(repo => repo.GetByIdAsync(authorId)).ReturnsAsync(author);
        _authorRepoMock.Setup(repo => repo.DeleteByIdAsync(authorId)).Returns(Task.CompletedTask);

        var request = new DeleteAuthorCommand { Id = authorId };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _authorRepoMock.Verify(repo => repo.GetByIdAsync(authorId), Times.Once);
        _authorRepoMock.Verify(repo => repo.DeleteByIdAsync(authorId), Times.Once);
    }
}
