using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace syzygy
{
    public class WindowChromeHelper
    {
        #region DLL Imports
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("shell32", CallingConvention = CallingConvention.StdCall)]
        public static extern int SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport("user32", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);
        #endregion

        #region Parameters
        public struct WindowLongParams
        {
            /// <summary>Sets a new address for the window procedure.</summary>
            /// <remarks>You cannot change this attribute if the window does not belong to the same process as the calling thread.</remarks>
            public static readonly int GWL_WNDPROC = -4;

            /// <summary>Sets a new application instance handle.</summary>
            public static readonly int GWLP_HINSTANCE = -6;

            public static readonly int GWLP_HWNDPARENT = -8;

            /// <summary>Sets a new identifier of the child window.</summary>
            /// <remarks>The window cannot be a top-level window.</remarks>
            public static readonly int GWL_ID = -12;

            /// <summary>Sets a new window style.</summary>
            public static readonly int GWL_STYLE = -16;

            /// <summary>Sets a new extended window style.</summary>
            /// <remarks>See <see cref="ExWindowStyles"/>.</remarks>
            public static readonly int GWL_EXSTYLE = -20;

            /// <summary>Sets the user data associated with the window.</summary>
            /// <remarks>This data is intended for use by the application that created the window. Its value is initially zero.</remarks>
            public static readonly int GWL_USERDATA = -21;

            /// <summary>Sets the return value of a message processed in the dialog box procedure.</summary>
            /// <remarks>Only applies to dialog boxes.</remarks>
            public static readonly int DWLP_MSGRESULT = 0;

            /// <summary>Sets new extra information that is private to the application, such as handles or pointers.</summary>
            /// <remarks>Only applies to dialog boxes.</remarks>
            public static readonly int DWLP_USER = 8;

            /// <summary>Sets the new address of the dialog box procedure.</summary>
            /// <remarks>Only applies to dialog boxes.</remarks>
            public static readonly int DWLP_DLGPROC = 4;
        }

        /// <summary>
        /// Window Styles.
        /// The following styles can be specified wherever a window style is required. After the control has been created, these styles cannot be modified, except as noted.
        /// </summary>
        public struct WindowStyles
        {
            /// <summary>
            /// The window has a thin-line border.
            /// </summary>
            public static readonly long WS_BORDER = 0x00800000L;

            /// <summary>
            /// The window has a title bar (includes the WS_BORDER style).
            /// </summary>
            public static readonly long WS_CAPTION = 0x00C00000L;

            /// <summary>
            /// The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.
            /// </summary>
            public static readonly long WS_CHILD = 0x40000000L;

            /// <summary>
            /// Same as the WS_CHILD style.
            /// </summary>
            public static readonly long WS_CHILDWINDOW = 0x40000000L;

            /// <summary>
            /// Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.
            /// </summary>
            public static readonly long WS_CLIPCHILDREN = 0x02000000L;

            /// <summary>
            /// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated. If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window.
            /// </summary>
            public static readonly long WS_CLIPSIBLINGS = 0x04000000L;

            /// <summary>
            /// The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.
            /// </summary>
            public static readonly long WS_DISABLED = 0x08000000L;

            /// <summary>
            /// The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.
            /// </summary>
            public static readonly long WS_DLGFRAME = 0x00400000L;

            /// <summary>
            /// The window is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style. The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys. You can turn this style on and off to change dialog box navigation.To change this style after a window has been created, use the SetWindowLong function.
            /// </summary>
            public static readonly long WS_GROUP = 0x00020000L;

            /// <summary>
            /// The window has a horizontal scroll bar.
            /// </summary>
            public static readonly long WS_HSCROLL = 0x00100000L;

            /// <summary>
            /// The window is initially minimized. Same as the WS_MINIMIZE style.
            /// </summary>
            public static readonly long WS_ICONIC = 0x20000000L;

            /// <summary>
            /// The window is initially maximized.
            /// </summary>
            public static readonly long WS_MAXIMIZE = 0x01000000L;

            /// <summary>
            /// The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified. 
            /// </summary>
            public static readonly long WS_MAXIMIZEBOX = 0x00010000L;

            /// <summary>
            /// The window is initially minimized. Same as the WS_ICONIC style.
            /// </summary>
            public static readonly long WS_MINIMIZE = 0x20000000L;

            /// <summary>
            /// The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified. 
            /// </summary>
            public static readonly long WS_MINIMIZEBOX = 0x00020000L;

            /// <summary>
            /// The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style.
            /// </summary>
            public static readonly long WS_OVERLAPPED = 0x00000000L;

            /// <summary>
            /// The window is an overlapped window. Same as the WS_TILEDWINDOW style. 
            /// </summary>
            public static readonly long WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;

            /// <summary>
            /// The windows is a pop-up window. This style cannot be used with the WS_CHILD style.
            /// </summary>
            public static readonly long WS_POPUP = 0x80000000L;

            /// <summary>
            /// The window is a pop-up window. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible.
            /// </summary>
            public static readonly long WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU;

            /// <summary>
            /// The window has a sizing border. Same as the WS_THICKFRAME style.
            /// </summary>
            public static readonly long WS_SIZEBOX = 0x00040000L;

            /// <summary>
            /// The window has a window menu on its title bar. The WS_CAPTION style must also be specified.
            /// </summary>
            public static readonly long WS_SYSMENU = 0x00080000L;

            /// <summary>
            /// The window is a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style. You can turn this style on and off to change dialog box navigation.To change this style after a window has been created, use the SetWindowLong function. For user-created windows and modeless dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
            /// </summary>
            public static readonly long WS_TABSTOP = 0x00010000L;

            /// <summary>
            /// The window has a sizing border. Same as the WS_SIZEBOX style.
            /// </summary>
            public static readonly long WS_THICKFRAME = 0x00040000L;

            /// <summary>
            /// The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_OVERLAPPED style. 
            /// </summary>
            public static readonly long WS_TILED = 0x00000000L;

            /// <summary>
            /// The window is an overlapped window. Same as the WS_OVERLAPPEDWINDOW style.
            /// </summary>
            public static readonly long WS_TILEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;

            /// <summary>
            /// The window is initially visible. This style can be turned on and off by using the ShowWindow or SetWindowPos function.
            /// </summary>
            public static readonly long WS_VISIBLE = 0x10000000L;

            /// <summary>
            /// The window has a vertical scroll bar.
            /// </summary>
            public static readonly long WS_VSCROLL = 0x00200000L;
        }

        /// <summary>
        /// Windows Messages
        /// </summary>
        public struct WindowsMessage
        {
            /// <summary>
            /// The WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default maximized size and position, or its default minimum or maximum tracking size.
            /// </summary>
            public const int WM_GETMINMAXINFO = 0x0024;

            /// <summary>
            /// Sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
            /// </summary>
            public const int WM_WINDOWPOSCHANGING = 0x0046;

            /// <summary>
            /// Sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
            /// </summary>
            public const int WM_WINDOWPOSCHANGED = 0x0047;

            /// <summary>
            /// Sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.
            /// </summary>
            public const int WM_NCCALCSIZE = 0x0083;

            /// <summary>
            /// Sent to a window in order to determine what part of the window corresponds to a particular screen coordinate. This can happen, for example, when the cursor moves, when a mouse button is pressed or released, or in response to a call to a function such as WindowFromPoint. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.
            /// </summary>
            public const int WM_NCHITTEST = 0x0084;

            /// <summary>
            /// Sent to a window after its size has changed.
            /// </summary>
            public const int WM_SIZE = 0x0005;

            /// <summary>
            /// Sent one time to a window, after it has exited the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
            /// </summary>
            public const int WM_EXITSIZEMOVE = 0x0232;

            /// <summary>
            /// Sent to a window after the SetWindowLong function has changed one or more of the window's styles.
            /// </summary>
            public const int WM_STYLECHANGED = 0x007D;

            /// <summary>
            /// Sent to a window when the SetWindowLong function is about to change one or more of the window's styles.
            /// </summary>
            public const int WM_STYLECHANGING = 0x007C;

            /// <summary>
            /// Sent when an application changes the enabled state of a window. It is sent to the window whose enabled state is changing. This message is sent before the EnableWindow function returns, but after the enabled state (WS_DISABLED style bit) of the window has changed.
            /// </summary>
            public const int WM_ENABLE = 0x000A;

            /// <summary>
            /// Sent prior to the WM_CREATE message when a window is first created.
            /// </summary>
            public const int WM_NCCREATE = 0x0081;

        }

        /// <summary>
        /// Determines the function's return value if the window does not intersect any display monitor. This parameter can be one of the following values.
        /// </summary>
        public struct MonitorFromWindowParams
        {
            /// <summary>
            /// Returns NULL.
            /// </summary>
            public static readonly int MONITOR_DEFAULTTONULL = 0;
            /// <summary>
            /// Returns a handle to the primary display monitor.
            /// </summary>
            public static readonly int MONITOR_DEFAULTTOPRIMARY = 1;
            /// <summary>
            /// Returns a handle to the display monitor that is nearest to the window.
            /// </summary>
            public static readonly int MONITOR_DEFAULTTONEAREST = 2;
        };


        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public ShowWindowCommands showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rect rcNormalPosition;
        }

        internal enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1,
            Minimized = 2,
            Maximized = 3,
        }

        public enum ABEdge
        {
            ABE_LEFT = 0,
            ABE_TOP = 1,
            ABE_RIGHT = 2,
            ABE_BOTTOM = 3
        }

        public enum ABMsg
        {
            ABM_NEW = 0,
            ABM_REMOVE = 1,
            ABM_QUERYPOS = 2,
            ABM_SETPOS = 3,
            ABM_GETSTATE = 4,
            ABM_GETTASKBARPOS = 5,
            ABM_ACTIVATE = 6,
            ABM_GETAUTOHIDEBAR = 7,
            ABM_SETAUTOHIDEBAR = 8,
            ABM_WINDOWPOSCHANGED = 9,
            ABM_SETSTATE = 10
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public bool lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        #endregion

        #region Methods
        public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        {
                if (IntPtr.Size == 8)
                {
                    return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
                }
                else
                {
                    return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
                }
        }

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        #region Handle Autohide Taskbar
        private static int GetEdge(RECT rc)
        {
            int uEdge = -1;

            if (rc.top == rc.left && rc.bottom > rc.right)
            {
                uEdge = (int)ABEdge.ABE_LEFT;
            }
            else if (rc.top == rc.left && rc.bottom < rc.right)
            {
                uEdge = (int)ABEdge.ABE_TOP;
            }
            else if (rc.top > rc.left)
            {
                uEdge = (int)ABEdge.ABE_BOTTOM;
            }
            else
            {
                uEdge = (int)ABEdge.ABE_RIGHT;
            }
            return uEdge;
        }

        private static MINMAXINFO AdjustWorkingAreaForAutoHide(IntPtr monitorContainingApplication, MINMAXINFO mmi)
        {
            Application.Current.MainWindow.Tag = "noInflate"; // tag to signal to the main window that the border should not be inflated if autohide is enabled
                                                              // because WM_GETMINMAXINFO will handle the sizing and we don't want the inflated border to show here
                                                              // this line can be deleted if not using XAML to handle maximize (if a pure WinAPI solution is found)
            IntPtr hwnd = FindWindow("Shell_TrayWnd", null);

            if (hwnd == null)
            {
                return mmi;
            }

            IntPtr monitorWithTaskbarOnIt = MonitorFromWindow(hwnd, MonitorFromWindowParams.MONITOR_DEFAULTTONEAREST);

            if (!monitorContainingApplication.Equals(monitorWithTaskbarOnIt))
            {
                // probably need to adjust the mmi here if I find a solution for maximizing without autohide taskbar enabled
                return mmi;
            }

            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = hwnd;
            SHAppBarMessage((int)ABMsg.ABM_GETTASKBARPOS, ref abd);
            int uEdge = GetEdge(abd.rc);
            bool autoHide = Convert.ToBoolean(SHAppBarMessage((int)ABMsg.ABM_GETSTATE, ref abd));
            

            if (!autoHide)
            {
                Application.Current.MainWindow.Tag = "Inflate"; // tag to signal to the main window that the border should be inflated since autohide is disabled
                                                                // this line can be deleted if a pure WinAPI solution is found
                return mmi;
            }

            switch (uEdge)
            {
                case (int)ABEdge.ABE_LEFT:
                    mmi.ptMaxPosition.x += 1;
                    mmi.ptMaxTrackSize.x -= 1;
                    mmi.ptMaxSize.x -= 1;
                    break;
                case (int)ABEdge.ABE_RIGHT:
                    mmi.ptMaxSize.x -= 1;
                    mmi.ptMaxTrackSize.x -= 1;
                    break;
                case (int)ABEdge.ABE_TOP:
                    mmi.ptMaxPosition.y += 1;
                    mmi.ptMaxTrackSize.y -= 1;
                    mmi.ptMaxSize.y -= 1;
                    break;
                case (int)ABEdge.ABE_BOTTOM:
                    mmi.ptMaxSize.y -= 1;
                    mmi.ptMaxTrackSize.y -= 1;
                    break;
                default:
                    return mmi;
            }
            return mmi;
        }
        #endregion

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            IntPtr monitorContainingApplication = MonitorFromWindow(hwnd, MonitorFromWindowParams.MONITOR_DEFAULTTONEAREST);

            if (monitorContainingApplication != IntPtr.Zero)
            {
                // Get monitor information
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitorContainingApplication, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
                mmi.ptMaxTrackSize.x = mmi.ptMaxSize.x;                                //maximum drag X size for the window
                mmi.ptMaxTrackSize.y = mmi.ptMaxSize.y;                                //maximum drag Y size for the window
                mmi.ptMinTrackSize.x = 200;                                            //minimum drag X size for the window
                mmi.ptMinTrackSize.y = 40;                                             //minimum drag Y size for the window
                mmi = AdjustWorkingAreaForAutoHide(monitorContainingApplication, mmi); //need to adjust sizing if taskbar is set to autohide
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        private static void WmWindowPosChanging(IntPtr handle, IntPtr lParam)
        {
            // BUGS:
            // 1.when you use aero snap to restore the window and then try to aero snap it back to maximize without releasing the mouse button, resize mode
            //  hasn't changed yet.
            // 2.when the window is maximized, the resize mode hasn't been changed back to can resize, so you can't use WIN + arrows to dock the window left
            //   or right

            //Console.WriteLine("WINDOWPOSCHANGING fired");

            //WINDOWPLACEMENT wndpl = new WINDOWPLACEMENT();
            //wndpl.length = Marshal.SizeOf(wndpl);

            //if (GetWindowPlacement(handle, ref wndpl))
            //{
            //    if (wndpl.showCmd == ShowWindowCommands.Maximized)
            //    {
                    //IntPtr noMaximizeBox = new IntPtr(GetWindowLongPtr(handle, WindowLongParams.GWL_STYLE).ToInt32() & ~WindowStyles.WS_MAXIMIZEBOX);
                    //SetWindowLongPtr(new HandleRef(null, handle), WindowLongParams.GWL_STYLE, noMaximizeBox);
                    //ShowWindow(handle, 5);
                    //Console.WriteLine("WINDOWPOSCHANGING maximized fired");
                //}

                //if (wndpl.showCmd == ShowWindowCommands.Normal)
                //{
                    //IntPtr MaximizeBox = new IntPtr(WindowStyles.WS_CAPTION | WindowStyles.WS_MAXIMIZEBOX | WindowStyles.WS_MINIMIZEBOX | WindowStyles.WS_SYSMENU | WindowStyles.WS_SIZEBOX);
                    //SetWindowLongPtr(new HandleRef(null, handle), WindowLongParams.GWL_STYLE, MaximizeBox);
                    //ShowWindow(handle, 5);
                //}

                //else
                //{
                    //ResizeMode = ResizeMode.CanResize;
                //}

            //}

        }

        #region CUSTOM_TASKBAR_CHECK
        // delete this if it isn't being used
        public static bool AutoHideTaskbarEnabled()
        {
            IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", null);
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = taskbarHandle;
            SHAppBarMessage((int)ABMsg.ABM_GETTASKBARPOS, ref abd);
            bool autoHide = Convert.ToBoolean(SHAppBarMessage((int)ABMsg.ABM_GETSTATE, ref abd));
            return (autoHide ? true : false);
        }
        #endregion

        #region Handle Window Procedure
        public static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WindowsMessage.WM_GETMINMAXINFO:
                {
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
                }
                //case WindowsMessage.WM_WINDOWPOSCHANGING:
                //{
                //    WmWindowPosChanging(hwnd, lParam);
                //    //handled = true; // setting handled = true here causes visual bugs when using WIN KEY + ARROWS to dock the window
                //    break;
                //}
                //case WindowsMessage.WM_SIZE:
                //{
                //        //if(wParam.ToInt32() == 0)
                //        //{
                //        //    //IntPtr MaximizeBox = new IntPtr(GetWindowLongPtr(hwnd, WindowLongParams.GWL_STYLE).ToInt32() & WindowStyles.WS_MAXIMIZEBOX);
                //        //    //SetWindowLongPtr(new HandleRef(null, hwnd), WindowLongParams.GWL_STYLE, MaximizeBox);
                //        //    //ShowWindow(hwnd, 0);
                //        //}

                //        //WINDOWPLACEMENT wndpl = new WINDOWPLACEMENT();
                //        //wndpl.length = Marshal.SizeOf(wndpl);
                //        //GetWindowPlacement(hwnd, ref wndpl);

                //        //if(wndpl.showCmd == ShowWindowCommands.Maximized)
                //        //{

                //        //}

                //        break;
                //}
                //case WindowsMessage.WM_NCCREATE:
                //{
                //    break;
                //}
                //case WindowsMessage.WM_NCCALCSIZE:
                //{
                //    break;
                //    //handled = true;
                //    //return IntPtr.Zero;
                //    // disabling the WindowChrome class and returning IntPtr.Zero here fixes the aero peek bug due to SetWindowRgn, but it also makes the window
                //    // flash white on load...
                //}
                //case WindowsMessage.WM_NCHITTEST:
                //{
                //    //const int GripSize = 16;
                //    //const int BorderSize = 7;
                //    //int x = lParam.ToInt32() << 16 >> 16;
                //    //int y = lParam.ToInt32() >> 16;
                //    //Point pos = PointFromScreen(new Point(x, y));

                //    //if (pos.X > GripSize && pos.X < ActualWidth - GripSize && pos.Y >= ActualHeight - BorderSize)
                //    //{
                //    //    handled = true;
                //    //    return (IntPtr)15; // This doesn't work?
                //    //}
                //    break;
                //}
            }
            return IntPtr.Zero;
        }
        #endregion

        #endregion
    }
}