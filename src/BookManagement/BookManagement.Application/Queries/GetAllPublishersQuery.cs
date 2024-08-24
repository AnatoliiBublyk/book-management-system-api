using BookManagement.Contracts.Dtos;
using MediatR;

namespace BookManagement.Application.Queries;

public class GetAllPublishersQuery : IRequest<IEnumerable<PublisherDto>>
{
    
}