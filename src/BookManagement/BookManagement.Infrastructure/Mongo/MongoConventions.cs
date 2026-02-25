using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace BookManagement.Infrastructure.Mongo
{
    /// <summary>
    /// Registers global BSON conventions/serializers for consistent documents.
    /// Call <see cref="Register"/> once at startup (already wired in AddMongoInfrastructure).
    /// </summary>
    public static class MongoConventions
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered) return;

            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("BookManagementConventions", pack, _ => true);

            // Serialize Guid as standard string to avoid legacy binary subtype confusion.
            // Adjust if you prefer Binary subtype with Standard representation.
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

            _registered = true;
        }
    }
}
