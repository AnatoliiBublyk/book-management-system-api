using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using MediatR;

namespace BookManagement.Application.Commands;

public class AddBookCommand : IRequest<AddBookResponse>
{
    public AddBookRequest Request { get; set; }
}