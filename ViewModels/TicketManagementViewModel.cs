using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TicketManagementSystem.Models;
using TicketManagementSystem.Services;
using System.Windows;
using TicketManagementSystem.Core;

namespace TicketManagementSystem.ViewModels;

public partial class TicketManagementViewModel : ObservableObject
{
    private readonly TicketService _ticketService = new();

    [ObservableProperty]
    private ObservableCollection<Ticket> _tickets = new();

    [ObservableProperty]
    private string _filterUser = string.Empty;

    [ObservableProperty]
    private Ticket _selectedTicket = new();

    [ObservableProperty]
    private string _newTitle = string.Empty;

    [ObservableProperty]
    private string _newDescription = string.Empty;

    [ObservableProperty]
    private string _newStatus = "Open";

    public TicketManagementViewModel()
    {
        LoadTickets();
    }

    private void LoadTickets(string? username = null)
    {
        var currentUser = UserSession.CurrentUser;
        if (currentUser == null) return;

        if (currentUser.Role == "Admin")
        {
            var list = _ticketService.GetAll();
            Tickets = new ObservableCollection<Ticket>(list);
        }
        else
        {
            var list = _ticketService.GetByUser(currentUser.Username);
            Tickets = new ObservableCollection<Ticket>(list);
        }
    }

    [RelayCommand]
    private void Filter()
    {
        LoadTickets(FilterUser);
    }

    [RelayCommand]
    private void AddTicket()
    {
        if (string.IsNullOrWhiteSpace(NewTitle))
        {
            NotificationManager.Show("Please enter a title.", "Warning");
            MessageBox.Show("Please enter a title.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var newTicket = new Ticket
        {
            Title = NewTitle,
            Description = NewDescription,
            CreatedBy = string.IsNullOrWhiteSpace(FilterUser) ? "guest" : FilterUser,
            Status = NewStatus
        };

        _ticketService.Add(newTicket);
        NotificationManager.Show("Ticket added successfully!", "Success");
        LoadTickets(FilterUser);

        NewTitle = string.Empty;
        NewDescription = string.Empty;
        NewStatus = "Open";
    }

    [RelayCommand]
    private void UpdateTicket()
    {
        if (SelectedTicket == null) return;
        _ticketService.Update(SelectedTicket);
        NotificationManager.Show("Ticket updated.", "Success");
        LoadTickets(FilterUser);
    }

    [RelayCommand]
    private void DeleteTicket()
    {
        var user = UserSession.CurrentUser;
        if (user == null) return;

        if (user.Role != "Admin")
        {
            MessageBox.Show("Only admins can delete tickets.", "Permission Denied",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            NotificationManager.Show("Only admins can delete tickets.", "Error");
            return;
        }

        if (SelectedTicket == null) return;

        if (MessageBox.Show("Delete this ticket?", "Confirm",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            _ticketService.Delete(SelectedTicket.Id);
            NotificationManager.Show("Ticket deleted.", "Success");
            LoadTickets(FilterUser);
        }
    }
}