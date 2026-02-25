using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using MongoDB.Driver;

namespace BookManagement.Infrastructure.Mongo.Repo
{
    public class MongoBookRepo : IBookRepo
    {
        private readonly IMongoCollection<Book> _books;

        public MongoBookRepo(IMongoDatabase database)
        {
            _books = database.GetCollection<Book>("Books");
        }

        public async Task<IQueryable<Book>> GetAllAsync()
        {
            var all = await _books.Find(_ => true).ToListAsync();
            return all.AsQueryable();
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return await _books.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Book> AddAsync(Book entity)
        {
            await _books.InsertOneAsync(entity);
            return entity;
        }

        public async Task<Book> UpdateAsync(Book entity)
        {
            await _books.ReplaceOneAsync(b => b.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
            return entity;
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await _books.DeleteOneAsync(b => b.Id == id);
        }
    }
}
