﻿using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Responses;
using Mapster;
using MapsterMapper;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class UpdateAuthorHandler(IMapper mapper, IAuthorRepo repo) : IRequestHandler<UpdateAuthorCommand, UpdateAuthorResponse>
{
    public async Task<UpdateAuthorResponse> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var authorEntity = await repo.GetByIdAsync(request.Id);

        request.Body.Adapt(authorEntity);

        var result = await repo.UpdateAsync(authorEntity);
        return new UpdateAuthorResponse
        {
            Author = mapper.Map<AuthorDto>(result)
        };
    }
}