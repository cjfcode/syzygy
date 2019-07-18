using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Syzygy
{
    internal class PopupMenuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        private bool _IsPopupOpen;
        public bool IsPopupOpen
        {
            get
            {
                return _IsPopupOpen;
            }
            set
            {
                if (_IsPopupOpen == value)
                    return;
                _IsPopupOpen = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsPopupOpen)));
            }
        }

        public ICommand AboutMenuItemCommand { get; private set; }
        public ICommand AddGameMenuItemCommand { get; private set; }
        public ICommand SettingsMenuItemCommand { get; private set; }
        public ICommand QuitMenuItemCommand { get; private set; }
        public ICommand StatusBarMenuItemCommand { get; private set; } // will be moved to settings page later

        public PopupMenuViewModel()
        {
            AboutMenuItemCommand = new RelayCommand(AboutMenuItemClick);
            AddGameMenuItemCommand = new RelayCommand(AddGameMenuItemClick);
            QuitMenuItemCommand = new RelayCommand(QuitMenuItemClick);
            SettingsMenuItemCommand = new RelayCommand(SettingsMenuItemClick);
            StatusBarMenuItemCommand = new RelayCommand(StatusBarMenuItemClick);
        }

        private void AboutMenuItemClick()
        {
            if (AboutWindow.IsOpen)
            {
                IntPtr handle = new WindowInteropHelper(Application.Current.Windows.OfType<AboutWindow>().SingleOrDefault()).Handle;

                if (NativeMethods.IsIconic(handle)) // iconic = window is minimized
                {
                    NativeMethods.ShowWindow(handle, NativeMethods.nCmdShow.SW_RESTORE);
                }
                NativeMethods.SetForegroundWindow(handle);
            }
            else
            {
                AboutWindow about = new AboutWindow();
                about.Owner = Application.Current.MainWindow;
                about.Show();
                about.Owner = null;
            }
            IsPopupOpen = false;
        }

        private void AddGameMenuItemClick()
        {

            IsPopupOpen = false;
        }

        private void QuitMenuItemClick()
        {
            //IsPopupOpen = false;
            Application.Current.Shutdown();
        }

        private void SettingsMenuItemClick()
        {

            IsPopupOpen = false;
        }

        private void StatusBarMenuItemClick()
        {
            // logic to open or close or hide or show status bar
            IsPopupOpen = false;
        }
    }
}