using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using TicketManagementSystem.Core;
using TicketManagementSystem.Models;
using Microsoft.Win32;
using System.IO;
using TicketManagementSystem.Data;

namespace TicketManagementSystem.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty] private string _username = string.Empty;
        [ObservableProperty] private string _fullName = string.Empty;
        [ObservableProperty] private string _email = string.Empty;
        [ObservableProperty] private string _password = string.Empty;
        [ObservableProperty] private string _role = string.Empty;
        [ObservableProperty] private string? _avatarImage; // store file path

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
            Role = user.Role;
            AvatarImage = user.AvatarPath;
        }

        [RelayCommand]
        private void UploadAvatar()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Avatar",
                Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (dialog.ShowDialog() == true)
            {
                var user = UserSession.CurrentUser;
                if (user == null) return;

                string avatarsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Avatars");
                Directory.CreateDirectory(avatarsDir);

                string destPath = Path.Combine(avatarsDir, $"{user.Id}_{Path.GetFileName(dialog.FileName)}");
                File.Copy(dialog.FileName, destPath, true);

                user.AvatarPath = destPath;

                using var db = new AppDbContext();
                db.Users.Update(user);
                db.SaveChanges();

                AvatarImage = destPath;

                // 🔹 Notify other parts of app (like LayoutViewModel)
                UserSession.RaiseProfileUpdated();

                NotificationManager.Show("Avatar updated successfully!", "Success");
            }
        }

        [RelayCommand]
        private void UpdateProfile()
        {
            var user = UserSession.CurrentUser;
            if (user == null) return;

            user.FullName = FullName;
            user.Email = Email;
            user.Password = Password;

            using var db = new AppDbContext();
            db.Users.Update(user);
            db.SaveChanges();

            // 🔹 Also notify layout after profile update
            UserSession.RaiseProfileUpdated();

            MessageBox.Show("Profile updated successfully.",
                            "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NotificationManager.Show("Profile updated successfully.", "Success");
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