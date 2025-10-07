using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Services
{
    public class UserService
    {
        public IEnumerable<User> GetAll()
        {
            using var db = new AppDbContext();
            return db.Users.OrderBy(u => u.Username).ToList();
        }
    }
}