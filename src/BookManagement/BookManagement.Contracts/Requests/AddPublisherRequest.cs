namespace BookManagement.Contracts.Requests;

public class AddPublisherRequest
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}