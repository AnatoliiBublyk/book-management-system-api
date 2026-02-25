using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BookManagement.Infrastructure.Mongo
{
    public static class MongoServiceCollectionExtensions
    {
        /// <summary>
        /// Registers MongoDB infrastructure services.
        /// Binds options from configuration section: DatabaseSettings:Mongo
        /// </summary>
        public static IServiceCollection AddMongoInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            var section = configuration.GetSection("DatabaseSettings:Mongo");
            var options = new MongoOptions();
            section.Bind(options);

            // Validate
            if (string.IsNullOrWhiteSpace(options.ConnectionString))
                throw new InvalidOperationException("Mongo ConnectionString is not configured (DatabaseSettings:Mongo:ConnectionString).");
            if (string.IsNullOrWhiteSpace(options.DatabaseName))
                throw new InvalidOperationException("Mongo DatabaseName is not configured (DatabaseSettings:Mongo:DatabaseName).");

            // Register singletons for client and options; Database is resolved via context
            services.AddSingleton(options);
            MongoConventions.Register();
                    services.AddSingleton<IMongoClient>(_ => new MongoClient(options.ConnectionString));
            services.AddSingleton<MongoDbContext>();

            return services;
        }
    }
}
