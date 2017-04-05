namespace nManager.Wow.MemoryClass.Magic
{
    using System;
    using System.Diagnostics;

    public static class SProcess
    {
        public static int[] GetProcessesFromClassname(string Classname)
        {
            IntPtr[] ptrArray = SWindow.FindWindows(Classname, null);
            if ((ptrArray == null) || (ptrArray.Length == 0))
            {
                return null;
            }
            int[] numArray = new int[ptrArray.Length];
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = GetProcessFromWindow(ptrArray[i]);
            }
            return numArray;
        }

        public static int[] GetProcessesFromWindowTitle(string WindowTitle)
        {
            IntPtr[] ptrArray = SWindow.FindWindows(null, WindowTitle);
            if ((ptrArray == null) || (ptrArray.Length == 0))
            {
                return null;
            }
            int[] numArray = new int[ptrArray.Length];
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = GetProcessFromWindow(ptrArray[i]);
            }
            return numArray;
        }

        public static int GetProcessFromClassname(string Classname)
        {
            IntPtr hWnd = SWindow.FindWindow(Classname, null);
            if (hWnd == IntPtr.Zero)
            {
                return 0;
            }
            return GetProcessFromWindow(hWnd);
        }

        public static int GetProcessFromProcessName(string ProcessName)
        {
            if (ProcessName.EndsWith(".exe"))
            {
                ProcessName = ProcessName.Remove(ProcessName.Length - 4, 4);
            }
            Process[] processesByName = Process.GetProcessesByName(ProcessName);
            if ((processesByName != null) && (processesByName.Length != 0))
            {
                return processesByName[0].Id;
            }
            return 0;
        }

        public static int GetProcessFromWindow(IntPtr hWnd)
        {
            int dwProcessId = 0;
            Imports.GetWindowThreadProcessId(hWnd, out dwProcessId);
            return dwProcessId;
        }

        public static int GetProcessFromWindowTitle(string WindowTitle)
        {
            IntPtr hWnd = SWindow.FindWindow(null, WindowTitle);
            if (hWnd == IntPtr.Zero)
            {
                return 0;
            }
            return GetProcessFromWindow(hWnd);
        }

        public static IntPtr OpenProcess(int dwProcessId)
        {
            return OpenProcess(dwProcessId, 0x1f0fff);
        }

        public static IntPtr OpenProcess(IntPtr hWnd)
        {
            return OpenProcess(GetProcessFromWindow(hWnd), 0x1f0fff);
        }

        public static IntPtr OpenProcess(int dwProcessId, uint dwAccessRights)
        {
            return Imports.OpenProcess(dwAccessRights, false, dwProcessId);
        }

        public static IntPtr OpenProcess(IntPtr hWnd, uint dwAccessRights)
        {
            return OpenProcess(GetProcessFromWindow(hWnd), dwAccessRights);
        }
    }
}

