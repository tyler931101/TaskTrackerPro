using System.Linq;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Services
{
    public class AuthenticationService
    {
        public User? Login(string username, string password)
        {
            using var db = new AppDbContext();
            return db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public bool Register(string username, string password, string email)
        {
            using var db = new AppDbContext();

            if (db.Users.Any(u => u.Username == username))
                return false;

            // 🧠 First registered user becomes Admin
            bool isFirstUser = !db.Users.Any();

            db.Users.Add(new User
            {
                Username = username,
                Password = password,
                Email = email,
                FullName = username,
                Role = isFirstUser ? "Admin" : "User"
            });

            db.SaveChanges();
            return true;
        }
    }
}