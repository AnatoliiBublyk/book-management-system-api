using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using MongoDB.Driver;

namespace BookManagement.Infrastructure.Mongo.Repo
{
    public class MongoAuthorRepo : IAuthorRepo
    {
        private readonly IMongoCollection<Author> _authors;

        public MongoAuthorRepo(IMongoDatabase database)
        {
            _authors = database.GetCollection<Author>("authors");
        }

        public async Task<IQueryable<Author>> GetAllAsync()
        {
            var all = await _authors.Find(_ => true).ToListAsync();
            return all.AsQueryable();
        }

        public async Task<Author> GetByIdAsync(Guid id)
        {
            return await _authors.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Author> AddAsync(Author entity)
        {
            await _authors.InsertOneAsync(entity);
            return entity;
        }

        public async Task<Author> UpdateAsync(Author entity)
        {
            await _authors.ReplaceOneAsync(a => a.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
            return entity;
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await _authors.DeleteOneAsync(a => a.Id == id);
        }
    }
}
