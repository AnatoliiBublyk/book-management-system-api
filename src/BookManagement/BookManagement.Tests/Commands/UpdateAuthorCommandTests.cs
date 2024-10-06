using BookManagement.Application.Commands.Handlers;
using BookManagement.Application.Commands;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Domain.Entities;
using MapsterMapper;
using Moq;

namespace BookManagement.Tests.Commands;

public class UpdateAuthorHandlerTests
{
    private readonly Mock<IAuthorRepo> _authorRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateAuthorHandler _handler;

    public UpdateAuthorHandlerTests()
    {
        _authorRepoMock = new Mock<IAuthorRepo>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateAuthorHandler(_mapperMock.Object, _authorRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUpdatedAuthor_WhenUpdateIsSuccessful()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var author = new Author { Id = authorId, FirstName = "OldFirstName", LastName = "OldLastName" };
        var updatedAuthor = new Author { Id = authorId, FirstName = "NewFirstName", LastName = "NewLastName" };
        var requestBody = new UpdateAuthorRequest { FirstName = "NewFirstName", LastName = "NewLastName" };
        var updatedAuthorDto = new AuthorDto { Id = authorId, FirstName = "NewFirstName", LastName = "NewLastName" };

        _authorRepoMock.Setup(repo => repo.GetByIdAsync(authorId)).ReturnsAsync(author);
        _authorRepoMock.Setup(repo => repo.UpdateAsync(author)).ReturnsAsync(updatedAuthor);
        _mapperMock.Setup(mapper => mapper.Map<AuthorDto>(updatedAuthor)).Returns(updatedAuthorDto);

        var request = new UpdateAuthorCommand { Id = authorId, Body = requestBody };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(updatedAuthorDto, result.Author);
        _authorRepoMock.Verify(repo => repo.GetByIdAsync(authorId), Times.Once);
        _authorRepoMock.Verify(repo => repo.UpdateAsync(author), Times.Once);
    }
}
