namespace nManager.Wow.MemoryClass
{
    using nManager.Wow;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Usefuls
    {
        public static PatternResult FindPattern(byte[] pattern, string mask)
        {
            uint num = 0xfffffff;
            uint lpAddress = 0;
            do
            {
                MEMORY_BASIC_INFORMATION lpBuffer = new MEMORY_BASIC_INFORMATION();
                VirtualQueryEx(Memory.WowMemory.Memory.ProcessHandle, lpAddress, out lpBuffer, Marshal.SizeOf(lpBuffer));
                try
                {
                    if ((lpBuffer.AllocationBase != 0) && (lpBuffer.RegionSize > 0x1000))
                    {
                        uint num3 = Memory.WowMemory.Memory.FindPattern((uint) lpBuffer.AllocationBase, lpBuffer.RegionSize, pattern, mask);
                        if (num3 != lpBuffer.AllocationBase)
                        {
                            return new PatternResult { AllocationBase = (uint) lpBuffer.AllocationBase, dwAddress = num3, RegionSize = (uint) lpBuffer.RegionSize };
                        }
                    }
                }
                catch
                {
                }
                lpAddress = (uint) (lpBuffer.BaseAddress + lpBuffer.RegionSize);
                Thread.Sleep(3);
            }
            while (lpAddress <= num);
            return null;
        }

        [DllImport("kernel32", CharSet=CharSet.Ansi)]
        public static extern int GetProcAddress(int hwnd, string procedureName);
        [DllImport("kernel32")]
        public static extern int LoadLibrary(string librayName);
        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, uint lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int Type;
        }

        public class PatternResult
        {
            public uint AllocationBase;
            public uint dwAddress;
            public uint RegionSize;
        }
    }
}

