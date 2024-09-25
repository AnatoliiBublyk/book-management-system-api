using BookManagement.Application.Commands;
using BookManagement.Application.Queries;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    [Consumes("application/json")]
    [Authorize]
    public class AuthorsController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<AuthorDto>> GetAllAuthors()
        {
            var authors = await mediator.Send(new GetAllAuthorsQuery());
            return authors;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<AuthorDto> GetAuthorById(Guid id)
        {
            var author = await mediator.Send(new GetAuthorByIdQuery()
            {
                Id = id
            });
            return author;
        }
        
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<UpdateAuthorResponse>> UpdateAuthor(Guid id, [FromBody] UpdateAuthorRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var response = await mediator.Send(new UpdateAuthorCommand()
            {
                Id = id,
                Body = request
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
