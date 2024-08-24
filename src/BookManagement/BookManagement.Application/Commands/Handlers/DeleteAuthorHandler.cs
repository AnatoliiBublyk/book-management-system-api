using BookManagement.Application.Repo;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class DeleteAuthorHandler(IAuthorRepo repo) : IRequestHandler<DeleteAuthorCommand, bool>
{
    public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(request.Id);

        await repo.DeleteByIdAsync(request.Id);

        return true;
    }
}