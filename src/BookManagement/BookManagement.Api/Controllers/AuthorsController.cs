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
    public class AuthorsController(IMediator mediator, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<AuthorDto>> GetAllAuthors()
        {
            var Authors = await mediator.Send(new GetAllAuthorsQuery());
            return Authors;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<AuthorDto> GetAuthorById(Guid id)
        {
            var Author = await mediator.Send(new GetAuthorByIdQuery()
            {
                Id = id
            });
            return Author;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<AddAuthorResponse>> AddAuthor([FromBody] AddAuthorRequest request)
        {
            var response = await mediator.Send(new AddAuthorCommand()
            {
                Request = request
            });
            return CreatedAtAction(nameof(GetAuthorById), new { id = response.Author.Id }, response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<UpdateAuthorResponse>> UpdateAuthor(Guid id, [FromBody] AuthorDto author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            var response = await mediator.Send(new UpdateAuthorCommand()
            {
                Id = id,
                Author = author
            });
            return Ok(response);

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            var response = await mediator.Send(new DeleteAuthorCommand()
            {
                Id = id
            });
            return response ? NoContent() : BadRequest();
        }

    }
}
