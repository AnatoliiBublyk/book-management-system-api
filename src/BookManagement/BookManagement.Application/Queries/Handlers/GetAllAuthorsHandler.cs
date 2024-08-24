using MediatR;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using MapsterMapper;

namespace BookManagement.Application.Queries.Handlers;

public class GetAllAuthorsHandler(IMapper mapper, IAuthorRepo repo) : IRequestHandler<GetAllAuthorsQuery, IEnumerable<AuthorDto>>
{
    public async Task<IEnumerable<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        var result = await repo.GetAllAsync();
        return mapper.Map<IEnumerable<AuthorDto>>(result);
    }
}