using BookManagement.Application.Repo;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class DeletePublisherHandler(IPublisherRepo repo) : IRequestHandler<DeletePublisherCommand, bool>
{
    public async Task<bool> Handle(DeletePublisherCommand request, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(request.Id);

        await repo.DeleteByIdAsync(request.Id);

        return true;
    }
}