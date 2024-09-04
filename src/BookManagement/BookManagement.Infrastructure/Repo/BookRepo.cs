using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Database;

namespace BookManagement.Infrastructure.Repo;

public class BookRepo(AppDbContext context) : BaseRepo<Book>(context), IBookRepo
{
}