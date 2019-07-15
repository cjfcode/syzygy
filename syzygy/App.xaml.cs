using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace Syzygy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string UniqueMutexName = string.Format("Global\\{{{0}}}", ((GuidAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(GuidAttribute), false)[0]).Value);
        private static Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            bool singleInstance;
            mutex = new Mutex(true, UniqueMutexName, out singleInstance);

            if (!singleInstance)
            {
                NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST, NativeMethods.WindowsMessage.WM_INSTANCEATTEMPT, IntPtr.Zero, IntPtr.Zero);
                Current.Shutdown();
            }
            base.OnStartup(e);
        }
    }
}