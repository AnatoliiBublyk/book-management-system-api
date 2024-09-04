using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using MediatR;

namespace BookManagement.Application.Commands;

public class UpdatePublisherCommand : IRequest<UpdatePublisherResponse>
{
    public Guid Id { get; set; }
    public UpdatePublisherRequest Body { get; set; }
}