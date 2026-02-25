using MongoDB.Driver;

namespace BookManagement.Infrastructure.Mongo
{
    /// <summary>
    /// Thin wrapper over MongoDB client and database with helpers for collection naming.
    /// </summary>
    public sealed class MongoDbContext
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly MongoOptions _options;

        public MongoDbContext(IMongoClient client, MongoOptions options)
        {
            _client = client;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _database = _client.GetDatabase(options.DatabaseName);
        }

        public IMongoClient Client => _client;
        public IMongoDatabase Database => _database;

        /// <summary>
        /// Resolve collection for type <typeparamref name="TDocument"/>.
        /// Uses [MongoCollection("Name")] if present, otherwise type name.
        /// Applies CollectionPrefix when configured.
        /// </summary>
        public IMongoCollection<TDocument> GetCollection<TDocument>(string? overrideName = null)
        {
            var name = overrideName ?? GetDefaultCollectionName(typeof(TDocument));

            if (!string.IsNullOrWhiteSpace(_options.CollectionPrefix))
            {
                name = $"{_options.CollectionPrefix}_{name}";
            }

            return _database.GetCollection<TDocument>(name);
        }

        private static string GetDefaultCollectionName(Type t)
        {
            var attr = t.GetCustomAttributes(typeof(MongoCollectionAttribute), inherit: false)
                        .OfType<MongoCollectionAttribute>()
                        .FirstOrDefault();
            return attr?.Name ?? t.Name;
        }
    }
}
