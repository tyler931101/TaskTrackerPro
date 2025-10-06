using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using TicketManagementSystem.Core;
using TicketManagementSystem.Services;

namespace TicketManagementSystem.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty] private string _username = string.Empty;
        [ObservableProperty] private string _password = string.Empty;

        private readonly AuthenticationService _auth = new();

        // ✅ LOGIN COMMAND
        [RelayCommand]
        private void Login()
        {
            var user = _auth.Login(Username, Password);
            if (user is not null)
            {
                UserSession.SetUser(user);
                NavigationService.Navigate(new TicketManagementSystem.Views.LayoutPage());
                NotificationManager.Show($"Welcome back, {user.FullName}!", "Success");
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                NotificationManager.Show("Invalid username or password.", "Error");
            }
        }

        // ✅ BACK COMMAND — e.g., return to IndexPage
        [RelayCommand]
        private void Back() => NavigationService.GoBack();

        // ✅ NEW: NAVIGATE TO REGISTER PAGE
        [RelayCommand]
        private void NavigateToRegister()
        {
            NavigationService.Navigate(new TicketManagementSystem.Views.RegisterPage());
        }
    }
}