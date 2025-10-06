using System.Windows.Controls;
using TicketManagementSystem.ViewModels;

namespace TicketManagementSystem.Views;

public partial class TicketManagementPage : Page
{
    public TicketManagementPage()
    {
        InitializeComponent();
        DataContext = new TicketManagementViewModel();
    }
}