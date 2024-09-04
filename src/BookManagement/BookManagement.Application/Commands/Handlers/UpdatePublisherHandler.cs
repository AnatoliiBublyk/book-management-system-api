using BookManagement.Application.Repo;
using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Responses;
using Mapster;
using MapsterMapper;
using MediatR;

namespace BookManagement.Application.Commands.Handlers;

public class UpdatePublisherHandler(IMapper mapper, IPublisherRepo repo) : IRequestHandler<UpdatePublisherCommand, UpdatePublisherResponse>
{
    public async Task<UpdatePublisherResponse> Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisherEntity = await repo.GetByIdAsync(request.Id);

        request.Body.Adapt(publisherEntity);

        var result = await repo.UpdateAsync(publisherEntity);
        return new UpdatePublisherResponse
        {
            Publisher = mapper.Map<PublisherDto>(result)
        };
    }
}