using System.Linq;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Services
{
    public class AuthenticationService
    {
        private readonly AppDbContext _db;

        public AuthenticationService()
        {
            _db = new AppDbContext();
        }

        /// <summary>
        /// Checks credentials and returns a User if valid; otherwise null.
        /// </summary>
        public User? Login(string username, string password)
        {
            // Find user by username
            var user = _db.Users.FirstOrDefault(u => u.Username == username);

            if (user is null)
                return null;

            // Verify password
            if (user.Password != password)
                return null;

            // Check if login is allowed
            if (!user.IsLoginAllowed)
                return user; // Still return so ViewModel can handle message

            // All good — return the authenticated user
            return user;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        public bool Register(string username, string password, string email)
        {
            if (_db.Users.Any(u => u.Username == username))
                return false;

            bool isFirstUser = !_db.Users.Any();

            var newUser = new User
            {
                Username = username,
                Password = password,
                Email = email,
                FullName = username,
                Role = isFirstUser ? "Admin" : "User",
                IsLoginAllowed = true
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();
            return true;
        }
    }
}