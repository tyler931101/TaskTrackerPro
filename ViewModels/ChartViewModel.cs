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

namespace TicketManagementSystem.ViewModels;

public partial class ChartViewModel : ObservableObject
{
    private readonly TicketService _ticketService = new();

    [ObservableProperty]
    private string filterUser = string.Empty;

    [ObservableProperty]
    private PlotModel _ticketsChartModel = new();

    public ChartViewModel()
    {
        LoadChart();
    }

    [RelayCommand]
    private void Filter()
    {
        LoadChart(FilterUser);
    }

    private void LoadChart(string? username = null)
    {
        IEnumerable<Ticket> tickets = string.IsNullOrWhiteSpace(username)
            ? _ticketService.GetAll()
            : _ticketService.GetByUser(username);

        var grouped = tickets
            .GroupBy(t => t.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToList();

        // Create chart
        var model = new PlotModel { Title = "Tickets by Status" };

        // Add axes
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

        // Add bar series
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