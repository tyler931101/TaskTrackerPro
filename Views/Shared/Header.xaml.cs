using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TicketManagementSystem.Core;
using TicketManagementSystem.Views;

namespace TicketManagementSystem.Views.Shared
{
    public partial class Header : UserControl
    {
        public Header()
        {
            InitializeComponent();
        }

        private void Logo_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new IndexPage());
        }

        private void Login_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
