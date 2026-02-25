using BookManagement.Api.Middlewares;
using BookManagement.Application.Queries;
using BookManagement.Application.Repo;
using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Database;
using BookManagement.Infrastructure.DatabaseSettings;
using BookManagement.Infrastructure.Repo;
using BookManagement.Infrastructure.Mongo;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using NLog.Web;
using NLog;
using Swashbuckle.AspNetCore.Filters;
using MongoDB.Driver;
using BookManagement.Infrastructure.Mongo.Repo;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Models;
using AspNetCore.Identity.MongoDbCore.Extensions;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddNLog();
        loggingBuilder.AddNLogWeb();
    });

    builder.Services.AddScoped<IMapper, Mapper>();

    // CQRS specific
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllBooksQuery).Assembly));

    // Load database settings
    var dbSettings = builder.Configuration
        .GetSection("DatabaseSettings")
        .Get<DatabaseSettings>();

    if (dbSettings.Provider.Equals("Sql", StringComparison.OrdinalIgnoreCase))
    {
        // EF-based Repos
        builder.Services.AddScoped<IBookRepo, BookRepo>();
        builder.Services.AddScoped<IPublisherRepo, PublisherRepo>();
        builder.Services.AddScoped<IAuthorRepo, AuthorRepo>();

        // Identity with EF Core (MySQL)
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);
        builder.Services.AddIdentityCore<Author>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString(dbSettings.Sql.ConnectionName)
            ));
    }
    else if (dbSettings.Provider.Equals("Mongo", StringComparison.OrdinalIgnoreCase))
    {
        MongoConventions.Register();

        var mongoConnection = builder.Configuration.GetConnectionString(dbSettings.Mongo.ConnectionName);
        var mongoDatabaseName = dbSettings.Mongo.DatabaseName;

        // Mongo client and database
        builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnection));
        builder.Services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoDatabaseName);
        });

        // Mongo-based Repos
        builder.Services.AddScoped<IBookRepo, MongoBookRepo>();
        builder.Services.AddScoped<IPublisherRepo, MongoPublisherRepo>();
        builder.Services.AddScoped<IAuthorRepo, MongoAuthorRepo>();

        // ?? Identity integration skipped for now (would require custom Mongo store)
        builder.Services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);
        // Identity with Mongo
        var identityConfig = new MongoDbIdentityConfiguration
        {
            MongoDbSettings = new MongoDbSettings
            {
                ConnectionString = mongoConnection,
                DatabaseName = mongoDatabaseName
            },
            IdentityOptionsAction = options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            }
        };

        builder.Services.AddIdentityCore<Author>()
                .AddRoles<MongoIdentityRole>()
                .AddMongoDbStores<Author, MongoIdentityRole, Guid>(mongoConnection, mongoDatabaseName)
                .AddSignInManager()
                .AddDefaultTokenProviders();
    }
    else
    {
        throw new InvalidOperationException("Unsupported database provider configured. Use 'Sql' or 'Mongo'.");
    }

    // Controllers
    builder.Services.AddControllers();

    // OpenApi
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.MapControllers();

    app.Run();
}
finally
{
    LogManager.Shutdown();
}
