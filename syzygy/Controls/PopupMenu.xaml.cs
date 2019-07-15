using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Syzygy.Windows;

namespace Syzygy.Controls
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
            if (PopupWindow.IsOpen)
            {
                IntPtr handle = new WindowInteropHelper(Application.Current.Windows.OfType<PopupWindow>().SingleOrDefault()).Handle; // may need to check window name within singleordefault function using x => x.Name or something if we create more popup windows

                if (NativeMethods.IsIconic(handle)) // iconic = window is minimized
                {
                    NativeMethods.ShowWindow(handle, NativeMethods.nCmdShow.SW_RESTORE);
                }
                NativeMethods.SetForegroundWindow(handle); // if the window is already opened, activate it
            }
            else
            {
                #region handle window startup placement
                /* set the owner of the window to be the window to position the startup location relative to.
                   child windows minimize when the parent is minimized and always stay on top of their parent,
                   we don't want this so remove the owner as soon as the window has been positioned */

                PopupWindow about = new PopupWindow();
                about.Owner = Application.Current.MainWindow;
                about.Show();
                about.Owner = null;

                /* alternative way of positioning the window center of the MainWindow that doesn't use Owner setting */
                //about.Left = Application.Current.MainWindow.Left + (Application.Current.MainWindow.ActualWidth - Application.Current.Windows.OfType<PopupWindow>().SingleOrDefault().Width) / 2;
                //about.Top = Application.Current.MainWindow.Top + (Application.Current.MainWindow.ActualHeight - Application.Current.Windows.OfType<PopupWindow>().SingleOrDefault().Height) / 2;
                //about.Show();
                #endregion
            }
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