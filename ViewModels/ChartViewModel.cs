using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TicketManagementSystem.Models;
using TicketManagementSystem.Services;

namespace TicketManagementSystem.ViewModels
{
    public partial class ChartViewModel : ObservableObject
    {
        private readonly TicketService _ticketService = new();
        private readonly UserService _userService = new();

        [ObservableProperty]
        private string filterUser = string.Empty;

        [ObservableProperty]
        private PlotModel ticketsChartModel = new();

        [ObservableProperty]
        private ObservableCollection<User> users = new();

        [ObservableProperty]
        private User? selectedUser;

        public ChartViewModel()
        {
            LoadUsers();
            LoadChart();
        }

        private void LoadUsers()
        {
            var allUsers = _userService.GetAll().ToList();

            // Add "All Users" placeholder
            allUsers.Insert(0, new User { Id = 0, Username = "All Users" });
            Users = new ObservableCollection<User>(allUsers);
        }

        [RelayCommand]
        private void Filter()
        {
            string username = string.Empty;

            if (SelectedUser != null && SelectedUser.Username != "All Users")
                username = SelectedUser.Username;
            else if (!string.IsNullOrWhiteSpace(FilterUser))
                username = FilterUser;

            LoadChart(username);
        }

        // Auto-refresh on typing or selection
        partial void OnFilterUserChanged(string value) => Filter();
        partial void OnSelectedUserChanged(User? value) => Filter();

        private void LoadChart(string? username = null)
        {
            IEnumerable<Ticket> tickets = string.IsNullOrWhiteSpace(username)
                ? _ticketService.GetAll()
                : _ticketService.GetByUser(username);

            var grouped = tickets
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            var model = new PlotModel { Title = "Tickets by Status" };

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                ItemsSource = grouped,
                LabelField = "Status",
                Title = "Status"
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Count",
                MinimumPadding = 0,
                AbsoluteMinimum = 0
            });

            var columnSeries = new ColumnSeries
            {
                Title = "Tickets",
                ItemsSource = grouped,
                ValueField = "Count",
                FillColor = OxyColors.CornflowerBlue
            };

            model.Series.Add(columnSeries);
            TicketsChartModel = model;
        }
    }
}