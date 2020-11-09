using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudyingWPF
{
    /// <summary>
    /// Interaction logic for ManageImagesWindow.xaml
    /// </summary>
    public partial class ManageImagesWindow : Window
    {
        public ManageImagesWindow()
        {
            InitializeComponent();
            BackButton.Click += OnClickBack;
        }

        private void OnClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }
    }
}
