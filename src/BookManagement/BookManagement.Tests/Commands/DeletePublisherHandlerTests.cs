using BookManagement.Application.Commands.Handlers;
using BookManagement.Application.Commands;
using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using Moq;

namespace BookManagement.Tests.Commands;

public class DeletePublisherHandlerTests
{
    private readonly Mock<IPublisherRepo> _publisherRepoMock;
    private readonly DeletePublisherHandler _handler;

    public DeletePublisherHandlerTests()
    {
        _publisherRepoMock = new Mock<IPublisherRepo>();
        _handler = new DeletePublisherHandler(_publisherRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenPublisherIsDeleted()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisher = new Publisher { Id = publisherId };

        _publisherRepoMock.Setup(repo => repo.GetByIdAsync(publisherId)).ReturnsAsync(publisher);
        _publisherRepoMock.Setup(repo => repo.DeleteByIdAsync(publisherId)).Returns(Task.CompletedTask);

        var request = new DeletePublisherCommand { Id = publisherId };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _publisherRepoMock.Verify(repo => repo.GetByIdAsync(publisherId), Times.Once);
        _publisherRepoMock.Verify(repo => repo.DeleteByIdAsync(publisherId), Times.Once);
    }
}
