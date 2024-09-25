using BookManagement.Contracts.Requests;
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

        //Publisher
        Config.NewConfig<AddPublisherRequest, Publisher>()
            .Ignore(x => x.Id)
            .Ignore(x => x.Books);

    }
}