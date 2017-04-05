namespace nManager.Wow.MemoryClass.Magic
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class Imports
    {
        [DllImport("user32", EntryPoint="GetClassName")]
        private static extern int _GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32", EntryPoint="GetWindowText")]
        private static extern int _GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("kernel32")]
        public static extern bool CloseHandle(IntPtr hObject);
        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr dwThreadId);
        [DllImport("user32")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        public static string GetClassName(IntPtr hWnd)
        {
            return GetClassName(hWnd, 0x100);
        }

        public static string GetClassName(IntPtr hWnd, int nMaxCount)
        {
            StringBuilder lpString = new StringBuilder(nMaxCount);
            int length = _GetClassName(hWnd, lpString, nMaxCount);
            if (length > 0)
            {
                return lpString.ToString(0, length);
            }
            return null;
        }

        [DllImport("kernel32")]
        public static extern bool GetExitCodeThread(IntPtr hThread, out UIntPtr lpExitCode);
        [DllImport("kernel32", EntryPoint="GetModuleHandleW")]
        public static extern UIntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);
        [DllImport("kernel32")]
        public static extern UIntPtr GetProcAddress(UIntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);
        [DllImport("kernel32")]
        public static extern bool GetThreadContext(IntPtr hThread, ref CONTEXT lpContext);
        [DllImport("user32")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int dwProcessId);
        public static string GetWindowTitle(IntPtr hWnd)
        {
            return GetWindowTitle(hWnd, 0x100);
        }

        public static string GetWindowTitle(IntPtr hWnd, int nMaxCount)
        {
            StringBuilder lpString = new StringBuilder(nMaxCount);
            int length = _GetWindowText(hWnd, lpString, nMaxCount);
            if (length > 0)
            {
                return lpString.ToString(0, length);
            }
            return null;
        }

        [DllImport("user32")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("kernel32")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32")]
        public static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, uint dwAddress, IntPtr lpBuffer, int nSize, out int lpBytesRead);
        [DllImport("kernel32")]
        public static extern uint ResumeThread(IntPtr hThread);
        [DllImport("kernel32")]
        public static extern bool SetThreadContext(IntPtr hThread, ref CONTEXT lpContext);
        [DllImport("kernel32")]
        public static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32")]
        public static extern uint TerminateThread(IntPtr hThread, uint dwExitCode);
        [DllImport("kernel32")]
        public static extern uint VirtualAllocEx(IntPtr hProcess, uint dwAddress, int nSize, uint dwAllocationType, uint dwProtect);
        [DllImport("kernel32")]
        public static extern bool VirtualFreeEx(IntPtr hProcess, uint dwAddress, int nSize, uint dwFreeType);
        [DllImport("kernel32")]
        public static extern uint WaitForSingleObject(IntPtr hObject, uint dwMilliseconds);
        [DllImport("kernel32")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, uint dwAddress, IntPtr lpBuffer, int nSize, out IntPtr iBytesWritten);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    }
}

