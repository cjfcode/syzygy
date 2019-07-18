using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Syzygy
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public static bool IsOpen { get; private set; }

        public AboutWindow()
        {
            InitializeComponent();
        }

        private void AboutWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hWnd = new WindowInteropHelper(this).Handle;

            IntPtr myStyle = new IntPtr((int)NativeMethods.WindowStyles.WS_CAPTION | (int)NativeMethods.WindowStyles.WS_MAXIMIZEBOX |
                                        (int)NativeMethods.WindowStyles.WS_MINIMIZEBOX | (int)NativeMethods.WindowStyles.WS_SYSMENU |
                                        (int)NativeMethods.WindowStyles.WS_SIZEBOX);

            NativeMethods.SetWindowLongPtr(new HandleRef(null, hWnd), NativeMethods.WindowLongFlags.GWL_STYLE, myStyle);

            //HwndSource.FromHwnd(hWnd).AddHook(new HwndSourceHook(WindowChromeHelper.WndProc));
            IsOpen = true;
        }

        private void AboutWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }
    }
}