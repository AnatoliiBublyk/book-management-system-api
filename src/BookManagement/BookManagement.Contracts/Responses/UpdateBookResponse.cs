using BookManagement.Contracts.Dtos;

namespace BookManagement.Contracts.Responses;

public class UpdateBookResponse
{
    public string Status { get; set; }
    public string Message { get; set; }
    public BookDto Book { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;

}