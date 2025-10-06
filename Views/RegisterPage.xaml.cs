using System.Windows.Controls;
using TicketManagementSystem.ViewModels;

namespace TicketManagementSystem.Views;

public partial class RegisterPage : Page
{
    private readonly RegisterViewModel _vm = new();

    public RegisterPage()
    {
        InitializeComponent();
        DataContext = _vm;
    }

    private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is PasswordBox pb)
            _vm.Password = pb.Password;
    }
}