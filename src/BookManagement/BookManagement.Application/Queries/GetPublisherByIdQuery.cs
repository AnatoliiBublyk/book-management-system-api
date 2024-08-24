using BookManagement.Contracts.Dtos;
using MediatR;

namespace BookManagement.Application.Queries;

public class GetPublisherByIdQuery : IRequest<PublisherDto>
{
    public Guid Id { get; set; }
}