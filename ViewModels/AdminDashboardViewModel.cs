using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketManagementSystem.Models;
using TicketManagementSystem.Services;

namespace TicketManagementSystem.ViewModels
{
    public partial class AdminDashboardViewModel : ObservableObject
    {
        private readonly UserService _userService = new();

        [ObservableProperty]
        private ObservableCollection<User> _users = new();

        public AdminDashboardViewModel()
        {
            LoadUsers();
        }

        [RelayCommand]
        private void LoadUsers()
        {
            Users = new ObservableCollection<User>(_userService.GetAll());
        }

        [RelayCommand]
        private void AllowLogin(User? user)
        {
            if (user == null) return;
            _userService.SetLoginAllowed(user, true);
            LoadUsers();
        }

        [RelayCommand]
        private void StopLogin(User? user)
        {
            if (user == null) return;
            _userService.SetLoginAllowed(user, false);
            LoadUsers();
        }
    }
}