using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using MediatR;

namespace BookManagement.Application.Commands;

public class AddPublisherCommand : IRequest<AddPublisherResponse>
{
    public AddPublisherRequest Request { get; set; }
}