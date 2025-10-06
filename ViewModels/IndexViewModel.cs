using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TicketManagementSystem.Core;
using TicketManagementSystem.Views;

namespace TicketManagementSystem.ViewModels
{
    public partial class IndexViewModel : ObservableObject
    {
        public ObservableCollection<string> CarouselImages { get; }

        public IndexViewModel()
        {
            CarouselImages = new ObservableCollection<string>
            {
                "Images/carousel1.jpg",
                "Images/carousel2.jpg",
                "Images/carousel3.jpg",
                "Images/carousel4.jpg",
                "Images/carousel5.jpg",
                "Images/carousel6.jpg"
            };
        }

        [RelayCommand]
        private void NavigateToLogin()
        {
            NavigationService.Navigate(new LoginPage());
        }

        [RelayCommand]
        private void NavigateToRegister()
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }
}