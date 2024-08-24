using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class UpdateBookHandler(IMapper mapper, IBookRepo repo) : IRequestHandler<UpdateBookCommand, UpdateBookResponse>
{
    public async Task<UpdateBookResponse> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var bookEntity = await repo.GetByIdAsync(request.Id);

        var book = mapper.Map<Book>(request.Book);
        var result = await repo.UpdateAsync(book);
        return new UpdateBookResponse
        {
            Book = mapper.Map<BookDto>(result)
        };
    }
}