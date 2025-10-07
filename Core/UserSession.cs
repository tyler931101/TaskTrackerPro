using System;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Core
{
    public static class UserSession
    {
        public static User? CurrentUser { get; private set; }

        // 🔹 Event to notify when profile data changes (avatar, name, etc.)
        public static event Action? ProfileUpdated;

        public static void SetUser(User user)
        {
            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        // 🔹 Call this whenever the current user's profile is updated
        public static void RaiseProfileUpdated()
        {
            ProfileUpdated?.Invoke();
        }
    }
}