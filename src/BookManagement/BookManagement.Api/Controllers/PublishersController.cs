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
    public class PublishersController(IMediator mediator, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<PublisherDto>> GetAllPublishers()
        {
            var Publishers = await mediator.Send(new GetAllPublishersQuery());
            return Publishers;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<PublisherDto> GetPublisherById(Guid id)
        {
            var Publisher = await mediator.Send(new GetPublisherByIdQuery()
            {
                Id = id
            });
            return Publisher;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<AddPublisherResponse>> AddPublisher([FromBody] AddPublisherRequest request)
        {
            var response = await mediator.Send(new AddPublisherCommand()
            {
                Request = request
            });
            return CreatedAtAction(nameof(GetPublisherById), new { id = response.Publisher.Id }, response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<UpdatePublisherResponse>> UpdatePublisher(Guid id, [FromBody] PublisherDto author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            var response = await mediator.Send(new UpdatePublisherCommand()
            {
                Id = id,
                Publisher = author
            });
            return Ok(response);

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeletePublisher(Guid id)
        {
            var response = await mediator.Send(new DeletePublisherCommand()
            {
                Id = id
            });
            return response ? NoContent() : BadRequest();
        }
    }
}
