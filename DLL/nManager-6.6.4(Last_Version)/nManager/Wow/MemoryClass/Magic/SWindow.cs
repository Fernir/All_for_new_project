namespace nManager.Wow.MemoryClass.Magic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class SWindow
    {
        private static List<IntPtr> _eluorufegaef;
        private static object _jubuopalouciujFiaca = new object();

        public static IntPtr[] EnumMainWindows()
        {
            List<IntPtr> list = new List<IntPtr>();
            foreach (Process process in Process.GetProcesses())
            {
                list.Add(process.MainWindowHandle);
            }
            return list.ToArray();
        }

        public static IntPtr[] EnumWindows()
        {
            lock (_jubuopalouciujFiaca)
            {
                if (!UvuocaexUhai())
                {
                    return null;
                }
                return _eluorufegaef.ToArray();
            }
        }

        public static IntPtr FindMainWindow(string Classname, string WindowTitle)
        {
            if (Classname == null)
            {
                Classname = string.Empty;
            }
            if (WindowTitle == null)
            {
                WindowTitle = string.Empty;
            }
            foreach (Process process in Process.GetProcesses())
            {
                if ((process.MainWindowHandle != IntPtr.Zero) && (((WindowTitle.Length > 0) && (process.MainWindowTitle == WindowTitle)) || ((Classname.Length > 0) && (Imports.GetClassName(process.MainWindowHandle) == Classname))))
                {
                    return process.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }

        public static IntPtr FindMainWindowContains(string Classname, string WindowTitle)
        {
            if (Classname == null)
            {
                Classname = string.Empty;
            }
            if (WindowTitle == null)
            {
                WindowTitle = string.Empty;
            }
            foreach (Process process in Process.GetProcesses())
            {
                if ((process.MainWindowHandle != IntPtr.Zero) && (((WindowTitle.Length > 0) && process.MainWindowTitle.Contains(WindowTitle)) || ((Classname.Length > 0) && Imports.GetClassName(process.MainWindowHandle).Contains(Classname))))
                {
                    return process.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }

        public static IntPtr[] FindMainWindows(string Classname, string WindowTitle)
        {
            if (Classname == null)
            {
                Classname = string.Empty;
            }
            if (WindowTitle == null)
            {
                WindowTitle = string.Empty;
            }
            List<IntPtr> list = new List<IntPtr>();
            foreach (Process process in Process.GetProcesses())
            {
                if ((process.MainWindowHandle != IntPtr.Zero) && (((WindowTitle.Length > 0) && (process.MainWindowTitle == WindowTitle)) || ((Classname.Length > 0) && (Imports.GetClassName(process.MainWindowHandle) == Classname))))
                {
                    list.Add(process.MainWindowHandle);
                }
            }
            return list.ToArray();
        }

        public static IntPtr[] FindMainWindowsContains(string Classname, string WindowTitle)
        {
            if (Classname == null)
            {
                Classname = string.Empty;
            }
            if (WindowTitle == null)
            {
                WindowTitle = string.Empty;
            }
            List<IntPtr> list = new List<IntPtr>();
            foreach (Process process in Process.GetProcesses())
            {
                if ((process.MainWindowHandle != IntPtr.Zero) && (((WindowTitle.Length > 0) && process.MainWindowTitle.Contains(WindowTitle)) || ((Classname.Length > 0) && Imports.GetClassName(process.MainWindowHandle).Contains(Classname))))
                {
                    list.Add(process.MainWindowHandle);
                }
            }
            return list.ToArray();
        }

        public static IntPtr FindWindow(string Classname, string WindowTitle)
        {
            if (Classname == null)
            {
                Classname = string.Empty;
            }
            if (WindowTitle == null)
            {
                WindowTitle = string.Empty;
            }
            lock (_jubuopalouciujFiaca)
            {
                if (!UvuocaexUhai())
                {
                    return IntPtr.Zero;
                }
                foreach (IntPtr ptr in _eluorufegaef)
                {
                    if ((WindowTitle.Length > 0) && (Imports.GetWindowTitle(ptr) == WindowTitle))
                    {
                        return ptr;
                    }
                    if ((Classname.Length > 0) && (Imports.GetClassName(ptr) == Classname))
                    {
                        return ptr;
                    }
                }
            }
            return IntPtr.Zero;
        }

        public static IntPtr FindWindowByProcessId(int dwProcessId)
        {
            return Process.GetProcessById(dwProcessId).MainWindowHandle;
        }

        public static IntPtr FindWindowByProcessName(string ProcessName)
        {
            if (ProcessName.EndsWith(".exe"))
            {
                ProcessName = ProcessName.Remove(ProcessName.Length - 4, 4);
            }
            Process[] processesByName = Process.GetProcessesByName(ProcessName);
            if ((processesByName != null) && (processesByName.Length != 0))
            {
                return processesByName[0].MainWindowHandle;
            }
            return IntPtr.Zero;
        }

        public static IntPtr FindWindowContains(string Classname, string WindowTitle)
        {
            if (Classname == null)
            {
                Classname = string.Empty;
            }
            if (WindowTitle == null)
            {
                WindowTitle = string.Empty;
            }
            lock (_jubuopalouciujFiaca)
            {
                if (!UvuocaexUhai())
                {
                    return IntPtr.Zero;
                }
                foreach (IntPtr ptr in _eluorufegaef)
                {
                    if ((WindowTitle.Length > 0) && Imports.GetWindowTitle(ptr).Contains(WindowTitle))
                    {
                        return ptr;
                    }
                    if ((Classname.Length > 0) && Imports.GetClassName(ptr).Contains(Classname))
                    {
                        return ptr;
                    }
                }
            }
            return IntPtr.Zero;
        }

        public static IntPtr[] FindWindows(string Classname, string WindowTitle)
        {
            if (Classname == null)
            {
                Classname = string.Empty;
            }
            if (WindowTitle == null)
            {
                WindowTitle = string.Empty;
            }
            List<IntPtr> list = new List<IntPtr>();
            lock (_jubuopalouciujFiaca)
            {
                if (!UvuocaexUhai())
                {
                    return null;
                }
                foreach (IntPtr ptr in _eluorufegaef)
                {
                    if (((WindowTitle.Length > 0) && (Imports.GetWindowTitle(ptr) == WindowTitle)) || ((Classname.Length > 0) && (Imports.GetClassName(ptr) == Classname)))
                    {
                        list.Add(ptr);
                    }
                }
            }
            return list.ToArray();
        }

        public static IntPtr[] FindWindowsByProcessName(string ProcessName)
        {
            List<IntPtr> list = new List<IntPtr>();
            if (ProcessName.EndsWith(".exe"))
            {
                ProcessName = ProcessName.Remove(ProcessName.Length - 4, 4);
            }
            Process[] processesByName = Process.GetProcessesByName(ProcessName);
            if ((processesByName == null) || (processesByName.Length == 0))
            {
                return null;
            }
            foreach (Process process in processesByName)
            {
                list.Add(process.MainWindowHandle);
            }
            return list.ToArray();
        }

        public static IntPtr[] FindWindowsContains(string Classname, string WindowTitle)
        {
            if (Classname == null)
            {
                Classname = string.Empty;
            }
            if (WindowTitle == null)
            {
                WindowTitle = string.Empty;
            }
            List<IntPtr> list = new List<IntPtr>();
            lock (_jubuopalouciujFiaca)
            {
                if (!UvuocaexUhai())
                {
                    return null;
                }
                foreach (IntPtr ptr in _eluorufegaef)
                {
                    if (((WindowTitle.Length > 0) && Imports.GetWindowTitle(ptr).Contains(WindowTitle)) || ((Classname.Length > 0) && Imports.GetClassName(ptr).Contains(Classname)))
                    {
                        list.Add(ptr);
                    }
                }
            }
            return list.ToArray();
        }

        private static bool UvuocaexUhai()
        {
            _eluorufegaef = new List<IntPtr>();
            Imports.EnumWindowsProc lpEnumFunc = new Imports.EnumWindowsProc(SWindow.Woawef);
            return Imports.EnumWindows(lpEnumFunc, IntPtr.Zero);
        }

        private static bool Woawef(IntPtr afejiebou, IntPtr akaravuOsa)
        {
            _eluorufegaef.Add(afejiebou);
            return true;
        }
    }
}

