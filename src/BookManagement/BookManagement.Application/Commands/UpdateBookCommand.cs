using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using MediatR;

namespace BookManagement.Application.Commands;

public class UpdateBookCommand : IRequest<UpdateBookResponse>
{
    public Guid Id { get; set; }
    public UpdateBookRequest Body { get; set; }
}