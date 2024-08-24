using MediatR;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using MapsterMapper;

namespace BookManagement.Application.Queries.Handlers;

public class GetAuthorByIdHandler(IMapper mapper, IAuthorRepo repo) : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
{
    public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repo.GetByIdAsync(request.Id);
        return mapper.Map<AuthorDto>(result);
    }
}