using MediatR;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using MapsterMapper;

namespace BookManagement.Application.Queries.Handlers;

public class GetAllPublishersHandler(IMapper mapper, IPublisherRepo repo) : IRequestHandler<GetAllPublishersQuery, IEnumerable<PublisherDto>>
{
    public async Task<IEnumerable<PublisherDto>> Handle(GetAllPublishersQuery request, CancellationToken cancellationToken)
    {
        var result = await repo.GetAllAsync();
        return mapper.Map<IEnumerable<PublisherDto>>(result);
    }
}