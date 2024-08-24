using MediatR;

namespace BookManagement.Application.Commands;

public class DeletePublisherCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}