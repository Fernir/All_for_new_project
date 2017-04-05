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
        private ProcessModule _afeovuofuwedaIsa;
        private IntPtr _akowuafe;
        private ProcessModuleCollection _alowiotiwobaik;
        private bool _amuajepuAj;
        private IntPtr _awahelegilAwopoqe;
        private bool _eqiudiuhumeib;
        private int _geivemioh;
        private const uint _isiajuamuofonTuhiojai = 0;
        private List<Araulew> _ohuefaSo;
        private IntPtr _periwe;
        private int _wieveu;
        public bool SetDebugPrivileges;

        public BlackMagic()
        {
            this.SetDebugPrivileges = true;
            this._akowuafe = IntPtr.Zero;
            this._awahelegilAwopoqe = IntPtr.Zero;
            this._periwe = IntPtr.Zero;
            this.Asm = new ManagedFasm();
            this._ohuefaSo = new List<Araulew>();
            if (this._amuajepuAj && (this._akowuafe != IntPtr.Zero))
            {
                this.Asm.SetProcessHandle(this._akowuafe);
            }
        }

        public BlackMagic(int ProcessId) : this()
        {
            this._amuajepuAj = this.Open(ProcessId);
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
            return SMemory.AllocateMemory(this._akowuafe, nSize, dwAllocationType, dwProtect);
        }

        public void Close()
        {
            this.Asm.Dispose();
            this.CloseProcess();
            this.CloseThread();
        }

        public void CloseProcess()
        {
            if (this._akowuafe != IntPtr.Zero)
            {
                Imports.CloseHandle(this._akowuafe);
            }
            this._akowuafe = IntPtr.Zero;
            this._awahelegilAwopoqe = IntPtr.Zero;
            this._wieveu = 0;
            this._afeovuofuwedaIsa = null;
            this._alowiotiwobaik = null;
            this._amuajepuAj = false;
            this.Asm.SetProcessHandle(IntPtr.Zero);
        }

        public void CloseThread()
        {
            if (this._periwe != IntPtr.Zero)
            {
                Imports.CloseHandle(this._periwe);
            }
            this._periwe = IntPtr.Zero;
            this._geivemioh = 0;
            this._eqiudiuhumeib = false;
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
            if (this._amuajepuAj)
            {
                return SThread.CreateRemoteThread(this._akowuafe, dwStartAddress, dwParameter, dwCreationFlags, out dwThreadId);
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
            Araulew araulew = null;
            araulew = new Araulew(dwStart, nSize, this.ReadBytes(dwStart, nSize));
            return (dwStart + SPattern.FindPattern(araulew._gebode, bPattern, szMask));
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
            return SMemory.FreeMemory(this._akowuafe, dwAddress, nSize, dwFreeType);
        }

        public ProcessModule GetModule(string sModuleName)
        {
            foreach (ProcessModule module in this._alowiotiwobaik)
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
            foreach (ProcessModule module in this._alowiotiwobaik)
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
            return this._afeovuofuwedaIsa.FileName;
        }

        public string GetModuleFilePath(int index)
        {
            return this._alowiotiwobaik[index].FileName;
        }

        public string GetModuleFilePath(string sModuleName)
        {
            foreach (ProcessModule module in this._alowiotiwobaik)
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
            if (!this._amuajepuAj)
            {
                return 0;
            }
            return SInject.InjectDllCreateThread(this._akowuafe, szDllPath);
        }

        public uint InjectDllRedirectThread(string szDllPath)
        {
            if (!this._amuajepuAj)
            {
                return 0;
            }
            if (this._eqiudiuhumeib)
            {
                return SInject.InjectDllRedirectThread(this._akowuafe, this._periwe, szDllPath);
            }
            return SInject.InjectDllRedirectThread(this._akowuafe, this._wieveu, szDllPath);
        }

        public uint InjectDllRedirectThread(IntPtr hThread, string szDllPath)
        {
            if (!this._amuajepuAj)
            {
                return 0;
            }
            return SInject.InjectDllRedirectThread(this._akowuafe, hThread, szDllPath);
        }

        private string MoecaebiuMeusomi(uint evuiboraikaUs, Encoding pihiuku = null, int agaqiapeojiXeiperu = 0x200)
        {
            if (pihiuku == null)
            {
                pihiuku = Encoding.UTF8;
            }
            byte[] bytes = this.ReadBytes(evuiboraikaUs, agaqiapeojiXeiperu);
            string str = pihiuku.GetString(bytes);
            if (str.IndexOf('\0') != -1)
            {
                str = str.Remove(str.IndexOf('\0'));
            }
            return str;
        }

        public bool Open(int ProcessId)
        {
            if (ProcessId == 0)
            {
                return false;
            }
            if (ProcessId == this._wieveu)
            {
                return true;
            }
            if (this._amuajepuAj)
            {
                this.CloseProcess();
            }
            if (this.SetDebugPrivileges)
            {
                Process.EnterDebugMode();
            }
            this._amuajepuAj = (this._akowuafe = SProcess.OpenProcess(ProcessId)) != IntPtr.Zero;
            if (this._amuajepuAj)
            {
                this._wieveu = ProcessId;
                this._awahelegilAwopoqe = SWindow.FindWindowByProcessId(ProcessId);
                this._alowiotiwobaik = Process.GetProcessById(this._wieveu).Modules;
                this._afeovuofuwedaIsa = this._alowiotiwobaik[0];
                if (this.Asm == null)
                {
                    this.Asm = new ManagedFasm(this._akowuafe);
                }
                else
                {
                    this.Asm.SetProcessHandle(this._akowuafe);
                }
            }
            return this._amuajepuAj;
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
            return (this._amuajepuAj && this.OpenThread(SThread.GetMainThreadId(this._wieveu)));
        }

        public bool OpenThread(int dwThreadId)
        {
            if (dwThreadId == 0)
            {
                return false;
            }
            if (dwThreadId == this._geivemioh)
            {
                return true;
            }
            if (this._eqiudiuhumeib)
            {
                this.CloseThread();
            }
            this._eqiudiuhumeib = (this._periwe = SThread.OpenThread(dwThreadId)) != IntPtr.Zero;
            if (this._eqiudiuhumeib)
            {
                this._geivemioh = dwThreadId;
            }
            return this._eqiudiuhumeib;
        }

        public string ReadASCIIString(uint dwAddress)
        {
            return this.MoecaebiuMeusomi(dwAddress, Encoding.ASCII, 0x200);
        }

        public string ReadASCIIString(uint dwAddress, int nLength)
        {
            return this.MoecaebiuMeusomi(dwAddress, Encoding.ASCII, nLength);
        }

        public byte ReadByte(uint dwAddress)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadByte(this._akowuafe, dwAddress);
        }

        public byte[] ReadBytes(uint dwAddress, int nSize)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadBytes(this._akowuafe, dwAddress, nSize);
        }

        public double ReadDouble(uint dwAddress)
        {
            return this.ReadDouble(dwAddress, false);
        }

        public double ReadDouble(uint dwAddress, bool bReverse)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadDouble(this._akowuafe, dwAddress, bReverse);
        }

        public float ReadFloat(uint dwAddress)
        {
            return this.ReadFloat(dwAddress, false);
        }

        public float ReadFloat(uint dwAddress, bool bReverse)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadFloat(this._akowuafe, dwAddress, bReverse);
        }

        public int ReadInt(uint dwAddress)
        {
            return this.ReadInt(dwAddress, false);
        }

        public int ReadInt(uint dwAddress, bool bReverse)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadInt(this._akowuafe, dwAddress, bReverse);
        }

        public long ReadInt64(uint dwAddress)
        {
            return this.ReadInt64(dwAddress, false);
        }

        public long ReadInt64(uint dwAddress, bool bReverse)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadInt64(this._akowuafe, dwAddress, bReverse);
        }

        public object ReadObject(uint dwAddress, Type objType)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadObject(this._akowuafe, dwAddress, objType);
        }

        public sbyte ReadSByte(uint dwAddress)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadSByte(this._akowuafe, dwAddress);
        }

        public short ReadShort(uint dwAddress)
        {
            return this.ReadShort(dwAddress, false);
        }

        public short ReadShort(uint dwAddress, bool bReverse)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadShort(this._akowuafe, dwAddress, bReverse);
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
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadUInt(this._akowuafe, dwAddress, bReverse);
        }

        public UInt128 ReadUInt128(uint dwAddress)
        {
            return this.ReadUInt128(dwAddress, false);
        }

        public UInt128 ReadUInt128(uint dwAddress, bool bReverse)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadUInt128(this._akowuafe, dwAddress, bReverse);
        }

        public ulong ReadUInt64(uint dwAddress)
        {
            return this.ReadUInt64(dwAddress, false);
        }

        public ulong ReadUInt64(uint dwAddress, bool bReverse)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadUInt64(this._akowuafe, dwAddress, bReverse);
        }

        public string ReadUnicodeString(uint dwAddress, int nLength)
        {
            return SMemory.ReadUnicodeString(this._akowuafe, dwAddress, nLength);
        }

        public ushort ReadUShort(uint dwAddress)
        {
            return this.ReadUShort(dwAddress, false);
        }

        public ushort ReadUShort(uint dwAddress, bool bReverse)
        {
            if (!this._amuajepuAj || (this._akowuafe == IntPtr.Zero))
            {
                throw new Exception("Process is not open for read/write.");
            }
            return SMemory.ReadUShort(this._akowuafe, dwAddress, bReverse);
        }

        public string ReadUTF8String(uint dwAddress)
        {
            return this.MoecaebiuMeusomi(dwAddress, null, 0x200);
        }

        public string ReadUTF8String(uint dwAddress, int maxLenght)
        {
            return this.MoecaebiuMeusomi(dwAddress, Encoding.UTF8, maxLenght);
        }

        public bool ResumeThread()
        {
            if (!this._eqiudiuhumeib)
            {
                return false;
            }
            return this.ResumeThread(this._periwe);
        }

        public bool ResumeThread(IntPtr hThread)
        {
            return (SThread.ResumeThread(hThread) != uint.MaxValue);
        }

        public bool SuspendThread()
        {
            if (!this._eqiudiuhumeib)
            {
                return false;
            }
            return this.SuspendThread(this._periwe);
        }

        public bool SuspendThread(IntPtr hThread)
        {
            return (SThread.SuspendThread(hThread) != uint.MaxValue);
        }

        public bool WriteASCIIString(uint dwAddress, string Value)
        {
            return SMemory.WriteASCIIString(this._akowuafe, dwAddress, Value);
        }

        public bool WriteByte(uint dwAddress, byte Value)
        {
            return SMemory.WriteByte(this._akowuafe, dwAddress, Value);
        }

        public bool WriteBytes(uint dwAddress, byte[] Value)
        {
            return this.WriteBytes(dwAddress, Value, Value.Length);
        }

        public bool WriteBytes(uint dwAddress, byte[] Value, int nSize)
        {
            return SMemory.WriteBytes(this._akowuafe, dwAddress, Value, nSize);
        }

        public bool WriteDouble(uint dwAddress, double Value)
        {
            return SMemory.WriteDouble(this._akowuafe, dwAddress, Value);
        }

        public bool WriteFloat(uint dwAddress, float Value)
        {
            return SMemory.WriteFloat(this._akowuafe, dwAddress, Value);
        }

        public bool WriteInt(uint dwAddress, int Value)
        {
            return SMemory.WriteInt(this._akowuafe, dwAddress, Value);
        }

        public bool WriteInt128(uint dwAddress, UInt128 Value)
        {
            return SMemory.WriteInt128(this._akowuafe, dwAddress, Value);
        }

        public bool WriteInt64(uint dwAddress, long Value)
        {
            return SMemory.WriteInt64(this._akowuafe, dwAddress, Value);
        }

        public bool WriteObject(uint dwAddress, object Value)
        {
            return SMemory.WriteObject(this._akowuafe, dwAddress, Value, Value.GetType());
        }

        public bool WriteObject(uint dwAddress, object Value, Type objType)
        {
            return SMemory.WriteObject(this._akowuafe, dwAddress, Value, objType);
        }

        public bool WriteSByte(uint dwAddress, sbyte Value)
        {
            return SMemory.WriteSByte(this._akowuafe, dwAddress, Value);
        }

        public bool WriteShort(uint dwAddress, short Value)
        {
            return SMemory.WriteShort(this._akowuafe, dwAddress, Value);
        }

        public bool WriteUInt(uint dwAddress, uint Value)
        {
            return SMemory.WriteUInt(this._akowuafe, dwAddress, Value);
        }

        public bool WriteUInt64(uint dwAddress, ulong Value)
        {
            return SMemory.WriteUInt64(this._akowuafe, dwAddress, Value);
        }

        public bool WriteUnicodeString(uint dwAddress, string Value)
        {
            return SMemory.WriteUnicodeString(this._akowuafe, dwAddress, Value);
        }

        public bool WriteUShort(uint dwAddress, ushort Value)
        {
            return SMemory.WriteUShort(this._akowuafe, dwAddress, Value);
        }

        public ManagedFasm Asm { get; set; }

        public bool IsProcessOpen
        {
            get
            {
                return this._amuajepuAj;
            }
        }

        public bool IsThreadOpen
        {
            get
            {
                return this._eqiudiuhumeib;
            }
        }

        public ProcessModule MainModule
        {
            get
            {
                return this._afeovuofuwedaIsa;
            }
        }

        public ProcessModuleCollection Modules
        {
            get
            {
                return this._alowiotiwobaik;
            }
        }

        public IntPtr ProcessHandle
        {
            get
            {
                return this._akowuafe;
            }
        }

        public int ProcessId
        {
            get
            {
                return this._wieveu;
            }
        }

        public IntPtr ThreadHandle
        {
            get
            {
                return this._periwe;
            }
        }

        public int ThreadId
        {
            get
            {
                return this._geivemioh;
            }
        }

        public IntPtr WindowHandle
        {
            get
            {
                return this._awahelegilAwopoqe;
            }
        }

        private class Araulew
        {
            public uint _diubekuihoQifuiw;
            public byte[] _gebode;
            public int _vegao;

            public Araulew()
            {
            }

            public Araulew(uint Start, int Size, byte[] bData)
            {
                this._diubekuihoQifuiw = Start;
                this._vegao = Size;
                this._gebode = bData;
            }
        }
    }
}

