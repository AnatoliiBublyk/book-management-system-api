using BookManagement.Application.Commands.Handlers;
using BookManagement.Application.Commands;
using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using Moq;

namespace BookManagement.Tests.Commands;

public class DeleteBookHandlerTests
{
    private readonly Mock<IBookRepo> _bookRepoMock;
    private readonly DeleteBookHandler _handler;

    public DeleteBookHandlerTests()
    {
        _bookRepoMock = new Mock<IBookRepo>();
        _handler = new DeleteBookHandler(_bookRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenBookIsDeleted()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var book = new Book { Id = bookId };

        _bookRepoMock.Setup(repo => repo.GetByIdAsync(bookId)).ReturnsAsync(book);
        _bookRepoMock.Setup(repo => repo.DeleteByIdAsync(bookId)).Returns(Task.CompletedTask);

        var request = new DeleteBookCommand { Id = bookId };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _bookRepoMock.Verify(repo => repo.GetByIdAsync(bookId), Times.Once);
        _bookRepoMock.Verify(repo => repo.DeleteByIdAsync(bookId), Times.Once);
    }
}
