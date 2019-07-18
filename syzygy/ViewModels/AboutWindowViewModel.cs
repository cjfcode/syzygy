using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Syzygy
{
    internal class AboutWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public ICommand MinimizeAboutWindowCommand { get; private set; }
        public ICommand MaximizeAboutWindowCommand { get; private set; }
        public ICommand CloseAboutWindowCommand { get; private set; }

        public Thickness MaximizeFix { get; set; } = new Thickness
        (
            SystemParameters.WindowNonClientFrameThickness.Left + SystemParameters.WindowResizeBorderThickness.Left,
            SystemParameters.WindowNonClientFrameThickness.Top + SystemParameters.WindowResizeBorderThickness.Top - SystemParameters.CaptionHeight - SystemParameters.BorderWidth,
            SystemParameters.WindowNonClientFrameThickness.Right + SystemParameters.WindowResizeBorderThickness.Right,
            SystemParameters.WindowNonClientFrameThickness.Bottom + SystemParameters.WindowResizeBorderThickness.Bottom
        );

        public AboutWindowViewModel()
        {
            MinimizeAboutWindowCommand = new RelayCommand(MinimizeAboutWindow);
            MaximizeAboutWindowCommand = new RelayCommand(MaximizeAboutWindow);
            CloseAboutWindowCommand = new RelayCommand(CloseAboutWindow);
        }

        private void CloseAboutWindow()
        {
            SystemCommands.CloseWindow(Application.Current.Windows.OfType<AboutWindow>().FirstOrDefault());
        }

        private void MaximizeAboutWindow()
        {
            if (Application.Current.Windows.OfType<AboutWindow>().FirstOrDefault().WindowState == WindowState.Normal)
            {
                SystemCommands.MaximizeWindow(Application.Current.Windows.OfType<AboutWindow>().FirstOrDefault());

            }
            else if (Application.Current.Windows.OfType<AboutWindow>().FirstOrDefault().WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(Application.Current.Windows.OfType<AboutWindow>().FirstOrDefault());
            }
        }

        private void MinimizeAboutWindow()
        {
            SystemCommands.MinimizeWindow(Application.Current.Windows.OfType<AboutWindow>().FirstOrDefault());
        }
    }
}