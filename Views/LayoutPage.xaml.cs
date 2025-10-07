using System.Windows;                       // ✅ Needed for RoutedEventArgs
using System.Windows.Controls;              // ✅ For Button, Page, etc.
using System.Windows.Controls.Primitives;   // ✅ For PlacementMode
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

        private void AvatarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.ContextMenu != null)
            {
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.Placement = PlacementMode.Bottom;
                button.ContextMenu.IsOpen = true;
            }
        }
    }
}