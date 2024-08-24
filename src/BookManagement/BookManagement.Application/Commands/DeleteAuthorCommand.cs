using MediatR;

namespace BookManagement.Application.Commands;

public class DeleteAuthorCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}