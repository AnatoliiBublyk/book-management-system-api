using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class AddPublisherHandler(IMapper mapper, IPublisherRepo repo) : IRequestHandler<AddPublisherCommand, AddPublisherResponse>
{
    public async Task<AddPublisherResponse> Handle(AddPublisherCommand request, CancellationToken cancellationToken)
    {
        var newPublisher = mapper.Map<Publisher>(request.Request);
        var result = await repo.AddAsync(newPublisher);
        return new AddPublisherResponse
        {
            Publisher = mapper.Map<PublisherDto>(result)
        };
    }
}