using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Services;

public class TicketService
{
    public IEnumerable<Ticket> GetAll()
    {
        using var db = new AppDbContext();
        return db.Tickets.OrderByDescending(t => t.CreatedAt).ToList();
    }

    public IEnumerable<Ticket> GetByUser(string username)
    {
        using var db = new AppDbContext();
        return db.Tickets.Where(t => t.CreatedBy == username).OrderByDescending(t => t.CreatedAt).ToList();
    }

    public void Add(Ticket ticket)
    {
        using var db = new AppDbContext();
        db.Tickets.Add(ticket);
        db.SaveChanges();
    }

    public void Update(Ticket ticket)
    {
        using var db = new AppDbContext();
        db.Tickets.Update(ticket);
        db.SaveChanges();
    }

    public void Delete(int id)
    {
        using var db = new AppDbContext();
        var existing = db.Tickets.Find(id);
        if (existing != null)
        {
            db.Tickets.Remove(existing);
            db.SaveChanges();
        }
    }
}