namespace BookManagement.Domain.Entities.Abstractions;

public interface IEntity<T>
{
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
}