namespace BookManagement.Domain.Entities;
public class Author
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    // Navigation properties
    public ICollection<Book> Books { get; set; }
}
