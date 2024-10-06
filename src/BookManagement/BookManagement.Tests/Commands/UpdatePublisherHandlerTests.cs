using BookManagement.Application.Commands.Handlers;
using BookManagement.Application.Commands;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Domain.Entities;
using MapsterMapper;
using Moq;

namespace BookManagement.Tests.Commands;

public class UpdatePublisherHandlerTests
{
    private readonly Mock<IPublisherRepo> _publisherRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdatePublisherHandler _handler;

    public UpdatePublisherHandlerTests()
    {
        _publisherRepoMock = new Mock<IPublisherRepo>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdatePublisherHandler(_mapperMock.Object, _publisherRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUpdatedPublisher_WhenUpdateIsSuccessful()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisher = new Publisher { Id = publisherId, Name = "OldName", Address = "OldAddress" };
        var updatedPublisher = new Publisher { Id = publisherId, Name = "NewName", Address = "NewAddress" };
        var requestBody = new UpdatePublisherRequest { Name = "NewName", Address = "NewAddress" };
        var updatedPublisherDto = new PublisherDto { Id = publisherId, Name = "NewName", Address = "NewAddress" };

        _publisherRepoMock.Setup(repo => repo.GetByIdAsync(publisherId)).ReturnsAsync(publisher);
        _publisherRepoMock.Setup(repo => repo.UpdateAsync(publisher)).ReturnsAsync(updatedPublisher);
        _mapperMock.Setup(mapper => mapper.Map<PublisherDto>(updatedPublisher)).Returns(updatedPublisherDto);

        var request = new UpdatePublisherCommand { Id = publisherId, Body = requestBody };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(updatedPublisherDto, result.Publisher);
        _publisherRepoMock.Verify(repo => repo.GetByIdAsync(publisherId), Times.Once);
        _publisherRepoMock.Verify(repo => repo.UpdateAsync(publisher), Times.Once);
    }
}
