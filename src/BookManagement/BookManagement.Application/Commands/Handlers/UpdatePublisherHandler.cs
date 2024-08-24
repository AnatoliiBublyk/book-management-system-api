using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class UpdatePublisherHandler(IMapper mapper, IPublisherRepo repo) : IRequestHandler<UpdatePublisherCommand, UpdatePublisherResponse>
{
    public async Task<UpdatePublisherResponse> Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
    {
        var authorEntity = await repo.GetByIdAsync(request.Id);

        var author = mapper.Map<Publisher>(request.Publisher);

        var result = await repo.UpdateAsync(author);
        return new UpdatePublisherResponse
        {
            Publisher = mapper.Map<PublisherDto>(result)
        };
    }
}