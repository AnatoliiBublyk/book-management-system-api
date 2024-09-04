using BookManagement.Domain.Entities.Abstractions;

namespace BookManagement.Domain.Entities;
public class Publisher : BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    // Navigation properties
    public ICollection<Book> Books { get; set; }
}
