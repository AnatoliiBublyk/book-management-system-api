using System.Reflection;
using BookManagement.Api.Middlewares;
using BookManagement.Application.Queries;
using BookManagement.Application.Repo;
using BookManagement.Application.Services;
using BookManagement.Application.Services.Impl;
using BookManagement.Infrastructure.Database;
using BookManagement.Infrastructure.Repo;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMapper, Mapper>();

// Services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IHashService, Sha256Service>();

// CQRS specific
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllBooksQuery).Assembly));

// Repos
builder.Services.AddScoped<IBookRepo, BookRepo>();
builder.Services.AddScoped<IPublisherRepo, PublisherRepo>();
builder.Services.AddScoped<IAuthorRepo, AuthorRepo>();

// Controllers
builder.Services.AddControllers();

// OpenApi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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
