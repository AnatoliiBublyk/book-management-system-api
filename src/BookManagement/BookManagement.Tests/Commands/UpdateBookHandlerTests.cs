using BookManagement.Application.Commands.Handlers;
using BookManagement.Application.Commands;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Domain.Entities;
using MapsterMapper;
using Moq;

namespace BookManagement.Tests.Commands;

public class UpdateBookHandlerTests
{
    private readonly Mock<IBookRepo> _bookRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateBookHandler _handler;

    public UpdateBookHandlerTests()
    {
        _bookRepoMock = new Mock<IBookRepo>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateBookHandler(_mapperMock.Object, _bookRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUpdatedBook_WhenUpdateIsSuccessful()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var book = new Book { Id = bookId, Title = "OldTitle", Isbn = "1234567890123" };
        var updatedBook = new Book { Id = bookId, Title = "NewTitle", Isbn = "1234567890123" };
        var requestBody = new UpdateBookRequest { Title = "NewTitle", Isbn = "1234567890123" };
        var updatedBookDto = new BookDto { Id = bookId, Title = "NewTitle", Isbn = "1234567890123" };

        _bookRepoMock.Setup(repo => repo.GetByIdAsync(bookId)).ReturnsAsync(book);
        _bookRepoMock.Setup(repo => repo.UpdateAsync(book)).ReturnsAsync(updatedBook);
        _mapperMock.Setup(mapper => mapper.Map<BookDto>(updatedBook)).Returns(updatedBookDto);

        var request = new UpdateBookCommand { Id = bookId, Body = requestBody };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(updatedBookDto, result.Book);
        _bookRepoMock.Verify(repo => repo.GetByIdAsync(bookId), Times.Once);
        _bookRepoMock.Verify(repo => repo.UpdateAsync(book), Times.Once);
    }
}
