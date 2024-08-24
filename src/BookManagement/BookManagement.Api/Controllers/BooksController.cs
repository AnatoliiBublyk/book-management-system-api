using BookManagement.Application.Commands;
using BookManagement.Application.Queries;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    [Consumes("application/json")]
    public class BooksController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<BookDto>> GetAllBooks()
        {
            var books = await mediator.Send(new GetAllBooksQuery());
            return books;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<BookDto> GetBookById(Guid id)
        {
            var book = await mediator.Send(new GetBookByIdQuery()
            {
                Id = id
            });
            return book;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<AddBookResponse>> AddBook([FromBody] AddBookRequest request)
        {
            var response = await mediator.Send(new AddBookCommand()
            {
                Request = request
            });
            return CreatedAtAction(nameof(GetBookById), new { id = response.Book.Id }, response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<UpdateBookResponse>> UpdateBook(Guid id, [FromBody] BookDto book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            var response = await mediator.Send(new UpdateBookCommand()
            {
                Id = id,
                Book = book
            });
            return Ok(response);

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var response = await mediator.Send(new DeleteBookCommand()
            {
                Id = id
            });
            return response ? NoContent() : BadRequest();
        }

    }
}
