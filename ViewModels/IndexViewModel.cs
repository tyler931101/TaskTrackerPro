using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using TicketManagementSystem.Core;
using TicketManagementSystem.Views;

namespace TicketManagementSystem.ViewModels
{
    public partial class IndexViewModel : ObservableObject
    {
        public ObservableCollection<string> CarouselImages { get; }

        [ObservableProperty]
        private string currentImage;

        private int _currentIndex;
        private readonly DispatcherTimer _carouselTimer;

        public IndexViewModel()
        {
            // ✅ Correct resource paths
            CarouselImages = new ObservableCollection<string>
            {
                "pack://application:,,,/TicketManagementSystem;component/Resources/Images/carousel1.jpg",
                "pack://application:,,,/TicketManagementSystem;component/Resources/Images/carousel2.jpg",
                "pack://application:,,,/TicketManagementSystem;component/Resources/Images/carousel3.jpg",
                "pack://application:,,,/TicketManagementSystem;component/Resources/Images/carousel4.jpg",
                "pack://application:,,,/TicketManagementSystem;component/Resources/Images/carousel5.jpg",
                "pack://application:,,,/TicketManagementSystem;component/Resources/Images/carousel6.jpg"
            };

            // Show first image
            CurrentImage = CarouselImages[0];
            _currentIndex = 0;

            // ✅ Auto-rotate every 3 seconds
            _carouselTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            _carouselTimer.Tick += CarouselTimer_Tick;
            _carouselTimer.Start();
        }

        private void CarouselTimer_Tick(object? sender, EventArgs e)
        {
            if (CarouselImages.Count == 0) return;

            _currentIndex = (_currentIndex + 1) % CarouselImages.Count;
            CurrentImage = CarouselImages[_currentIndex];
        }

        [RelayCommand]
        private void NavigateToLogin() => NavigationService.Navigate(new LoginPage());

        [RelayCommand]
        private void NavigateToRegister() => NavigationService.Navigate(new RegisterPage());
    }
}