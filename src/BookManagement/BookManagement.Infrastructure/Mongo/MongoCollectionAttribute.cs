namespace BookManagement.Infrastructure.Mongo
{
    /// <summary>
    /// Optional attribute to override the MongoDB collection name for a given entity type.
    /// If not specified, the type name will be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MongoCollectionAttribute : Attribute
    {
        public MongoCollectionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
