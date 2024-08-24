using BookManagement.Application.Repo;
using BookManagement.Application.Services.Impl;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class AddAuthorHandler(IMapper mapper, IAuthorRepo repo, IHashService hashService) : IRequestHandler<AddAuthorCommand, AddAuthorResponse>
{
    public async Task<AddAuthorResponse> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
    {
        var newAuthor = mapper.Map<Author>(request.Request);
        newAuthor.PasswordHash = hashService.GetHash(request.Request.Password);
        var result = await repo.AddAsync(newAuthor);
        return new AddAuthorResponse
        {
            Author = mapper.Map<AuthorDto>(result)
        };
    }
}