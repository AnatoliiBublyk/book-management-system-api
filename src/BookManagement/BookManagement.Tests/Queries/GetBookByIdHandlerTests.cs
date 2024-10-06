using BookManagement.Application.Queries.Handlers;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using Moq;
using MapsterMapper;
using BookManagement.Application.Queries;
using BookManagement.Domain.Entities;

namespace BookManagement.Tests.Queries;

public class GetBookByIdHandlerTests
{
    private readonly Mock<IBookRepo> _bookRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetBookByIdHandler _handler;

    public GetBookByIdHandlerTests()
    {
        _bookRepoMock = new Mock<IBookRepo>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetBookByIdHandler(_mapperMock.Object, _bookRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnBookDto_WhenBookExists()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var bookTitle = "Test Book";
        var bookIsbn = "1234567890123";
        var bookDescription = "A description of the test book.";
        var publicationDate = DateTime.Now;

        var book = new Book
        {
            Id = bookId,
            Title = bookTitle,
            Isbn = bookIsbn,
            Description = bookDescription,
            PublicationDate = publicationDate
        };

        var bookDto = new BookDto
        {
            Id = bookId,
            Title = bookTitle,
            Isbn = bookIsbn,
            Description = bookDescription,
            PublicationDate = publicationDate
        };

        var query = new GetBookByIdQuery { Id = bookId };

        _bookRepoMock.Setup(repo => repo.GetByIdAsync(bookId)).ReturnsAsync(book);
        _mapperMock.Setup(mapper => mapper.Map<BookDto>(book)).Returns(bookDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equivalent(bookDto, result);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenBookDoesNotExist()
    {
        // Arrange
        var query = new GetBookByIdQuery { Id = Guid.NewGuid() };

        _bookRepoMock.Setup(repo => repo.GetByIdAsync(query.Id)).ReturnsAsync((Book)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
