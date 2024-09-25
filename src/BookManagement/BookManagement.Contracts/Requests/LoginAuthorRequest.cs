namespace BookManagement.Contracts.Requests;

public class LoginAuthorRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}