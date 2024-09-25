using System.ComponentModel.DataAnnotations;
using BookManagement.Domain.Entities.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace BookManagement.Domain.Entities;
public class Author : IdentityUser<Guid>, IEntity<Guid>
{
    public override Guid Id { get; set; }
    [EmailAddress]
    public override string Email { get; set; }
    public override string UserName { get; set; }
    public override string PasswordHash { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }


    // Navigation properties
    public ICollection<Book> Books { get; set; }
}
