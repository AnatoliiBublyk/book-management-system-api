using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using MediatR;

namespace BookManagement.Application.Commands;

public class UpdateAuthorCommand : IRequest<UpdateAuthorResponse>
{
    public Guid Id { get; set; }
    public AuthorDto Author { get; set; }
}