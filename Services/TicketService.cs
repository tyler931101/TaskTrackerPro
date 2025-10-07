using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Services
{
    public class TicketService
    {
        // ✅ Get all tickets (includes assigned users)
        public IEnumerable<Ticket> GetAll()
        {
            using var db = new AppDbContext();
            return db.Tickets
                .Include(t => t.AssignedUser)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }

        // ✅ Get tickets assigned to a specific user
        public IEnumerable<Ticket> GetByUser(string username)
        {
            using var db = new AppDbContext();
            return db.Tickets
                .Include(t => t.AssignedUser)
                .Where(t => t.AssignedUser != null && t.AssignedUser.Username == username)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }

        // ✅ Add new ticket
        public void Add(Ticket ticket)
        {
            using var db = new AppDbContext();
            db.Tickets.Add(ticket);
            db.SaveChanges();
        }

        // ✅ Update existing ticket
        public void Update(Ticket ticket)
        {
            using var db = new AppDbContext();
            db.Tickets.Update(ticket);
            db.SaveChanges();
        }

        // ✅ Delete by ID
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
}