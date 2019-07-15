using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace Syzygy
{
    public static class NativeMethods
    {
        #region METHODS
        [DllImport("user32", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32")]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, WindowLongFlags nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, WindowLongFlags nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("User32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32")]
        public static extern IntPtr MonitorFromWindow(IntPtr handle, MonitorFromWindowParams flags);

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong32(HandleRef hWnd, WindowLongFlags nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, WindowLongFlags nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("shell32", CallingConvention = CallingConvention.StdCall)]
        public static extern int SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, nCmdShow nCmdShow);

        private static MINMAXINFO AdjustWorkingAreaForAutoHide(IntPtr monitorContainingApplication, MINMAXINFO mmi)
        {
            SetTagState(TagState.noInflate); // tag to signal to the main window that the border should not be inflated since autohide is enabled
                                             // and so GETMINMAXINFO will take care of the sizing

            IntPtr hwnd = FindWindow("Shell_TrayWnd", null);

            if (hwnd == null)
            {
                return mmi;
            }

            IntPtr monitorWithTaskbarOnIt = MonitorFromWindow(hwnd, MonitorFromWindowParams.MONITOR_DEFAULTTONEAREST);

            if (!monitorContainingApplication.Equals(monitorWithTaskbarOnIt))
            {
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
                SetTagState(TagState.Inflate); // tag to signal to the main window that the border should be inflated since autohide is disabled
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

        public static IntPtr SetWindowLongPtr(HandleRef hWnd, WindowLongFlags nIndex, IntPtr dwNewLong)
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

        /// <summary>
        /// The tag state of the window. Used in combination with the SetTagState function to control the state of the window's tag.
        /// </summary>
        private enum TagState
        {
            Inflate,
            noInflate
        }

        /// <summary>
        /// Sets the window's tag state so that MINMAXINFO can know whether or not to inflate the margin when applying autohide taskbar fix.
        /// </summary>
        /// <param name="tagState"></param>
        private static void SetTagState(TagState tagState)
        {
            foreach (Window window in Application.Current.Windows)
            {
                window.Tag = tagState;
            }
        }

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


        public static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WindowsMessage.WM_INSTANCEATTEMPT)
            {
                // Iconic mode means the application is minimized
                if (IsIconic(hwnd))
                {
                    ShowWindow(hwnd, nCmdShow.SW_RESTORE);
                }

                // Simulate a key press, this fixes the issue where the SetForegroundWindow API function is unreliable on Windows 10
                keybd_event(0, 0, 0, 0);

                // If the process is already running, activate the window
                SetForegroundWindow(hwnd);
                handled = true;
            }
            else if (msg == WindowsMessage.WM_GETMINMAXINFO)
            {
                WmGetMinMaxInfo(hwnd, lParam);
                handled = true;
            }
            return IntPtr.Zero;
        }
        #endregion

        #region DATA
        public const int HWND_BROADCAST = 0xffff;

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
        }

        public enum MonitorFromWindowParams
        {
            MONITOR_DEFAULTTONULL = 0,
            MONITOR_DEFAULTTOPRIMARY = 1,
            MONITOR_DEFAULTTONEAREST = 2
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        public enum nCmdShow
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11
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

        public enum WindowLongFlags
        {
            GWL_EXSTYLE = -20,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWLP_USER = 0x8,
            DWLP_MSGRESULT = 0x0,
            DWLP_DLGPROC = 0x4
        }

        public readonly struct WindowsMessage
        {
            public static int WM_CREATE = 0x0001;
            public static int WM_GETMINMAXINFO = 0x0024;
            public static int WM_NCCALCSIZE = 0x0083;
            public static int WM_NCCREATE = 0x0081;
            public static int WM_NCHITTEST = 0x0084;
            public static int WM_WINDOWPOSCHANGING = 0x0046;
            public static int WM_WINDOWPOSCHANGED = 0x0047;
            public static int WM_INSTANCEATTEMPT = RegisterWindowMessage(string.Format("WM_INSTANCEATTEMPT|{0}", ((GuidAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(GuidAttribute), false)[0]).Value));
            public static int WM_SIZE = 0x0005;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public nCmdShow showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rect rcNormalPosition;
        }

        public enum WindowStyles
        {
            WS_OVERLAPPED = 0x00000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_CAPTION = WS_BORDER | WS_DLGFRAME,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_CHILDWINDOW = WS_CHILD,
        }
        #endregion
    }
}