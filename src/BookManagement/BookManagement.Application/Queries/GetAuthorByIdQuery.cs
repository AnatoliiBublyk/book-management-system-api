using BookManagement.Contracts.Dtos;
using MediatR;

namespace BookManagement.Application.Queries;

public class GetAuthorByIdQuery : IRequest<AuthorDto>
{
    public Guid Id { get; set; }
}