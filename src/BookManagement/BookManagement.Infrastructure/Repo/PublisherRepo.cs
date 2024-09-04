using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Database;

namespace BookManagement.Infrastructure.Repo;

public class PublisherRepo(AppDbContext context) : BaseRepo<Publisher>(context), IPublisherRepo
{
}