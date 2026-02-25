namespace BookManagement.Infrastructure.Mongo
{
    /// <summary>
    /// Strongly typed settings for MongoDB.
    /// Bind from configuration section: DatabaseSettings:Mongo
    /// </summary>
    public sealed class MongoOptions
    {
        /// <summary>MongoDB connection string. E.g. mongodb://localhost:27017</summary>
        public string ConnectionString { get; init; } = string.Empty;

        /// <summary>Database name. E.g. BookManagementDb</summary>
        public string DatabaseName { get; init; } = "BookManagementDb";

        /// <summary>
        /// Optional collection prefix. If set, actual collection names become: {prefix}_{Name}.
        /// Useful to isolate per-environment data in a shared cluster.
        /// </summary>
        public string? CollectionPrefix { get; init; }
    }
}
