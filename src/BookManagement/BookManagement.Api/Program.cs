using BookManagement.Api.Middlewares;
using BookManagement.Application.Queries;
using BookManagement.Application.Repo;
using BookManagement.Application.Services;
using BookManagement.Application.Services.Impl;
using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Database;
using BookManagement.Infrastructure.Repo;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using NLog.Web;
using NLog;
using Swashbuckle.AspNetCore.Filters;

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

    // Services
    builder.Services.AddScoped<IHashService, Sha256Service>();

    // CQRS specific
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllBooksQuery).Assembly));

    // Repos
    builder.Services.AddScoped<IBookRepo, BookRepo>();
    builder.Services.AddScoped<IPublisherRepo, PublisherRepo>();
    builder.Services.AddScoped<IAuthorRepo, AuthorRepo>();

    // Identity
    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication()
        .AddBearerToken(IdentityConstants.BearerScheme);
    builder.Services.AddIdentityCore<Author>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddApiEndpoints();
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    }

    );



    var app = builder.Build();

    // HTTP request pipeline.
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