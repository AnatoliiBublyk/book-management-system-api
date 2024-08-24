using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using MediatR;

namespace BookManagement.Application.Commands;

public class UpdatePublisherCommand : IRequest<UpdatePublisherResponse>
{
    public Guid Id { get; set; }
    public PublisherDto Publisher { get; set; }
}