using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using MediatR;

namespace BookManagement.Application.Commands;

public class UpdateBookCommand : IRequest<UpdateBookResponse>
{
    public Guid Id { get; set; }
    public BookDto Book { get; set; }
}