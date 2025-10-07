using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;
using TicketManagementSystem.Core;

namespace TicketManagementSystem.ViewModels
{
    public partial class LayoutViewModel : ObservableObject
    {
        private readonly Frame _frame;

        [ObservableProperty]
        private bool _isAdmin;

        [ObservableProperty]
        private string _currentUsername = string.Empty;

        [ObservableProperty]
        private string _currentRole = string.Empty;

        [ObservableProperty]
        private string _avatarPath = "/Assets/default-avatar.png";

        [ObservableProperty]
        private string _fullName = string.Empty;

        public LayoutViewModel(Frame frame)
        {
            _frame = frame;
            _frame.Navigate(new TicketManagementSystem.Views.TicketManagementPage());

            var user = UserSession.CurrentUser;

            if (user != null)
            {
                IsAdmin = user.Role == "Admin";
                CurrentUsername = user.Username;
                CurrentRole = user.Role;
                AvatarPath = string.IsNullOrEmpty(user.AvatarPath) ? "/Assets/default-avatar.png" : user.AvatarPath;
                FullName = user.FullName;
            }

            // 🔹 Subscribe to profile updates
            UserSession.ProfileUpdated += OnProfileUpdated;

            NotificationManager.Show($"Welcome, {CurrentUsername} ({CurrentRole})", "Info");
        }

        private void OnProfileUpdated()
        {
            var user = UserSession.CurrentUser;
            if (user == null) return;

            AvatarPath = string.IsNullOrEmpty(user.AvatarPath)
                ? "/Assets/default-avatar.png"
                : user.AvatarPath;
            CurrentUsername = user.Username;
            FullName = user.FullName;
        }

        [RelayCommand]
        private void NavigateTickets() =>
            _frame.Navigate(new TicketManagementSystem.Views.TicketManagementPage());

        [RelayCommand]
        private void NavigateCharts() =>
            _frame.Navigate(new TicketManagementSystem.Views.ChartPage());

        [RelayCommand]
        private void NavigateProfile() =>
            _frame.Navigate(new TicketManagementSystem.Views.ProfilePage());

        [RelayCommand]
        private void ToggleTheme() =>
            ThemeManager.ToggleTheme();

        [RelayCommand]
        private void NavigateDashboard()
        {
            if (!IsAdmin)
            {
                NotificationManager.Show("Access denied. Admins only.", "Error");
                return;
            }

            _frame.Navigate(new TicketManagementSystem.Views.AdminDashboardPage());
        }

        [RelayCommand]
        private void Logout()
        {
            UserSession.Logout();
            NavigationService.Navigate(new TicketManagementSystem.Views.LoginPage());
            NotificationManager.Show("You have been logged out.", "Info");
        }
    }
}