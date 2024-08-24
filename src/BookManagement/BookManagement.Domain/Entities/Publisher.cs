namespace BookManagement.Domain.Entities;
public class Publisher
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    // Navigation properties
    public ICollection<Book> Books { get; set; }
}
