using BookManagement.Contracts.Dtos;
using MediatR;

namespace BookManagement.Application.Queries;

public class GetAllAuthorsQuery : IRequest<IEnumerable<AuthorDto>>
{
    
}