using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Syzygy
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        public static bool IsOpen { get; private set; }

        public PopupWindow()
        {
            InitializeComponent();
        }

        private void PopupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hWnd = new WindowInteropHelper(this).Handle;

            IntPtr myStyle = new IntPtr((int)NativeMethods.WindowStyles.WS_CAPTION | (int)NativeMethods.WindowStyles.WS_MAXIMIZEBOX |
                                        (int)NativeMethods.WindowStyles.WS_MINIMIZEBOX | (int)NativeMethods.WindowStyles.WS_SYSMENU |
                                        (int)NativeMethods.WindowStyles.WS_SIZEBOX);

            NativeMethods.SetWindowLongPtr(new HandleRef(null, hWnd), NativeMethods.WindowLongFlags.GWL_STYLE, myStyle);

            //HwndSource.FromHwnd(hWnd).AddHook(new HwndSourceHook(WindowChromeHelper.WndProc));
            IsOpen = true;
        }

        private void PopupWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #region CAPTION_AREA_BUTTON_ACTIONS
        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                SystemCommands.MaximizeWindow(this);

            }
            else if (WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(this);
            }
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        #endregion

        public Thickness MaximizeFix { get; set; } = new Thickness
        (
            SystemParameters.WindowNonClientFrameThickness.Left + SystemParameters.WindowResizeBorderThickness.Left,
            SystemParameters.WindowNonClientFrameThickness.Top + SystemParameters.WindowResizeBorderThickness.Top - SystemParameters.CaptionHeight - SystemParameters.BorderWidth,
            SystemParameters.WindowNonClientFrameThickness.Right + SystemParameters.WindowResizeBorderThickness.Right,
            SystemParameters.WindowNonClientFrameThickness.Bottom + SystemParameters.WindowResizeBorderThickness.Bottom
        );
    }
}