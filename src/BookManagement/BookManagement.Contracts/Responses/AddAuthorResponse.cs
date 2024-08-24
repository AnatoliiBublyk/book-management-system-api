﻿using BookManagement.Contracts.Dtos;

namespace BookManagement.Contracts.Responses;

public class AddAuthorResponse
{
    public string Status { get; set; }
    public string Message { get; set; }
    public AuthorDto Author { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}