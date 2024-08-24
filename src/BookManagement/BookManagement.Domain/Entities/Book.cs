﻿namespace BookManagement.Domain.Entities;
public class Book
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Isbn { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Description { get; set; }

    public Guid AuthorId { get; set; }
    public Guid PublisherId { get; set; }

    // Navigation properties
    public Author Author { get; set; }
    public Publisher Publisher { get; set; }
}
