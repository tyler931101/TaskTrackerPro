using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using TicketManagementSystem.Core;
using TicketManagementSystem.Services;

namespace TicketManagementSystem.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _email = string.Empty;

    private readonly AuthenticationService _auth = new();

    [RelayCommand]
    private void Register()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            MessageBox.Show("Please fill all required fields.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        bool success = _auth.Register(Username, Password, Email);
        if (success)
        {
            MessageBox.Show("Registration successful! You can now log in.",
                            "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NotificationManager.Show("Registration successful! You can now log in.", "Success");
            NavigationService.Navigate(new TicketManagementSystem.Views.LoginPage());
        }
        else
        {
            MessageBox.Show("Username already exists.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void Back() => NavigationService.GoBack();
}