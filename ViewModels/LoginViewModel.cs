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

        [RelayCommand]
        private void Login()
        {
            var user = _auth.Login(Username, Password);

            if (user is null)
            {
                // Login failed due to either wrong credentials or not allowed
                NotificationManager.Show("Invalid username or password.", "Error");
                return;
            }

            if (!user.IsLoginAllowed)
            {
                NotificationManager.Show("Login is not allowed, please contact support team", "Success");
                return;
            }

            // ✅ User exists and is allowed
            UserSession.SetUser(user);
            NavigationService.Navigate(new TicketManagementSystem.Views.LayoutPage());
            NotificationManager.Show($"Welcome!, {user.FullName}!", "Success");
        }

        [RelayCommand]
        private void Back() => NavigationService.GoBack();

        [RelayCommand]
        private void NavigateToRegister() =>
            NavigationService.Navigate(new TicketManagementSystem.Views.RegisterPage());
    }
}