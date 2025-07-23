
using Microsoft.EntityFrameworkCore;
using ToDoList.DAL.Models;

namespace ToDoList.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; } = null!;
    public DbSet<Record> Record { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Records)
            .WithOne(r => r.User);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Record>()
            .HasOne(r => r.User)
            .WithMany(u => u.Records);
        
    }
}