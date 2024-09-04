namespace BookManagement.Domain.Entities.Abstractions;

public class BaseEntity : IEntity<Guid>
{ 
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}