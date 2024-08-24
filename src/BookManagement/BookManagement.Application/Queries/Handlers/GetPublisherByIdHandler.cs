using MediatR;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using MapsterMapper;

namespace BookManagement.Application.Queries.Handlers;

public class GetPublisherByIdHandler(IMapper mapper, IPublisherRepo repo) : IRequestHandler<GetPublisherByIdQuery, PublisherDto>
{
    public async Task<PublisherDto> Handle(GetPublisherByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repo.GetByIdAsync(request.Id);
        return mapper.Map<PublisherDto>(result);
    }
}