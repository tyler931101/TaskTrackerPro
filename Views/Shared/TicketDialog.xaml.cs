using System;
using System.Linq;
using System.Windows;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Views.Shared
{
    public partial class TicketDialog : Window
    {
        public Ticket? CreatedOrUpdatedTicket { get; private set; }
        private readonly bool _isEditMode;

        public TicketDialog(Ticket? ticket = null)
        {
            InitializeComponent();

            using var db = new AppDbContext();
            UserCombo.ItemsSource = db.Users.ToList();

            if (ticket != null)
            {
                // 🧩 Edit mode
                _isEditMode = true;
                Title = "Edit Ticket";
                CreateButton.Content = "Update"; // Change button text dynamically

                TitleBox.Text = ticket.Title;
                DescriptionBox.Text = ticket.Description;
                DueDatePicker.SelectedDate = ticket.DueDate;

                // Pre-select user
                if (ticket.AssignedUserId != null)
                {
                    var selectedUser = db.Users.FirstOrDefault(u => u.Id == ticket.AssignedUserId);
                    if (selectedUser != null)
                        UserCombo.SelectedItem = selectedUser;
                }

                // Store original ticket for updating
                Tag = ticket;
            }
            else
            {
                // 🆕 Create mode
                _isEditMode = false;
                Title = "Create Ticket";
                CreateButton.Content = "Create";
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text) || DueDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please fill Title and Due Date.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedUser = UserCombo.SelectedItem as User;

            if (_isEditMode && Tag is Ticket existing)
            {
                // ✏️ Update existing ticket
                existing.Title = TitleBox.Text;
                existing.Description = DescriptionBox.Text;
                existing.DueDate = DueDatePicker.SelectedDate ?? DateTime.Now;
                existing.AssignedUserId = selectedUser?.Id;
                existing.UpdatedAt = DateTime.Now;

                CreatedOrUpdatedTicket = existing;
            }
            else
            {
                // 🆕 Create new ticket
                CreatedOrUpdatedTicket = new Ticket
                {
                    Title = TitleBox.Text,
                    Description = DescriptionBox.Text,
                    DueDate = DueDatePicker.SelectedDate ?? DateTime.Now,
                    AssignedUserId = selectedUser?.Id,
                    Status = "To Do",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}