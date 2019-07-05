using System.Windows;
using System.Windows.Controls;

namespace syzygy.Controls
{
    /// <summary>
    /// Interaction logic for PopupMenu.xaml
    /// </summary>
    public partial class PopupMenu : UserControl
    {
        public PopupMenu()
        {
            InitializeComponent();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
