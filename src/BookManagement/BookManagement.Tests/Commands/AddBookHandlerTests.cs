using BookManagement.Application.Commands.Handlers;
using BookManagement.Application.Commands;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Domain.Entities;
using Moq;
using MapsterMapper;

namespace BookManagement.Tests.Commands;

public class AddBookHandlerTests
{
    private readonly Mock<IBookRepo> _bookRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddBookHandler _handler;

    public AddBookHandlerTests()
    {
        _bookRepoMock = new Mock<IBookRepo>();
        _mapperMock = new Mock<IMapper>();
        _handler = new AddBookHandler(_mapperMock.Object, _bookRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAddBookResponse_WhenBookIsAdded()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var bookTitle = "Test Book";
        var bookIsbn = GenerateRandomIsbn();
        var authorId = Guid.NewGuid();
        var publisherId = Guid.NewGuid();

        var request = new AddBookCommand
        {
            Request = new AddBookRequest
            {
                Title = bookTitle,
                Isbn = bookIsbn,
                PublicationDate = DateTime.Now,
                Description = "Test Description",
                AuthorId = authorId,
                PublisherId = publisherId
            }
        };

        var newBook = new Book
        {
            Id = bookId,
            Title = bookTitle,
            Isbn = bookIsbn,
            PublicationDate = DateTime.Now,
            Description = "Test Description",
            AuthorId = authorId,
            PublisherId = publisherId
        };

        var bookDto = new BookDto
        {
            Id = bookId,
            Title = bookTitle,
            Isbn = bookIsbn,
            PublicationDate = newBook.PublicationDate,
            Description = "Test Description",
            AuthorId = authorId,
            PublisherId = publisherId
        };

        _mapperMock.Setup(mapper => mapper.Map<Book>(request.Request)).Returns(newBook);
        _bookRepoMock.Setup(repo => repo.AddAsync(newBook)).ReturnsAsync(newBook);
        _mapperMock.Setup(mapper => mapper.Map<BookDto>(newBook)).Returns(bookDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equivalent(bookDto, result.Book);
    }

    private string GenerateRandomIsbn()
    {
        var random = new Random();
        var isbn = new char[13];
        for (int i = 0; i < 13; i++)
        {
            isbn[i] = (char)random.Next(0, 9);
        }
        return new string(isbn);
    }
}
