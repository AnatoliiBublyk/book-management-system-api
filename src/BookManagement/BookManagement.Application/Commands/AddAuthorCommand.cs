using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using MediatR;

namespace BookManagement.Application.Commands;

public class AddAuthorCommand : IRequest<AddAuthorResponse>
{
    public AddAuthorRequest Request { get; set; }
}