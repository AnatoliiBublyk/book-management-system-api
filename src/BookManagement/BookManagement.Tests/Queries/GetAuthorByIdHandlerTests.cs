using BookManagement.Application.Queries;
using BookManagement.Application.Queries.Handlers;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Domain.Entities;
using MapsterMapper;
using Moq;

namespace BookManagement.Tests.Queries;
public class GetAuthorByIdHandlerTests
{
    private readonly Mock<IAuthorRepo> _authorRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAuthorByIdHandler _handler;

    public GetAuthorByIdHandlerTests()
    {
        _authorRepoMock = new Mock<IAuthorRepo>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAuthorByIdHandler(_mapperMock.Object, _authorRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthorDto_WhenAuthorExists()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var authorEmail = "author@example.com";
        var authorFirstName = "John";
        var authorLastName = "Doe";

        var author = new Author
        {
            Id = authorId,
            Email = authorEmail,
            UserName = authorEmail, // Assuming username is the same as email
            FirstName = authorFirstName,
            LastName = authorLastName
        };

        var authorDto = new AuthorDto
        {
            Id = authorId,
            Email = authorEmail,
            FirstName = authorFirstName,
            LastName = authorLastName
        };

        var query = new GetAuthorByIdQuery { Id = authorId };

        _authorRepoMock.Setup(repo => repo.GetByIdAsync(authorId)).ReturnsAsync(author);
        _mapperMock.Setup(mapper => mapper.Map<AuthorDto>(author)).Returns(authorDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(authorDto, result);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenAuthorDoesNotExist()
    {
        // Arrange
        var query = new GetAuthorByIdQuery { Id = Guid.NewGuid() };

        _authorRepoMock.Setup(repo => repo.GetByIdAsync(query.Id)).ReturnsAsync((Author)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
