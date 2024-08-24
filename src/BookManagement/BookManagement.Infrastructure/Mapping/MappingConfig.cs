using BookManagement.Contracts.Dtos;
using BookManagement.Contracts.Requests;
using BookManagement.Contracts.Responses;
using BookManagement.Domain.Entities;
using Mapster;

namespace BookManagement.Api.Mapping;

public class MappingConfig
{
    public TypeAdapterConfig Config { get; }

    public MappingConfig()
    {
        Config = new TypeAdapterConfig();

        //Book
        Config.NewConfig<AddBookRequest, Book>()
            .Ignore(x => x.Id)
            .Ignore(x => x.Author)
            .Ignore(x => x.Publisher);

        //Author
        Config.NewConfig<AddAuthorRequest, Author>()
            .Ignore(x => x.Id)
            .Ignore(x => x.Books);

        //Publisher
        Config.NewConfig<AddPublisherRequest, Publisher>()
            .Ignore(x => x.Id)
            .Ignore(x => x.Books);

    }
}