using BookManagement.Application.Queries.Handlers;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using Moq;
using MapsterMapper;
using BookManagement.Application.Queries;
using BookManagement.Domain.Entities;

namespace BookManagement.Tests.Queries;

public class GetPublisherByIdHandlerTests
{
    private readonly Mock<IPublisherRepo> _publisherRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPublisherByIdHandler _handler;

    public GetPublisherByIdHandlerTests()
    {
        _publisherRepoMock = new Mock<IPublisherRepo>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPublisherByIdHandler(_mapperMock.Object, _publisherRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPublisherDto_WhenPublisherExists()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisherName = "Test Publisher";
        var publisherAddress = "123 Test St";
        var publisherPhone = "123-456-7890";
        var publisherEmail = "test@publisher.com";

        var publisher = new Publisher
        {
            Id = publisherId,
            Name = publisherName,
            Address = publisherAddress,
            Phone = publisherPhone,
            Email = publisherEmail
        };

        var publisherDto = new PublisherDto
        {
            Id = publisherId,
            Name = publisherName,
            Address = publisherAddress,
            Phone = publisherPhone,
            Email = publisherEmail
        };

        var query = new GetPublisherByIdQuery { Id = publisherId };

        _publisherRepoMock.Setup(repo => repo.GetByIdAsync(publisherId)).ReturnsAsync(publisher);
        _mapperMock.Setup(mapper => mapper.Map<PublisherDto>(publisher)).Returns(publisherDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equivalent(publisherDto, result);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenPublisherDoesNotExist()
    {
        // Arrange
        var query = new GetPublisherByIdQuery { Id = Guid.NewGuid() };

        _publisherRepoMock.Setup(repo => repo.GetByIdAsync(query.Id)).ReturnsAsync((Publisher)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
