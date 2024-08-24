using MediatR;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using MapsterMapper;

namespace BookManagement.Application.Queries.Handlers;

public class GetBookByIdHandler(IMapper mapper, IBookRepo bookRepo) : IRequestHandler<GetBookByIdQuery, BookDto>
{
    public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await bookRepo.GetByIdAsync(request.Id);
        return mapper.Map<BookDto>(result);
    }
}