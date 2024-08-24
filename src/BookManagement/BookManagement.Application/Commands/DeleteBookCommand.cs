using MediatR;

namespace BookManagement.Application.Commands;

public class DeleteBookCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}