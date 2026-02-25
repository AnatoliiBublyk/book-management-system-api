using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using MongoDB.Driver;

namespace BookManagement.Infrastructure.Mongo.Repo
{
    public class MongoPublisherRepo : IPublisherRepo
    {
        private readonly IMongoCollection<Publisher> _publishers;

        public MongoPublisherRepo(IMongoDatabase database)
        {
            _publishers = database.GetCollection<Publisher>("Publishers");
        }

        public async Task<IQueryable<Publisher>> GetAllAsync()
        {
            var all = await _publishers.Find(_ => true).ToListAsync();
            return all.AsQueryable();
        }

        public async Task<Publisher> GetByIdAsync(Guid id)
        {
            return await _publishers.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Publisher> AddAsync(Publisher entity)
        {
            await _publishers.InsertOneAsync(entity);
            return entity;
        }

        public async Task<Publisher> UpdateAsync(Publisher entity)
        {
            await _publishers.ReplaceOneAsync(p => p.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
            return entity;
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await _publishers.DeleteOneAsync(p => p.Id == id);
        }
    }
}