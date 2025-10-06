using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Models;
using TicketManagementSystem.Core;

namespace TicketManagementSystem.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    private static bool _created = false;

    public AppDbContext()
    {
        if (!_created)
        {
            _created = true;
            Database.EnsureCreated(); // Automatically creates DB on first run
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = ConfigManager.Settings.DatabasePath;
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);

    //    // Seed default admin user
    //    modelBuilder.Entity<User>().HasData(new User
    //    {
    //        Username = "admin",
    //        Password = "1234",
    //        FullName = "Administrator",
    //        Email = "admin@demo.com"
    //        Role = "Admin"
    //    });
    //}
}