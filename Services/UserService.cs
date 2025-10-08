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
            return db.Users
                     .OrderBy(u => u.Username)
                     .ToList();
        }

        public void SetLoginAllowed(User user, bool isAllowed)
        {
            using var db = new AppDbContext();
            var existingUser = db.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.IsLoginAllowed = isAllowed;
                db.SaveChanges();
            }
        }
    }
}