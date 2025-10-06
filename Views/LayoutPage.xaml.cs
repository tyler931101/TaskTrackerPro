using System.Windows.Controls;
using TicketManagementSystem.ViewModels;

namespace TicketManagementSystem.Views
{
    public partial class LayoutPage : Page
    {
        public LayoutPage()
        {
            InitializeComponent();
            DataContext = new LayoutViewModel(ContentFrame);
        }
    }
}