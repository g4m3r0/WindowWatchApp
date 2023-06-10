using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WindowWatchApp.Common;

namespace WindowWatchApp.Common.Windows
{
    public class WindowsActivityTracker : IActivityTracker
    {
        // DLL Imports
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public DateTime LastActivity => DateTime.Now - this.InactivityPeriod;

        public TimeSpan InactivityPeriod
        {
            get
            {
                GetLastInputInfo(ref _lastInputInfo);
                uint elapsedMilliseconds = (uint)Environment.TickCount - _lastInputInfo.DwTime;
                elapsedMilliseconds = Math.Min(elapsedMilliseconds, int.MaxValue);
                return TimeSpan.FromMilliseconds(elapsedMilliseconds);
            }
        }

        struct LastInputInfo
        {
            public int CbSize;
            public uint DwTime;
        }

        [DllImport("user32.dll")]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetLastInputInfo(ref LastInputInfo plii);

        LastInputInfo _lastInputInfo;

        public WindowsActivityTracker()
        {
            _lastInputInfo.CbSize = Marshal.SizeOf(_lastInputInfo);
        }


        public string GetActiveApplication()
        {
            IntPtr hwnd = GetForegroundWindow();

            if (hwnd == IntPtr.Zero)
            {
                return null;
            }

            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);

            var proc = Process.GetProcessById((int)pid);
            return proc.ProcessName;
        }
    }
}
