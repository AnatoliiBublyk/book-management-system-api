using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using MediatR;

namespace BookManagement.Application.Commands;

public class AddBookCommand : IRequest<AddBookResponse>
{
    public AddBookRequest Request { get; set; }
}