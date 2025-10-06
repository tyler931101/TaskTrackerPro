using System.Windows.Controls;
using TicketManagementSystem.ViewModels;

namespace TicketManagementSystem.Views;

public partial class ChartPage : Page
{
    public ChartPage()
    {
        InitializeComponent();
        DataContext = new ChartViewModel();
    }
}