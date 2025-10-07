using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TicketManagementSystem.Models;
using TicketManagementSystem.ViewModels;
using TicketManagementSystem.Core; // for UserSession and NotificationManager

namespace TicketManagementSystem.Views
{
    public partial class TicketManagementPage : Page
    {
        public TicketManagementPage()
        {
            InitializeComponent();
            DataContext = new TicketManagementViewModel();
        }

        // 🟢 Start dragging
        private void Ticket_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is Border border &&
                border.DataContext is Ticket ticket)
            {
                // ✅ Allow drag start for everyone; validation happens at drop
                DragDrop.DoDragDrop(border, ticket, DragDropEffects.Move);
            }
        }

        // 🟢 Visual feedback while dragging over a column
        private void StatusColumn_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(typeof(Ticket)) ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        // 🟢 Drop ticket onto a column
        private void StatusColumn_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Ticket))) return;
            if (sender is not Border border || border.Tag is not string newStatus) return;
            if (DataContext is not TicketManagementViewModel vm) return;

            var ticket = (Ticket)e.Data.GetData(typeof(Ticket))!;
            var currentUser = UserSession.CurrentUser;

            // 🔹 Prevent unlogged user
            if (currentUser == null)
            {
                NotificationManager.Show("You must be logged in to move tickets.", "Access Denied");
                return;
            }

            // 🔹 Restrict non-admins from moving others' tickets
            if (currentUser.Role != "Admin" && ticket.AssignedUserId != currentUser.Id)
            {
                NotificationManager.Show("You can only move your own tickets.", "Access Denied");
                return;
            }

            // 🔹 Skip if same status
            if (ticket.Status == newStatus)
                return;

            // ✅ Move ticket
            vm.MoveTicket(ticket, newStatus);
        }
    }
}