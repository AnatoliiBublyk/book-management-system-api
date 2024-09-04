using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Responses;
using Mapster;
using MapsterMapper;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class UpdateBookHandler(IMapper mapper, IBookRepo repo) : IRequestHandler<UpdateBookCommand, UpdateBookResponse>
{
    public async Task<UpdateBookResponse> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var bookEntity = await repo.GetByIdAsync(request.Id);

        request.Body.Adapt(bookEntity);

        var result = await repo.UpdateAsync(bookEntity);
        return new UpdateBookResponse
        {
            Book = mapper.Map<BookDto>(result)
        };
    }
}