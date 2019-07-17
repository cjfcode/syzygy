using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Syzygy;

namespace Syzygy
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public ICommand AddGameMenuItemClick { get; private set; }
        public ICommand SettingsMenuItemClick { get; private set; }
        public ICommand AboutMenuItemClick { get; private set; }
        public ICommand QuitMenuItemClick { get; private set; }

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

        private bool isPopupOpen;
        public bool IsPopupOpen
        {
            get
            {
                return isPopupOpen;
            }
            set
            {
                if (isPopupOpen == value)
                    return;
                isPopupOpen = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsPopupOpen)));
            }
        }

        public MainWindowViewModel()
        {
            AddGameMenuItemClick = new RelayCommand(AddGame_Click);
            SettingsMenuItemClick = new RelayCommand(Settings_Click);
            AboutMenuItemClick = new RelayCommand(About_Click);
            QuitMenuItemClick = new RelayCommand(Quit_Click);
        }

        private void AddGame_Click()
        {
            IsPopupOpen = false;

        }

        private void Settings_Click()
        {
            IsPopupOpen = false;
        }


        private void About_Click()
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
            IsPopupOpen = false;
        }

        private void Quit_Click()
        {
            //IsPopupOpen = false;
            Application.Current.Shutdown();
        }
    }
}