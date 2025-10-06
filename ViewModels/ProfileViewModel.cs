using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using TicketManagementSystem.Core;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _fullName = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _role = string.Empty; // 👈 New property

    public ProfileViewModel()
    {
        LoadProfile();
    }

    private void LoadProfile()
    {
        var user = UserSession.CurrentUser;
        if (user == null)
        {
            MessageBox.Show("No user logged in.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            NotificationManager.Show("No user logged in.", "Error");
            return;
        }

        Username = user.Username;
        FullName = user.FullName;
        Email = user.Email;
        Password = user.Password;
        Role = user.Role; // 👈 load role
    }

    [RelayCommand]
    private void UpdateProfile()
    {
        var user = UserSession.CurrentUser;
        if (user == null) return;

        user.FullName = FullName;
        user.Email = Email;
        user.Password = Password;

        MessageBox.Show("Profile updated successfully.",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        NotificationManager.Show("Profile updated successfully.", "Success");
    }

    [RelayCommand]
    private void Logout()
    {
        UserSession.Clear();
        NavigationService.Navigate(new TicketManagementSystem.Views.IndexPage());
    }
}