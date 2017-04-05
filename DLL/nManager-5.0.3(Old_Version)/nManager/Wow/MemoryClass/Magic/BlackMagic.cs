namespace nManager.Wow.MemoryClass.Magic
{
    using Fasm;
    using nManager.Wow.Class;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class BlackMagic
    {
        private bool m_bProcessOpen;
        private bool m_bThreadOpen;
        private List<PatternDataEntry> m_Data;
        private IntPtr m_hProcess;
        private IntPtr m_hThread;
        private IntPtr m_hWnd;
        private ProcessModule m_MainModule;
        private ProcessModuleCollection m_Modules;
        private int m_ProcessId;
        private int m_ThreadId;
        private const uint RETURN_ERROR = 0;
        public bool SetDebugPrivileges;

        public BlackMagic()
        {
            this.SetDebugPrivileges = true;
            this.m_hProcess = IntPtr.Zero;
            this.m_hWnd = IntPtr.Zero;
            this.m_hThread = IntPtr.Zero;
            this.Asm = new ManagedFasm();
            this.m_Data = new List<PatternDataEntry>();
            if (this.m_bProcessOpen && (this.m_hProcess != IntPtr.Zero))
            {
                this.Asm.SetProcessHandle(this.m_hProcess);
            }
        }

        public BlackMagic(int ProcessId) : this()
        {
            this.m_bProcessOpen = this.Open(ProcessId);
        }

        public BlackMagic(IntPtr WindowHandle) : this(SProcess.GetProcessFromWindow(WindowHandle))
        {
        }

        public uint AllocateMemory()
        {
            return this.AllocateMemory(0x1000);
        }

        public uint AllocateMemory(int nSize)
        {
            return this.AllocateMemory(nSize, 0x1000, 0x40);
        }

        public uint AllocateMemory(int nSize, uint dwAllocationType, uint dwProtect)
        {
            return SMemory.AllocateMemory(this.m_hProcess, nSize, dwAllocationType, dwProtect);
        }

        public void Close()
        {
            this.Asm.Dispose();
            this.CloseProcess();
            this.CloseThread();
        }

        public void CloseProcess()
        {
            if (this.m_hProcess != IntPtr.Zero)
            {
                Imports.CloseHandle(this.m_hProcess);
            }
            this.m_hProcess = IntPtr.Zero;
            this.m_hWnd = IntPtr.Zero;
            this.m_ProcessId = 0;
            this.m_MainModule = null;
            this.m_Modules = null;
            this.m_bProcessOpen = false;
            this.Asm.SetProcessHandle(IntPtr.Zero);
        }

        public void CloseThread()
        {
            if (this.m_hThread != IntPtr.Zero)
            {
                Imports.CloseHandle(this.m_hThread);
            }
            this.m_hThread = IntPtr.Zero;
            this.m_ThreadId = 0;
            this.m_bThreadOpen = false;
        }

        public IntPtr CreateRemoteThread(uint dwStartAddress, uint dwParameter)
        {
            uint num;
            return this.CreateRemoteThread(dwStartAddress, dwParameter, 0, out num);
        }

        public IntPtr CreateRemoteThread(uint dwStartAddress, uint dwParameter, out uint dwThreadId)
        {
            return this.CreateRemoteThread(dwStartAddress, dwParameter, 0, out dwThreadId);
        }

        public IntPtr CreateRemoteThread(uint dwStartAddress, uint dwParameter, uint dwCreationFlags, out uint dwThreadId)
        {
            if (this.m_bProcessOpen)
            {
                return SThread.CreateRemoteThread(this.m_hProcess, dwStartAddress, dwParameter, dwCreationFlags, out dwThreadId);
            }
            dwThreadId = 0;
            return IntPtr.Zero;
        }

        public uint Execute(uint dwStartAddress)
        {
            return this.Execute(dwStartAddress, 0);
        }

        public uint Execute(uint dwStartAddress, uint dwParameter)
        {
            UIntPtr zero = UIntPtr.Zero;
            bool exitCodeThread = false;
            IntPtr hObject = this.CreateRemoteThread(dwStartAddress, dwParameter);
            if (hObject == IntPtr.Zero)
            {
                throw new Exception("Thread could not be remotely created.");
            }
            exitCodeThread = SThread.WaitForSingleObject(hObject, 0x2710) == 0;
            if (exitCodeThread)
            {
                exitCodeThread = Imports.GetExitCodeThread(hObject, out zero);
            }
            Imports.CloseHandle(hObject);
            if (!exitCodeThread)
            {
                throw new Exception("Error waiting for thread to exit or getting exit code.");
            }
            return (uint) zero;
        }

        ~BlackMagic()
        {
            this.Close();
        }

        public uint FindPattern(byte[] bPattern, string szMask)
        {
            return this.FindPattern((uint) ((int) this.MainModule.BaseAddress), this.MainModule.ModuleMemorySize, bPattern, szMask);
        }

        public uint FindPattern(string szPattern, string szMask)
        {
            return this.FindPattern(szPattern, szMask, ' ');
        }

        public uint FindPattern(ProcessModule pModule, byte[] bPattern, string szMask)
        {
            return this.FindPattern((uint) ((int) pModule.BaseAddress), pModule.ModuleMemorySize, bPattern, szMask);
        }

        public uint FindPattern(ProcessModule pModule, string szPattern, string szMask)
        {
            return this.FindPattern(pModule, szPattern, szMask, ' ');
        }

        public uint FindPattern(ProcessModuleCollection pModules, byte[] bPattern, string szMask)
        {
            uint num = 0;
            foreach (ProcessModule module in pModules)
            {
                num = this.FindPattern(module, bPattern, szMask);
                if (num != 0)
                {
                    return num;
                }
            }
            return num;
        }

        public uint FindPattern(ProcessModuleCollection pModules, string szPattern, string szMask)
        {
            return this.FindPattern(pModules, szPattern, szMask, ' ');
        }

        public uint FindPattern(string szPattern, string szMask, char Delimiter)
        {
            string[] strArray = szPattern.Split(new char[] { Delimiter });
            byte[] bPattern = new byte[strArray.Length];
            for (int i = 0; i < bPattern.Length; i++)
            {
                bPattern[i] = Convert.ToByte(strArray[i], 0x10);
            }
            return this.FindPattern(bPattern, szMask);
        }

        public uint FindPattern(ProcessModule[] pModules, byte[] bPattern, string szMask)
        {
            return this.FindPattern(new ProcessModuleCollection(pModules), bPattern, szMask);
        }

        public uint FindPattern(ProcessModule[] pModules, string szPattern, string szMask)
        {
            return this.FindPattern(new ProcessModuleCollection(pModules), szPattern, szMask, ' ');
        }

        public uint FindPattern(ProcessModule pModule, string szPattern, string szMask, char Delimiter)
        {
            string[] strArray = szPattern.Split(new char[] { Delimiter });
            byte[] bPattern = new byte[strArray.Length];
            for (int i = 0; i < bPattern.Length; i++)
            {
                bPattern[i] = Convert.ToByte(strArray[i], 0x10);
            }
            return this.FindPattern(pModule, bPattern, szMask);
        }

        public uint FindPattern(ProcessModuleCollection pModules, string szPattern, string szMask, char Delimiter)
        {
            string[] strArray = szPattern.Split(new char[] { Delimiter });
            byte[] bPattern = new byte[strArray.Length];
            for (int i = 0; i < bPattern.Length; i++)
            {
                bPattern[i] = Convert.ToByte(strArray[i], 0x10);
            }
            return this.FindPattern(pModules, bPattern, szMask);
        }

        public uint FindPattern(uint dwStart, int nSize, byte[] bPattern, string szMask)
        {
            PatternDataEntry entry = null;
            entry = new PatternDataEntry(dwStart, nSize, this.ReadBytes(dwStart, nSize));
            return (dwStart + SPattern.FindPattern(entry.bData, bPattern, szMask));
        }

        public uint FindPattern(uint dwStart, int nSize, string szPattern, string szMask)
        {
            return this.FindPattern(dwStart, nSize, szPattern, szMask, ' ');
        }

        public uint FindPattern(ProcessModule[] pModules, string szPattern, string szMask, char Delimiter)
        {
            return this.FindPattern(new ProcessModuleCollection(pModules), szPattern, szMask, Delimiter);
        }

        public uint FindPattern(uint dwStart, int nSize, string szPattern, string szMask, char Delimiter)
        {
            string[] strArray = szPattern.Split(new char[] { Delimiter });
            byte[] bPattern = new byte[strArray.Length];
            for (int i = 0; i < bPattern.Length; i++)
            {
                bPattern[i] = Convert.ToByte(strArray[i], 0x10);
            }
            return this.FindPattern(dwStart, nSize, bPattern, szMask);
        }

        public bool FreeMemory(uint dwAddress)
        {
            return this.FreeMemory(dwAddress, 0, 0x8000);
        }

        public bool FreeMemory(uint dwAddress, int nSize, uint dwFreeType)
        {
            return SMemory.FreeMemory(this.m_hProcess, dwAddress, nSize, dwFreeType);
        }

        public ProcessModule GetModule(string sModuleName)
        {
            foreach (ProcessModule module in this.m_Modules)
            {
                if (module.ModuleName.ToLower().Equals(sModuleName.ToLower()))
                {
                    return module;
                }
            }
            return null;
        }

        public ProcessModule GetModule(uint dwAddress)
        {
            foreach (ProcessModule module in this.m_Modules)
            {
                if ((((int) module.BaseAddress) <= dwAddress) && ((((ulong) ((int) module.BaseAddress)) + module.ModuleMemorySize) >= dwAddress))
                {
                    return module;
                }
            }
            return null;
        }

        public string GetModuleFilePath()
        {
            return this.m_MainModule.FileName;
        }

        public string GetModuleFilePath(int index)
        {
            return this.m_Modules[index].FileName;
        }

        public string GetModuleFilePath(string sModuleName)
        {
            foreach (ProcessModule module in this.m_Modules)
            {
                if (module.ModuleName.ToLower().Equals(sModuleName.ToLower()))
                {
                    return module.FileName;
                }
            }
            return string.Empty;
        }

        public uint InjectDllCreateThread(string szDllPath)
        {
            if (!this.m_bProcessOpen)
            {
                return 0;
            }
            return SInject.InjectDllCreateThread(this.m_hProcess, szDllPath);
        }

        public uint InjectDllRedirectThread(string szDllPath)
        {
            if (!this.m_bProcessOpen)
            {
                return 0;
            }
            if (this.m_bThreadOpen)
            {
                return SInject.InjectDllRedirectThread(this.m_hProcess, this.m_hThread, szDllPath);
            }
            return SInject.InjectDllRedirectThread(this.m_hProcess, this.m_ProcessId, szDllPath);
        }

        public uint InjectDllRedirectThread(IntPtr hThread, string szDllPath)
        {
            if (!this.m_bProcessOpen)
            {
                return 0;
            }
            return SInject.InjectDllRedirectThread(this.m_hProcess, hThread, szDllPath);
        }

        public bool Open(int ProcessId)
        {
            if (ProcessId == 0)
            {
                return false;
            }
            if (ProcessId == this.m_ProcessId)
            {
                return true;
            }
            if (this.m_bProcessOpen)
            {
                this.CloseProcess();
            }
            if (this.SetDebugPrivileges)
            {
                Process.EnterDebugMode();
            }
            this.m_bProcessOpen = (this.m_hProcess = SProcess.OpenProcess(ProcessId)) != IntPtr.Zero;
            if (this.m_bProcessOpen)
            {
                this.m_ProcessId = ProcessId;
                this.m_hWnd = SWindow.FindWindowByProcessId(ProcessId);
                this.m_Modules = Process.GetProcessById(this.m_ProcessId).Modules;
                this.m_MainModule = this.m_Modules[0];
                if (this.Asm == null)
                {
                    this.Asm = new ManagedFasm(this.m_hProcess);
                }
                else
                {
                    this.Asm.SetProcessHandle(this.m_hProcess);
                }
            }
            return this.m_bProcessOpen;
        }

        public bool Open(IntPtr WindowHandle)
        {
            if (WindowHandle == IntPtr.Zero)
            {
                return false;
            }
            return this.Open(SProcess.GetProcessFromWindow(WindowHandle));
        }

        public bool OpenProcessAndThread(int dwProcessId)
        {
            if (this.Open(dwProcessId) && this.OpenThread())
            {
                return true;
            }
            this.Close();
            return false;
        }

        public bool OpenProcessAndThread(IntPtr WindowHandle)
        {
            if (this.Open(WindowHandle) && this.OpenThread())
            {
                return true;
            }
            this.Close();
            return false;
        }

        public bool OpenThread()
        {
            return (this.m_bProcessOpen && this.OpenThread(SThread.GetMainThreadId(this.m_ProcessId)));
        }

        public bool OpenThread(int dwThreadId)
        {
            if (dwThreadId == 0)
            {
                return false;
            }
            if (dwThreadId == this.m_ThreadId)
            {
                return true;
            }
            if (this.m_bThreadOpen)
            {
                this.CloseThread();
            }
            this.m_bThreadOpen = (this.m_hThread = SThread.OpenThread(dwThreadId)) != IntPtr.Zero;
            if (this.m_bThreadOpen)
            {
                this.m_ThreadId = dwThreadId;
            }
            return this.m_bThreadOpen;
        }

        public string ReadASCIIString(uint dwAddress)
        {
            return this.ReadString(dwAddress, Encoding.ASCII, 0x200);
        }

        public string ReadASCIIString(uint dwAddress, int nLength)
        {
            return this.ReadString(dwAddress, Encoding.ASCII, nLength);
        }

        public byte ReadByte(uint dwAddress)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadByte(this.m_hProcess, dwAddress);
        }

        public byte[] ReadBytes(uint dwAddress, int nSize)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadBytes(this.m_hProcess, dwAddress, nSize);
        }

        public double ReadDouble(uint dwAddress)
        {
            return this.ReadDouble(dwAddress, false);
        }

        public double ReadDouble(uint dwAddress, bool bReverse)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadDouble(this.m_hProcess, dwAddress, bReverse);
        }

        public float ReadFloat(uint dwAddress)
        {
            return this.ReadFloat(dwAddress, false);
        }

        public float ReadFloat(uint dwAddress, bool bReverse)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadFloat(this.m_hProcess, dwAddress, bReverse);
        }

        public int ReadInt(uint dwAddress)
        {
            return this.ReadInt(dwAddress, false);
        }

        public int ReadInt(uint dwAddress, bool bReverse)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadInt(this.m_hProcess, dwAddress, bReverse);
        }

        public long ReadInt64(uint dwAddress)
        {
            return this.ReadInt64(dwAddress, false);
        }

        public long ReadInt64(uint dwAddress, bool bReverse)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadInt64(this.m_hProcess, dwAddress, bReverse);
        }

        public object ReadObject(uint dwAddress, Type objType)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadObject(this.m_hProcess, dwAddress, objType);
        }

        public sbyte ReadSByte(uint dwAddress)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadSByte(this.m_hProcess, dwAddress);
        }

        public short ReadShort(uint dwAddress)
        {
            return this.ReadShort(dwAddress, false);
        }

        public short ReadShort(uint dwAddress, bool bReverse)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadShort(this.m_hProcess, dwAddress, bReverse);
        }

        private string ReadString(uint address, Encoding encoding = null, int maxLength = 0x200)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = this.ReadBytes(address, maxLength);
            string str = encoding.GetString(bytes);
            if (str.IndexOf('\0') != -1)
            {
                str = str.Remove(str.IndexOf('\0'));
            }
            return str;
        }

        public T ReadT<T>(uint dwAddress, bool reverse = false) where T: struct
        {
            object obj2 = null;
            if (typeof(T) == typeof(ulong))
            {
                obj2 = this.ReadUInt64(dwAddress, reverse);
                return (T) obj2;
            }
            if (typeof(T) == typeof(UInt128))
            {
                obj2 = this.ReadUInt128(dwAddress, reverse);
                return (T) obj2;
            }
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Boolean:
                    obj2 = this.ReadShort(dwAddress, reverse) >= 0;
                    break;

                case TypeCode.Char:
                    obj2 = (char) ((ushort) this.ReadShort(dwAddress, reverse));
                    break;

                case TypeCode.Byte:
                    obj2 = this.ReadByte(dwAddress);
                    break;

                case TypeCode.Int16:
                    obj2 = this.ReadShort(dwAddress, reverse);
                    break;

                case TypeCode.UInt16:
                    obj2 = this.ReadUShort(dwAddress, reverse);
                    break;

                case TypeCode.Int32:
                    obj2 = this.ReadInt(dwAddress, reverse);
                    break;

                case TypeCode.UInt32:
                    obj2 = this.ReadUInt(dwAddress, reverse);
                    break;

                case TypeCode.Int64:
                    obj2 = this.ReadInt64(dwAddress, reverse);
                    break;

                case TypeCode.UInt64:
                    obj2 = this.ReadUInt64(dwAddress, reverse);
                    break;

                case TypeCode.Single:
                    obj2 = this.ReadFloat(dwAddress, reverse);
                    break;

                case TypeCode.Double:
                    obj2 = this.ReadDouble(dwAddress, reverse);
                    break;
            }
            if (obj2 != null)
            {
                return (T) obj2;
            }
            return default(T);
        }

        public uint ReadUInt(uint dwAddress)
        {
            return this.ReadUInt(dwAddress, false);
        }

        public uint ReadUInt(uint dwAddress, bool bReverse)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadUInt(this.m_hProcess, dwAddress, bReverse);
        }

        public UInt128 ReadUInt128(uint dwAddress)
        {
            return this.ReadUInt128(dwAddress, false);
        }

        public UInt128 ReadUInt128(uint dwAddress, bool bReverse)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadUInt128(this.m_hProcess, dwAddress, bReverse);
        }

        public ulong ReadUInt64(uint dwAddress)
        {
            return this.ReadUInt64(dwAddress, false);
        }

        public ulong ReadUInt64(uint dwAddress, bool bReverse)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadUInt64(this.m_hProcess, dwAddress, bReverse);
        }

        public string ReadUnicodeString(uint dwAddress, int nLength)
        {
            return SMemory.ReadUnicodeString(this.m_hProcess, dwAddress, nLength);
        }

        public ushort ReadUShort(uint dwAddress)
        {
            return this.ReadUShort(dwAddress, false);
        }

        public ushort ReadUShort(uint dwAddress, bool bReverse)
        {
            if (!this.m_bProcessOpen || (this.m_hProcess == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadUShort(this.m_hProcess, dwAddress, bReverse);
        }

        public string ReadUTF8String(uint dwAddress)
        {
            return this.ReadString(dwAddress, null, 0x200);
        }

        public string ReadUTF8String(uint dwAddress, int maxLenght)
        {
            return this.ReadString(dwAddress, Encoding.UTF8, maxLenght);
        }

        public bool ResumeThread()
        {
            if (!this.m_bThreadOpen)
            {
                return false;
            }
            return this.ResumeThread(this.m_hThread);
        }

        public bool ResumeThread(IntPtr hThread)
        {
            return (SThread.ResumeThread(hThread) != uint.MaxValue);
        }

        public bool SuspendThread()
        {
            if (!this.m_bThreadOpen)
            {
                return false;
            }
            return this.SuspendThread(this.m_hThread);
        }

        public bool SuspendThread(IntPtr hThread)
        {
            return (SThread.SuspendThread(hThread) != uint.MaxValue);
        }

        public bool WriteASCIIString(uint dwAddress, string Value)
        {
            return SMemory.WriteASCIIString(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteByte(uint dwAddress, byte Value)
        {
            return SMemory.WriteByte(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteBytes(uint dwAddress, byte[] Value)
        {
            return this.WriteBytes(dwAddress, Value, Value.Length);
        }

        public bool WriteBytes(uint dwAddress, byte[] Value, int nSize)
        {
            return SMemory.WriteBytes(this.m_hProcess, dwAddress, Value, nSize);
        }

        public bool WriteDouble(uint dwAddress, double Value)
        {
            return SMemory.WriteDouble(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteFloat(uint dwAddress, float Value)
        {
            return SMemory.WriteFloat(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteInt(uint dwAddress, int Value)
        {
            return SMemory.WriteInt(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteInt128(uint dwAddress, UInt128 Value)
        {
            return SMemory.WriteInt128(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteInt64(uint dwAddress, long Value)
        {
            return SMemory.WriteInt64(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteObject(uint dwAddress, object Value)
        {
            return SMemory.WriteObject(this.m_hProcess, dwAddress, Value, Value.GetType());
        }

        public bool WriteObject(uint dwAddress, object Value, Type objType)
        {
            return SMemory.WriteObject(this.m_hProcess, dwAddress, Value, objType);
        }

        public bool WriteSByte(uint dwAddress, sbyte Value)
        {
            return SMemory.WriteSByte(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteShort(uint dwAddress, short Value)
        {
            return SMemory.WriteShort(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteUInt(uint dwAddress, uint Value)
        {
            return SMemory.WriteUInt(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteUInt64(uint dwAddress, ulong Value)
        {
            return SMemory.WriteUInt64(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteUnicodeString(uint dwAddress, string Value)
        {
            return SMemory.WriteUnicodeString(this.m_hProcess, dwAddress, Value);
        }

        public bool WriteUShort(uint dwAddress, ushort Value)
        {
            return SMemory.WriteUShort(this.m_hProcess, dwAddress, Value);
        }

        public ManagedFasm Asm { get; set; }

        public bool IsProcessOpen
        {
            get
            {
                return this.m_bProcessOpen;
            }
        }

        public bool IsThreadOpen
        {
            get
            {
                return this.m_bThreadOpen;
            }
        }

        public ProcessModule MainModule
        {
            get
            {
                return this.m_MainModule;
            }
        }

        public ProcessModuleCollection Modules
        {
            get
            {
                return this.m_Modules;
            }
        }

        public IntPtr ProcessHandle
        {
            get
            {
                return this.m_hProcess;
            }
        }

        public int ProcessId
        {
            get
            {
                return this.m_ProcessId;
            }
        }

        public IntPtr ThreadHandle
        {
            get
            {
                return this.m_hThread;
            }
        }

        public int ThreadId
        {
            get
            {
                return this.m_ThreadId;
            }
        }

        public IntPtr WindowHandle
        {
            get
            {
                return this.m_hWnd;
            }
        }

        private class PatternDataEntry
        {
            public byte[] bData;
            public int Size;
            public uint Start;

            public PatternDataEntry()
            {
            }

            public PatternDataEntry(uint Start, int Size, byte[] bData)
            {
                this.Start = Start;
                this.Size = Size;
                this.bData = bData;
            }
        }
    }
}

