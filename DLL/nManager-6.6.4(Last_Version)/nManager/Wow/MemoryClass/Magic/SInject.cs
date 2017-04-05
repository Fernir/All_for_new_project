namespace nManager.Wow.MemoryClass.Magic
{
    using Fasm;
    using nManager.Helpful;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;

    public static class SInject
    {
        private const uint _isiajuamuofonTuhiojai = 0;

        public static uint InjectCode(IntPtr hProcess, string szAssembly)
        {
            uint dwAddress = SMemory.AllocateMemory(hProcess);
            if (!InjectCode(hProcess, dwAddress, szAssembly))
            {
                return 0;
            }
            return dwAddress;
        }

        public static uint InjectCode(IntPtr hProcess, string szFormatString, params object[] args)
        {
            return InjectCode(hProcess, string.Format(szFormatString, args));
        }

        public static bool InjectCode(IntPtr hProcess, uint dwAddress, string szAssembly)
        {
            byte[] buffer;
            if (((hProcess == IntPtr.Zero) || (szAssembly.Length == 0)) || (dwAddress == 0))
            {
                return false;
            }
            try
            {
                buffer = ManagedFasm.Assemble(szAssembly);
            }
            catch (Exception exception)
            {
                Logging.WriteError(exception.Message, true);
                return false;
            }
            return SMemory.WriteBytes(hProcess, dwAddress, buffer, buffer.Length);
        }

        public static bool InjectCode(IntPtr hProcess, uint dwAddress, string szFormat, params object[] args)
        {
            return InjectCode(hProcess, dwAddress, string.Format(szFormat, args));
        }

        public static uint InjectDllCreateThread(IntPtr hProcess, string szDllPath)
        {
            if (hProcess == IntPtr.Zero)
            {
                throw new ArgumentNullException("hProcess");
            }
            if (szDllPath.Length == 0)
            {
                throw new ArgumentNullException("szDllPath");
            }
            if (!szDllPath.Contains(@"\"))
            {
                szDllPath = Path.GetFullPath(szDllPath);
            }
            if (!File.Exists(szDllPath))
            {
                throw new ArgumentException("DLL not found.", "szDllPath");
            }
            uint exitCodeThread = 0;
            uint procAddress = (uint) Imports.GetProcAddress(Imports.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            if (procAddress > 0)
            {
                uint dwAddress = SMemory.AllocateMemory(hProcess);
                if (dwAddress <= 0)
                {
                    return exitCodeThread;
                }
                if (SMemory.WriteASCIIString(hProcess, dwAddress, szDllPath))
                {
                    IntPtr hObject = SThread.CreateRemoteThread(hProcess, procAddress, dwAddress);
                    if (SThread.WaitForSingleObject(hObject, 0x1388) == 0)
                    {
                        exitCodeThread = SThread.GetExitCodeThread(hObject);
                    }
                    Imports.CloseHandle(hObject);
                }
                SMemory.FreeMemory(hProcess, dwAddress);
            }
            return exitCodeThread;
        }

        public static uint InjectDllRedirectThread(IntPtr hProcess, int dwProcessId, string szDllPath)
        {
            IntPtr hThread = SThread.OpenThread(SThread.GetMainThreadId(dwProcessId));
            if (hThread == IntPtr.Zero)
            {
                return 0;
            }
            uint num = InjectDllRedirectThread(hProcess, hThread, szDllPath);
            Imports.CloseHandle(hThread);
            return num;
        }

        public static uint InjectDllRedirectThread(IntPtr hProcess, IntPtr hThread, string szDllPath)
        {
            if (hProcess == IntPtr.Zero)
            {
                throw new ArgumentNullException("hProcess");
            }
            if (hThread == IntPtr.Zero)
            {
                throw new ArgumentNullException("hThread");
            }
            if (szDllPath.Length == 0)
            {
                throw new ArgumentNullException("szDllPath");
            }
            if (!szDllPath.Contains(@"\"))
            {
                szDllPath = Path.GetFullPath(szDllPath);
            }
            if (!File.Exists(szDllPath))
            {
                throw new ArgumentException("DLL not found.", "szDllPath");
            }
            uint num = 0;
            new StringBuilder();
            ManagedFasm fasm = new ManagedFasm(hProcess);
            uint procAddress = (uint) Imports.GetProcAddress(Imports.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            if (procAddress == 0)
            {
                return 0;
            }
            uint dwAddress = SMemory.AllocateMemory(hProcess);
            if (dwAddress == 0)
            {
                return 0;
            }
            if (SThread.SuspendThread(hThread) != uint.MaxValue)
            {
                CONTEXT threadContext = SThread.GetThreadContext(hThread, 0x10001);
                if (threadContext.Eip > 0)
                {
                    try
                    {
                        fasm.AddLine("lpExitCode dd 0x{0:X}", new object[] { uint.MaxValue });
                        fasm.AddLine("push 0x{0:X}", new object[] { threadContext.Eip });
                        fasm.AddLine("pushad");
                        fasm.AddLine("push szDllPath");
                        fasm.AddLine("call 0x{0:X}", new object[] { procAddress });
                        fasm.AddLine("mov [lpExitCode], eax");
                        fasm.AddLine("popad");
                        fasm.AddLine("retn");
                        fasm.AddLine("szDllPath db '{0}',0", new object[] { szDllPath });
                        fasm.Inject(dwAddress);
                    }
                    catch
                    {
                        SMemory.FreeMemory(hProcess, dwAddress);
                        SThread.ResumeThread(hThread);
                        return 0;
                    }
                    threadContext.ContextFlags = 0x10001;
                    threadContext.Eip = dwAddress + 4;
                    if (SThread.SetThreadContext(hThread, threadContext) && (SThread.ResumeThread(hThread) != uint.MaxValue))
                    {
                        for (int i = 0; i < 400; i++)
                        {
                            Thread.Sleep(5);
                            if ((num = SMemory.ReadUInt(hProcess, dwAddress)) != uint.MaxValue)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            if (fasm != null)
            {
                fasm.Dispose();
                fasm = null;
            }
            SMemory.FreeMemory(hProcess, dwAddress);
            return num;
        }
    }
}

