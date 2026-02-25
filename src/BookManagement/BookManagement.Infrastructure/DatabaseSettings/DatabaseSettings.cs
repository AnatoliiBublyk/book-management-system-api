namespace BookManagement.Infrastructure.DatabaseSettings
{
    public class DatabaseSettings
    {
        public string? Provider { get; set; }
        public SqlSettings Sql { get; set; } = new();
        public MongoSettings Mongo { get; set; } = new();
    }
}
