using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Syzygy
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        ///// <summary>
        ///// Thickness used when inflating the margin on maximize.
        ///// </summary>
        public Thickness MaximizeFix { get; private set; } = new Thickness
        (
            SystemParameters.WindowNonClientFrameThickness.Left + SystemParameters.WindowResizeBorderThickness.Left,
            SystemParameters.WindowNonClientFrameThickness.Top + SystemParameters.WindowResizeBorderThickness.Top - SystemParameters.CaptionHeight - SystemParameters.BorderWidth,
            SystemParameters.WindowNonClientFrameThickness.Right + SystemParameters.WindowResizeBorderThickness.Right,
            SystemParameters.WindowNonClientFrameThickness.Bottom + SystemParameters.WindowResizeBorderThickness.Bottom
        );

        public ICommand MinimizeMainWindow { get; private set; }
        public ICommand MaximizeMainWindow { get; private set; }
        public ICommand CloseMainWindow { get; private set; }

        public MainWindowViewModel()
        {
            MinimizeMainWindow = new RelayCommand(MinimizeWindow);
            MaximizeMainWindow = new RelayCommand(MaximizeWindow);
            CloseMainWindow = new RelayCommand(CloseWindow);
        }

        private void CloseWindow()
        {
            SystemCommands.CloseWindow(Application.Current.MainWindow);
        }

        private void MaximizeWindow()
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Normal)
            {
                SystemCommands.MaximizeWindow(Application.Current.MainWindow);

            }
            else if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(Application.Current.MainWindow);
            }
        }

        private void MinimizeWindow()
        {
            SystemCommands.MinimizeWindow(Application.Current.MainWindow);
        }

    }
}