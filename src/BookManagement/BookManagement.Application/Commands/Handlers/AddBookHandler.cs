using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class AddBookHandler(IMapper mapper, IBookRepo repo) : IRequestHandler<AddBookCommand, AddBookResponse>
{
    public async Task<AddBookResponse> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var newBook = mapper.Map<Book>(request.Request);
        var result = await repo.AddAsync(newBook);
        return new AddBookResponse
        {
            Book = mapper.Map<BookDto>(result)
        };
    }
}