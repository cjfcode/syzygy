using System.ComponentModel;
using System.Windows.Input;

namespace Syzygy
{
    internal class StatusBarViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public ICommand CloseStatusBarCommand { get; private set; }
        public ICommand OpenStatusBarCommand { get; private set; }

        public StatusBarViewModel()
        {
            // do something
        }

        public void CloseStatusBar()
        {
            // logic to close status bar
        }

        public void OpenStatusBar()
        {
            // logic to display status bar
        }
    }
}