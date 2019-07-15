﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using syzygy.Properties;

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

        ///// <summary>
        ///// Thickness used to construct a border that will make the window fit perfectly within the screen when the window is maximized
        ///// </summary>
        public Thickness MaximizeFix { get; private set; } = new Thickness
        (
            SystemParameters.WindowNonClientFrameThickness.Left + SystemParameters.WindowResizeBorderThickness.Left,
            SystemParameters.WindowNonClientFrameThickness.Top + SystemParameters.WindowResizeBorderThickness.Top - SystemParameters.CaptionHeight - SystemParameters.BorderWidth,
            SystemParameters.WindowNonClientFrameThickness.Right + SystemParameters.WindowResizeBorderThickness.Right,
            SystemParameters.WindowNonClientFrameThickness.Bottom + SystemParameters.WindowResizeBorderThickness.Bottom
        );

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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Settings.Default.MainWindowPlacement = this.GetPlacement();
            Settings.Default.Save();
        }
    }
}