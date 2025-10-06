using System.Windows.Controls;
using TicketManagementSystem.Core;

namespace TicketManagementSystem.Views
{
    public partial class IndexPage : Page
    {
        public IndexPage()
        {
            InitializeComponent();
            TicketManagementSystem.Core.PageTransitions.FadeIn(this); // optional fade-in animation
        }
    }
}