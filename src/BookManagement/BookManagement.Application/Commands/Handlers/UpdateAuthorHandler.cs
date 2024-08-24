using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class UpdateAuthorHandler(IMapper mapper, IAuthorRepo repo) : IRequestHandler<UpdateAuthorCommand, UpdateAuthorResponse>
{
    public async Task<UpdateAuthorResponse> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var authorEntity = await repo.GetByIdAsync(request.Id);

        var author = mapper.Map<Author>(request.Author);
        author.PasswordHash = authorEntity.PasswordHash;

        var result = await repo.UpdateAsync(author);
        return new UpdateAuthorResponse
        {
            Author = mapper.Map<AuthorDto>(result)
        };
    }
}