using BookManagement.Application.Commands.Handlers;
using BookManagement.Application.Commands;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Domain.Entities;
using Moq;
using MapsterMapper;

namespace BookManagement.Tests.Commands;

public class AddPublisherHandlerTests
{
    private readonly Mock<IPublisherRepo> _publisherRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddPublisherHandler _handler;

    public AddPublisherHandlerTests()
    {
        _publisherRepoMock = new Mock<IPublisherRepo>();
        _mapperMock = new Mock<IMapper>();
        _handler = new AddPublisherHandler(_mapperMock.Object, _publisherRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAddPublisherResponse_WhenPublisherIsAdded()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisherName = "Test Publisher";
        var publisherAddress = "123 Test St";
        var publisherPhone = "1234567890";
        var publisherEmail = "test@example.com";

        var request = new AddPublisherCommand
        {
            Request = new AddPublisherRequest
            {
                Name = publisherName,
                Address = publisherAddress,
                Phone = publisherPhone,
                Email = publisherEmail
            }
        };

        var newPublisher = new Publisher
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

        _mapperMock.Setup(mapper => mapper.Map<Publisher>(request.Request)).Returns(newPublisher);
        _publisherRepoMock.Setup(repo => repo.AddAsync(newPublisher)).ReturnsAsync(newPublisher);
        _mapperMock.Setup(mapper => mapper.Map<PublisherDto>(newPublisher)).Returns(publisherDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(publisherDto, result.Publisher);
    }
}
