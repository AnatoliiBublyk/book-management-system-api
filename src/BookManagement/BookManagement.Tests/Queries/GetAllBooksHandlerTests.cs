using BookManagement.Application.Queries.Handlers;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Application.Queries;
using BookManagement.Domain.Entities;
using MapsterMapper;
using Moq;

namespace BookManagement.Tests.Queries;

public class GetAllBooksHandlerTests
{
    private readonly Mock<IBookRepo> _bookRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllBooksHandler _handler;

    public GetAllBooksHandlerTests()
    {
        _bookRepoMock = new Mock<IBookRepo>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllBooksHandler(_mapperMock.Object, _bookRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnBookDtos_WhenBooksExist()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "Book 1", Isbn = GenerateRandomIsbn(), AuthorId = Guid.NewGuid(), PublisherId = Guid.NewGuid() },
            new Book { Id = Guid.NewGuid(), Title = "Book 2", Isbn = GenerateRandomIsbn(), AuthorId = Guid.NewGuid(), PublisherId = Guid.NewGuid() }
        }.AsQueryable();

        var bookDtos = new List<BookDto>
        {
            new BookDto { Id = books.ElementAt(0).Id, Title = "Book 1", Isbn = books.ElementAt(0).Isbn },
            new BookDto { Id = books.ElementAt(1).Id, Title = "Book 2", Isbn = books.ElementAt(1).Isbn }
        };

        var query = new GetAllBooksQuery();

        _bookRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<BookDto>>(books)).Returns(bookDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equivalent(bookDtos, result);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmpty_WhenNoBooksExist()
    {
        // Arrange
        var books = new List<Book>().AsQueryable();
        var bookDtos = new List<BookDto>();

        var query = new GetAllBooksQuery();

        _bookRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<BookDto>>(books)).Returns(bookDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    // Helper function to generate a random 13-character ISBN string
    private string GenerateRandomIsbn()
    {
        var random = new Random();
        return new string(Enumerable.Range(0, 13).Select(_ => (char)random.Next(0, 10)).ToArray());
    }
}
