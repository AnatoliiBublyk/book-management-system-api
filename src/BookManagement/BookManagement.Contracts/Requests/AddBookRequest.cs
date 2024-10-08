﻿namespace BookManagement.Contracts.Requests;

public class AddBookRequest
{
    public string Title { get; set; }
    public string Isbn { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Description { get; set; }

    public Guid AuthorId { get; set; }
    public Guid PublisherId { get; set; }
}