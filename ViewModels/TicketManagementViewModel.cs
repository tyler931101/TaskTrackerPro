using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using TicketManagementSystem.Models;
using TicketManagementSystem.Data;
using TicketManagementSystem.Views.Shared;
using TicketManagementSystem.Core;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace TicketManagementSystem.ViewModels
{
    public partial class TicketManagementViewModel : ObservableObject
    {
        public ObservableCollection<TicketGroup> StatusGroups { get; set; } = new();
        public ObservableCollection<User> Users { get; set; } = new();

        [ObservableProperty] private string filterUser = string.Empty;
        [ObservableProperty] private User? selectedUser;
        [ObservableProperty] private bool isAdmin;

        public TicketManagementViewModel()
        {
            var user = UserSession.CurrentUser;
            IsAdmin = user != null && user.Role == "Admin";

            LoadUsers();
            LoadTickets();
        }

        private void LoadUsers()
        {
            using var db = new AppDbContext();
            Users.Clear();

            // Add "All Users" placeholder
            Users.Add(new User { Id = 0, Username = "All Users" });

            foreach (var user in db.Users.OrderBy(u => u.Username))
                Users.Add(user);
        }

        private void LoadTickets()
        {
            using var db = new AppDbContext();
            var tickets = db.Tickets
                .Include(t => t.AssignedUser)
                .ToList();

            // 🧩 Filter by selected user or text
            if (SelectedUser != null && SelectedUser.Username != "All Users")
                tickets = tickets.Where(t => t.AssignedUserId == SelectedUser.Id).ToList();
            else if (!string.IsNullOrWhiteSpace(FilterUser))
                tickets = tickets
                    .Where(t => t.AssignedUser != null &&
                                t.AssignedUser.Username.ToLower().Contains(FilterUser.ToLower()))
                    .ToList();

            var statuses = new[] { "To Do", "Progress", "Review", "Done", "Closed" };
            StatusGroups.Clear();

            foreach (var status in statuses)
            {
                var group = new TicketGroup(status,
                    new ObservableCollection<Ticket>(tickets.Where(t => t.Status == status)));
                StatusGroups.Add(group);
            }
        }

        [RelayCommand]
        private void Filter() => LoadTickets();

        // Auto-refresh on typing or selection
        partial void OnFilterUserChanged(string value) => LoadTickets();
        partial void OnSelectedUserChanged(User? value) => LoadTickets();

        [RelayCommand]
        private void AddTicket()
        {
            var currentUser = UserSession.CurrentUser;
            if (currentUser == null || currentUser.Role != "Admin")
            {
                NotificationManager.Show("Only administrators can create tickets.", "Access Denied");
                return;
            }

            var dialog = new TicketDialog();
            if (dialog.ShowDialog() == true && dialog.CreatedOrUpdatedTicket != null)
            {
                using var db = new AppDbContext();
                db.Tickets.Add(dialog.CreatedOrUpdatedTicket);
                db.SaveChanges();
                LoadTickets();

                NotificationManager.Show("Ticket created successfully!", "Success");
            }
        }

        [RelayCommand]
        private void EditTicket(Ticket ticket)
        {
            var dialog = new TicketDialog(ticket);
            if (dialog.ShowDialog() == true && dialog.CreatedOrUpdatedTicket != null)
            {
                using var db = new AppDbContext();
                db.Tickets.Update(dialog.CreatedOrUpdatedTicket);
                db.SaveChanges();
                LoadTickets();

                NotificationManager.Show("Ticket updated successfully!", "Success");
            }
        }

        [RelayCommand]
        private void DeleteTicket(Ticket ticket)
        {
            if (MessageBox.Show("Are you sure you want to delete this ticket?",
                                "Confirm Delete",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                using var db = new AppDbContext();
                var existing = db.Tickets.Find(ticket.Id);
                if (existing != null)
                {
                    db.Tickets.Remove(existing);
                    db.SaveChanges();
                    LoadTickets();
                    NotificationManager.Show("Ticket deleted.", "Deleted");
                }
            }
        }

        // ✅ Clean, minimal notification behavior when moving tickets
        public void MoveTicket(Ticket ticket, string newStatus)
        {
            using var db = new AppDbContext();
            var dbTicket = db.Tickets.FirstOrDefault(t => t.Id == ticket.Id);
            if (dbTicket == null) return;

            dbTicket.Status = newStatus;
            dbTicket.UpdatedAt = System.DateTime.Now;
            db.SaveChanges();

            LoadTickets();
            NotificationManager.Show($"Moved '{ticket.Title}' to '{newStatus}'.", "Updated");
        }
    }

    public class TicketGroup
    {
        public string Status { get; set; }
        public ObservableCollection<Ticket> Tickets { get; set; }

        public TicketGroup(string status, ObservableCollection<Ticket> tickets)
        {
            Status = status;
            Tickets = tickets;
        }
    }
}