using BookManagement.Application.Repo;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class DeleteBookHandler(IBookRepo repo) : IRequestHandler<DeleteBookCommand, bool>
{
    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(request.Id);

        await repo.DeleteByIdAsync(request.Id);

        return true;
    }
}