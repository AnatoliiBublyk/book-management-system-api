using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using MediatR;

namespace BookManagement.Application.Commands;

public class UpdateAuthorCommand : IRequest<UpdateAuthorResponse>
{
    public Guid Id { get; set; }
    public UpdateAuthorRequest Body { get; set; }
}