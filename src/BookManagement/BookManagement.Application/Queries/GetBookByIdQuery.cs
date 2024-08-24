using BookManagement.Contracts.Dtos;
using MediatR;

namespace BookManagement.Application.Queries;

public class GetBookByIdQuery : IRequest<BookDto>
{
    public Guid Id { get; set; }
}