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
    public class PublishersController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<PublisherDto>> GetAllPublishers()
        {
            var publishers = await mediator.Send(new GetAllPublishersQuery());
            return publishers;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<PublisherDto> GetPublisherById(Guid id)
        {
            var publisher = await mediator.Send(new GetPublisherByIdQuery()
            {
                Id = id
            });
            return publisher;
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
        public async Task<ActionResult<UpdatePublisherResponse>> UpdatePublisher(Guid id, [FromBody] UpdatePublisherRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var response = await mediator.Send(new UpdatePublisherCommand()
            {
                Id = id,
                Body = request
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
