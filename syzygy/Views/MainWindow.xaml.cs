﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Syzygy.Properties;

namespace Syzygy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hWnd = new WindowInteropHelper(this).Handle;

            IntPtr myStyle = new IntPtr((int)NativeMethods.WindowStyles.WS_CAPTION | (int)NativeMethods.WindowStyles.WS_MAXIMIZEBOX |
                                        (int)NativeMethods.WindowStyles.WS_MINIMIZEBOX | (int)NativeMethods.WindowStyles.WS_SYSMENU |
                                        (int)NativeMethods.WindowStyles.WS_SIZEBOX);

            NativeMethods.SetWindowLongPtr(new HandleRef(null, hWnd), NativeMethods.WindowLongFlags.GWL_STYLE, myStyle);

            HwndSource.FromHwnd(hWnd).AddHook(new HwndSourceHook(NativeMethods.WndProc));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.SetPlacement(Settings.Default.MainWindowPlacement);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Settings.Default.MainWindowPlacement = this.GetPlacement();
            Settings.Default.Save();
        }
    }
}