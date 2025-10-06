using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TicketManagementSystem.Core;
using TicketManagementSystem.Views;
using NavService = TicketManagementSystem.Core.NavigationService;

namespace TicketManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavService.Initialize(MainFrame);
            NavService.Navigate(new IndexPage());
        }
    }
}