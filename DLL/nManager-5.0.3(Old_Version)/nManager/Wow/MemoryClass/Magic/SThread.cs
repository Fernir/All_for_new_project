namespace nManager.Wow.MemoryClass.Magic
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public static class SThread
    {
        public static IntPtr CreateRemoteThread(IntPtr hProcess, uint dwStartAddress, uint dwParameter)
        {
            uint num;
            return CreateRemoteThread(hProcess, dwStartAddress, dwParameter, 0, out num);
        }

        public static IntPtr CreateRemoteThread(IntPtr hProcess, uint dwStartAddress, uint dwParameter, out uint dwThreadId)
        {
            return CreateRemoteThread(hProcess, dwStartAddress, dwParameter, 0, out dwThreadId);
        }

        public static IntPtr CreateRemoteThread(IntPtr hProcess, uint dwStartAddress, uint dwParameter, uint dwCreationFlags, out uint dwThreadId)
        {
            IntPtr ptr2;
            IntPtr ptr = Imports.CreateRemoteThread(hProcess, IntPtr.Zero, 0, (IntPtr) dwStartAddress, (IntPtr) dwParameter, dwCreationFlags, out ptr2);
            dwThreadId = (uint) ((int) ptr2);
            return ptr;
        }

        public static uint GetExitCodeThread(IntPtr hThread)
        {
            UIntPtr ptr;
            if (!Imports.GetExitCodeThread(hThread, out ptr))
            {
                throw new Exception("GetExitCodeThread failed.");
            }
            return (uint) ptr;
        }

        public static ProcessThread GetMainThread(int dwProcessId)
        {
            if (dwProcessId == 0)
            {
                return null;
            }
            return Process.GetProcessById(dwProcessId).Threads[0];
        }

        public static ProcessThread GetMainThread(IntPtr hWindowHandle)
        {
            if (hWindowHandle == IntPtr.Zero)
            {
                return null;
            }
            return GetMainThread(SProcess.GetProcessFromWindow(hWindowHandle));
        }

        public static int GetMainThreadId(int dwProcessId)
        {
            if (dwProcessId == 0)
            {
                return 0;
            }
            return Process.GetProcessById(dwProcessId).Threads[0].Id;
        }

        public static int GetMainThreadId(IntPtr hWindowHandle)
        {
            if (hWindowHandle == IntPtr.Zero)
            {
                return 0;
            }
            return GetMainThreadId(SProcess.GetProcessFromWindow(hWindowHandle));
        }

        public static CONTEXT GetThreadContext(IntPtr hThread, uint ContextFlags)
        {
            CONTEXT lpContext = new CONTEXT {
                ContextFlags = ContextFlags
            };
            if (!Imports.GetThreadContext(hThread, ref lpContext))
            {
                lpContext.ContextFlags = 0;
            }
            return lpContext;
        }

        public static IntPtr OpenThread(int dwThreadId)
        {
            return Imports.OpenThread(0x1f03ff, false, (uint) dwThreadId);
        }

        public static IntPtr OpenThread(uint dwDesiredAccess, int dwThreadId)
        {
            return Imports.OpenThread(dwDesiredAccess, false, (uint) dwThreadId);
        }

        public static uint ResumeThread(IntPtr hThread)
        {
            return Imports.ResumeThread(hThread);
        }

        public static bool SetThreadContext(IntPtr hThread, CONTEXT ctx)
        {
            return Imports.SetThreadContext(hThread, ref ctx);
        }

        public static uint SuspendThread(IntPtr hThread)
        {
            return Imports.SuspendThread(hThread);
        }

        public static uint TerminateThread(IntPtr hThread, uint dwExitCode)
        {
            return Imports.TerminateThread(hThread, dwExitCode);
        }

        public static uint WaitForSingleObject(IntPtr hObject)
        {
            return Imports.WaitForSingleObject(hObject, uint.MaxValue);
        }

        public static uint WaitForSingleObject(IntPtr hObject, uint dwMilliseconds)
        {
            return Imports.WaitForSingleObject(hObject, dwMilliseconds);
        }
    }
}

