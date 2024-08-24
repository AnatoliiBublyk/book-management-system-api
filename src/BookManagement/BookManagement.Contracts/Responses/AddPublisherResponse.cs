using BookManagement.Contracts.Dtos;

namespace BookManagement.Contracts.Responses;

public class AddPublisherResponse
{
    public string Status { get; set; }
    public string Message { get; set; }
    public PublisherDto Publisher { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}