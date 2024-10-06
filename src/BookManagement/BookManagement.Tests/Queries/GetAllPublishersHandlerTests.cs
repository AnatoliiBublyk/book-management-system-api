using BookManagement.Application.Queries;
using BookManagement.Application.Queries.Handlers;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Domain.Entities;
using MapsterMapper;
using Moq;

namespace BookManagement.Tests.Queries;
public class GetAllPublishersHandlerTests
{
    private readonly Mock<IPublisherRepo> _publisherRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllPublishersHandler _handler;

    public GetAllPublishersHandlerTests()
    {
        _publisherRepoMock = new Mock<IPublisherRepo>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllPublishersHandler(_mapperMock.Object, _publisherRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPublisherDtos_WhenPublishersExist()
    {
        // Arrange
        var publisher1Id = Guid.NewGuid();
        var publisher2Id = Guid.NewGuid();
        var publisher1Name = "Publisher 1";
        var publisher2Name = "Publisher 2";
        var publisher1Address = "123 Main St";
        var publisher2Address = "456 Elm St";
        var publisher1Phone = "123-456-7890";
        var publisher2Phone = "987-654-3210";
        var publisher1Email = "publisher1@example.com";
        var publisher2Email = "publisher2@example.com";

        var publishers = new List<Publisher>
        {
            new Publisher
            {
                Id = publisher1Id,
                Name = publisher1Name,
                Address = publisher1Address,
                Phone = publisher1Phone,
                Email = publisher1Email
            },
            new Publisher
            {
                Id = publisher2Id,
                Name = publisher2Name,
                Address = publisher2Address,
                Phone = publisher2Phone,
                Email = publisher2Email
            }
        }.AsQueryable();

        var publisherDtos = new List<PublisherDto>
        {
            new PublisherDto
            {
                Id = publisher1Id,
                Name = publisher1Name,
                Address = publisher1Address,
                Phone = publisher1Phone,
                Email = publisher1Email
            },
            new PublisherDto
            {
                Id = publisher2Id,
                Name = publisher2Name,
                Address = publisher2Address,
                Phone = publisher2Phone,
                Email = publisher2Email
            }
        };

        var query = new GetAllPublishersQuery();

        _publisherRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(publishers);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<PublisherDto>>(publishers)).Returns(publisherDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equivalent(publisherDtos, result);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmpty_WhenNoPublishersExist()
    {
        // Arrange
        var publishers = new List<Publisher>().AsQueryable();
        var publisherDtos = new List<PublisherDto>();

        var query = new GetAllPublishersQuery();

        _publisherRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(publishers);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<PublisherDto>>(publishers)).Returns(publisherDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
