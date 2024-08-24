using BookManagement.Domain.Entities;

namespace BookManagement.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Publisher> Publishers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuring Author entity
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(100);
        });

        // Configuring Publisher entity
        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(p => p.Address)
                .HasMaxLength(255);
            entity.Property(p => p.Phone)
                .HasMaxLength(20);
            entity.Property(p => p.Email)
                .HasMaxLength(100);
        });

        // Configuring Book entity
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(b => b.Isbn)
                .IsRequired()
                .HasMaxLength(13);
            entity.HasIndex(b => b.Isbn)
                .IsUnique();
            entity.Property(b => b.PublicationDate)
                .IsRequired();
            entity.Property(b => b.Description);

            // Foreign keys
            entity.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}