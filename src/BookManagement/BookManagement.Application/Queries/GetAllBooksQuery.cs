using BookManagement.Contracts.Dtos;
using MediatR;

namespace BookManagement.Application.Queries;

public class GetAllBooksQuery : IRequest<IEnumerable<BookDto>>
{
    
}