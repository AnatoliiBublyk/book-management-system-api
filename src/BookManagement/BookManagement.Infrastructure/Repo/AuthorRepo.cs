using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Database;

namespace BookManagement.Infrastructure.Repo;

public class AuthorRepo(AppDbContext context) : BaseRepo<Author>(context), IAuthorRepo
{
}