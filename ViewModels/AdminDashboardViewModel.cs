using CommunityToolkit.Mvvm.ComponentModel;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using System.Linq;
using TicketManagementSystem.Data;
using TicketManagementSystem.Models;
using TicketManagementSystem.Core;

namespace TicketManagementSystem.ViewModels;

public partial class AdminDashboardViewModel : ObservableObject
{
    [ObservableProperty] private int _totalTickets;
    [ObservableProperty] private int _openTickets;
    [ObservableProperty] private int _inProgressTickets;
    [ObservableProperty] private int _resolvedTickets;
    [ObservableProperty] private int _totalUsers;
    [ObservableProperty] private ObservableCollection<Ticket> _recentTickets = new();
    [ObservableProperty] private PlotModel _ticketsPieChartModel = new();

    public AdminDashboardViewModel()
    {
        if (UserSession.CurrentUser?.Role == "Admin")
        {
            LoadDashboard();
        }
    }

    private void LoadDashboard()
    {
        using var db = new AppDbContext();

        var tickets = db.Tickets.ToList();
        var users = db.Users.ToList();

        TotalTickets = tickets.Count;
        OpenTickets = tickets.Count(t => t.Status == "Open");
        InProgressTickets = tickets.Count(t => t.Status == "In Progress");
        ResolvedTickets = tickets.Count(t => t.Status == "Resolved");
        TotalUsers = users.Count;

        // --- OxyPlot Pie Chart ---
        var model = new PlotModel { Title = "Tickets by Status" };

        var pie = new PieSeries
        {
            StrokeThickness = 2,
            InsideLabelPosition = 0.8,
            AngleSpan = 360,
            StartAngle = 0
        };

        pie.Slices.Add(new PieSlice("Open", OpenTickets) { Fill = OxyColors.Orange });
        pie.Slices.Add(new PieSlice("In Progress", InProgressTickets) { Fill = OxyColors.Gold });
        pie.Slices.Add(new PieSlice("Resolved", ResolvedTickets) { Fill = OxyColors.ForestGreen });

        model.Series.Add(pie);
        TicketsPieChartModel = model;

        // --- Recent Tickets (5 most recent) ---
        RecentTickets = new ObservableCollection<Ticket>(
            tickets.OrderByDescending(t => t.CreatedAt).Take(5)
        );
    }
}