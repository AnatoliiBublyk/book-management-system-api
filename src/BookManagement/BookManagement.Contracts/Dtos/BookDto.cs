﻿namespace BookManagement.Contracts.Dtos;

public class BookDto : IBaseEntityDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string Isbn { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Description { get; set; }

    public Guid AuthorId { get; set; }
    public Guid PublisherId { get; set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
}