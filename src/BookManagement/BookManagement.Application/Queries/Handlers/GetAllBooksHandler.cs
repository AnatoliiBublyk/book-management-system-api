using MediatR;
using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using MapsterMapper;

namespace BookManagement.Application.Queries.Handlers;

public class GetAllBooksHandler(IMapper mapper, IBookRepo bookRepo) : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
{
    public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var result = await bookRepo.GetAllAsync();
        return mapper.Map<IEnumerable<BookDto>>(result);
    }
}