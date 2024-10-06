using BookManagement.Application.Queries;
using BookManagement.Application.Queries.Handlers;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Domain.Entities;
using MapsterMapper;
using Moq;

namespace BookManagement.Tests.Queries;

public class GetAllAuthorsHandlerTests
{
    private readonly Mock<IAuthorRepo> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllAuthorsHandler _handler;

    public GetAllAuthorsHandlerTests()
    {
        _repoMock = new Mock<IAuthorRepo>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetAllAuthorsHandler(_mapperMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthorDtos_WhenAuthorsExist()
    {
        // Arrange
        var authors = new List<Author>
        {
            new() { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
            new() { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
        }.AsQueryable();
        var authorDtos = new List<AuthorDto>
        {
            new() { Id = authors.ElementAt(0).Id, FirstName = "John", LastName = "Doe" },
            new() { Id = authors.ElementAt(1).Id, FirstName = "Jane", LastName = "Smith" }
        };
        var getAllAuthorsQuery = new GetAllAuthorsQuery();

        _repoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(authors);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<AuthorDto>>(authors)).Returns(authorDtos);

        // Act
        var result = await _handler.Handle(getAllAuthorsQuery, CancellationToken.None);

        // Assert
        Assert.Equivalent(result, authorDtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmpty_WhenNoAuthorsExist()
    {
        // Arrange
        var authors = new List<Author>().AsQueryable();
        var authorDtos = new List<AuthorDto>();
        var getAllAuthorsQuery = new GetAllAuthorsQuery();

        _repoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(authors.AsQueryable());
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<AuthorDto>>(authors)).Returns(authorDtos);

        // Act
        var result = await _handler.Handle(getAllAuthorsQuery, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
