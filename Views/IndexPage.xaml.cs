using System.Windows.Controls;
using TicketManagementSystem.ViewModels;

namespace TicketManagementSystem.Views;

public partial class IndexPage : Page
{
    public IndexPage()
    {
        InitializeComponent();
        DataContext = new IndexViewModel();
        TicketManagementSystem.Core.PageTransitions.FadeIn(this);
    }
}