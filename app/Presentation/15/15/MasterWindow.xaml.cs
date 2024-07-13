using System.Windows;

namespace Fifteens
{
    /// <summary>
    /// Interaction logic for MasterWindow.xaml
    /// </summary>
    public partial class MasterWindow : Window
    {
        public MasterWindow()
        {
            this.InitializeComponent();
            this.Content = new LoginWindow();
        }
    }
}
