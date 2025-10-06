using System.Windows.Controls;
using TicketManagementSystem.ViewModels;

namespace TicketManagementSystem.Views;

public partial class AdminDashboardPage : Page
{
    public AdminDashboardPage()
    {
        InitializeComponent();
        DataContext = new AdminDashboardViewModel();
        TicketManagementSystem.Core.PageTransitions.FadeIn(this);
    }
}