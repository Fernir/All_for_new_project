namespace nManager.Wow.MemoryClass
{
    using Fasm;
    using nManager;
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Helpers;
    using nManager.Wow.MemoryClass.Magic;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Hook
    {
        private readonly BlackMagic _memory = new BlackMagic();
        private uint _mExecuteRequested;
        private uint _mInjectionCode;
        private uint _mLocked;
        private uint _mLockRequested;
        private uint _mResult;
        private uint _mTrampoline;
        private byte[] _mZeroBytesInjectionCodes;
        public bool AllowReHook;
        internal uint JumpAddress;
        public static readonly object Locker = new object();
        public static readonly object LockerUnlock = new object();
        public bool ThreadHooked;

        public Hook()
        {
            try
            {
                this.Hooking();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Hook(): " + exception, true);
            }
        }

        public void Apply()
        {
            ManagedFasm fasm = new ManagedFasm(this.Memory.ProcessHandle);
            fasm.SetMemorySize(0x1000);
            fasm.SetPassLimit(100);
            fasm.AddLine("jmp " + this._mTrampoline);
            fasm.Inject(this.JumpAddress);
        }

        private void CheckEndsceneHook()
        {
            byte num = this.Memory.ReadByte(this.JumpAddress);
            if ((num != 0xe9) && this.AllowReHook)
            {
                this.ThreadHooked = false;
                this.AllowReHook = false;
                Logging.WriteError("ThreadHooked: UnHooked; JmpAddress: " + num.ToString("X") + ", trying to reHook.", true);
                this.Hooking();
            }
        }

        private void CreateTrampoline()
        {
            this._mTrampoline = this.Memory.AllocateMemory(0x1000);
            Console.WriteLine("m_trampoline : " + this._mTrampoline.ToString("X"));
            ManagedFasm fasm = new ManagedFasm(this.Memory.ProcessHandle);
            fasm.SetMemorySize(0x1000);
            fasm.SetPassLimit(100);
            fasm.AddLine("pushad");
            fasm.AddLine("pushfd");
            fasm.AddLine("@execution:");
            fasm.AddLine("mov eax, [{0}]", new object[] { this._mExecuteRequested });
            fasm.AddLine("test eax, eax");
            fasm.AddLine("je @lockcheck");
            fasm.AddLine("call {0}", new object[] { this._mInjectionCode });
            fasm.AddLine("mov [" + this._mResult + "], eax");
            fasm.AddLine("xor eax, eax");
            fasm.AddLine("mov [" + this._mExecuteRequested + "], eax");
            fasm.AddLine("@lockcheck:");
            fasm.AddLine("mov eax, [{0}]", new object[] { this._mLockRequested });
            fasm.AddLine("test eax, eax");
            fasm.AddLine("je @exit");
            fasm.AddLine("mov eax, 1");
            fasm.AddLine("mov [" + this._mLocked + "], eax");
            fasm.AddLine("jmp @execution");
            fasm.AddLine("@exit:");
            fasm.AddLine("xor eax, eax");
            fasm.AddLine("mov [" + this._mLocked + "], eax");
            fasm.AddLine("popfd");
            fasm.AddLine("popad");
            fasm.AddLine("jmp " + (this.JumpAddress + D3D.OriginalBytes.Length));
            this.Memory.WriteBytes(this._mTrampoline, D3D.OriginalBytes);
            fasm.Inject((uint modopt(IsLong)) (this._mTrampoline + D3D.OriginalBytes.Length));
        }

        internal void DisposeHooking()
        {
            try
            {
                if (this.Memory.IsProcessOpen)
                {
                    this.JumpAddress = this.GetJumpAdresse();
                    if (this.Memory.ReadByte(this.JumpAddress) == 0xe9)
                    {
                        lock (Locker)
                        {
                            if (D3D.OriginalBytes == null)
                            {
                                D3D.OriginalBytes = this.Memory.ReadBytes(this.JumpAddress, 5);
                                byte[] buffer = new byte[5];
                                if (D3D.OriginalBytes == buffer)
                                {
                                    Others.OpenWebBrowserOrApplication("http://thenoobbot.com/community/viewtopic.php?f=43&t=464");
                                    Logging.Write("An error is detected, you must switch the DirectX version used by your WoW client !");
                                    MessageBox.Show("An error is detected, you must switch the DirectX version used by your WoW client !");
                                    Pulsator.Dispose(true);
                                    return;
                                }
                                byte[] buffer2 = this.Memory.ReadBytes(this.JumpAddress, 9);
                                if ((buffer2[5] != 0x90) && (buffer2[6] != 0x90))
                                {
                                    D3D.OriginalBytes = new byte[] { 0x8b, 0xff, 0x55, 0x8b, 0xec };
                                }
                                else if ((buffer2[5] == 0x90) && (buffer2[6] != 0x90))
                                {
                                    D3D.OriginalBytes = new byte[] { 0x55, 0x8b, 0xec, 0x8b, 0x45, 8 };
                                }
                                else if ((buffer2[5] == 0x90) && (buffer2[6] == 0x90))
                                {
                                    D3D.OriginalBytes = new byte[] { 0x6a, 20, 0xb8, 12, 0x9a, 0x44, 0x73 };
                                }
                                else
                                {
                                    string str = "";
                                    byte[] buffer4 = buffer2;
                                    for (int i = 0; i < buffer4.Length; i++)
                                    {
                                        uint num = buffer4[i];
                                        if (str == "")
                                        {
                                            str = num.ToString();
                                        }
                                        else
                                        {
                                            str = str + ", " + num;
                                        }
                                    }
                                    Logging.WriteError("Error Hook_01 : Couldn't dispose previous Hooking correctly, please open a bug report thread on the forum with this log file.", true);
                                    Logging.WriteError("Error Hook_02 : " + str, true);
                                    Others.OpenWebBrowserOrApplication("http://thenoobbot.com/community/viewtopic.php?f=43&t=464");
                                    MessageBox.Show("World of Warcraft is currently in use by another Application than TheNoobBot and we could not automaticallt unhook it, try restarting the WoW Client, if this issue persist, open a bug report with this log file.");
                                    Pulsator.Dispose(true);
                                }
                            }
                            this.Remove(this.JumpAddress, D3D.OriginalBytes);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposeHooking(): " + exception, true);
            }
        }

        public void GameFrameLock()
        {
            lock (Locker)
            {
                if (nManagerSetting.CurrentSetting.UseFrameLock)
                {
                    this.Memory.WriteUInt(this._mLocked, 0);
                    this.Memory.WriteUInt(this._mLockRequested, 1);
                }
            }
        }

        public void GameFrameUnLock()
        {
            lock (LockerUnlock)
            {
                this.Memory.WriteUInt(this._mLockRequested, 0);
            }
        }

        private uint GetJumpAdresse()
        {
            try
            {
                if (D3D.IsD3D11(this.Memory.ProcessId))
                {
                    return D3D.D3D11Adresse();
                }
                return D3D.D3D9Adresse(this.Memory.ProcessId);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetJumpAdresse(): " + exception, true);
            }
            return 0;
        }

        private void Hooking()
        {
            try
            {
                lock (Locker)
                {
                    if (nManager.Wow.Memory.WowProcess.ProcessId > 0)
                    {
                        if (!this.Memory.IsProcessOpen || (this.Memory.ProcessId != nManager.Wow.Memory.WowProcess.ProcessId))
                        {
                            this.Memory.OpenProcessAndThread(nManager.Wow.Memory.WowProcess.ProcessId);
                        }
                        if (this.Memory.IsProcessOpen)
                        {
                            uint num = nManager.Wow.Helpers.Usefuls.WowVersion(this.Memory.ReadUTF8String(nManager.Wow.Memory.WowProcess.WowModule + 0xc5db08));
                            if (num != 0x54ee)
                            {
                                if (!System.Diagnostics.Process.GetProcessById(this.Memory.ProcessId).HasExited)
                                {
                                    if (((num == 0) || (num < 0x42e9)) || (num > 0x61a8))
                                    {
                                        MessageBox.Show(Translate.Get(Translate.Id.UpdateRequiredCases) + Environment.NewLine + Environment.NewLine + Translate.Get(Translate.Id.UpdateRequiredCase1) + Environment.NewLine + Translate.Get(Translate.Id.UpdateRequiredCase2), Translate.Get(Translate.Id.UpdateRequiredCasesTitle));
                                    }
                                    else
                                    {
                                        if (0x54ee > num)
                                        {
                                            MessageBox.Show(string.Concat(new object[] { Translate.Get(Translate.Id.UpdateRequireOlderTheNoobBot), "6.2.4", Translate.Get(Translate.Id.RunningWoWBuildDot), 0x54ee, Translate.Get(Translate.Id.RunningWoWBuild), num, Translate.Get(Translate.Id.RunningWoWBuildDot), Environment.NewLine, Environment.NewLine, Translate.Get(Translate.Id.PleaseDownloadOlder) }), Translate.Get(Translate.Id.UpdateRequireOlderTheNoobBotTitle));
                                        }
                                        if (0x54ee < num)
                                        {
                                            MessageBox.Show(string.Concat(new object[] { Translate.Get(Translate.Id.UpdateRequireNewerTheNoobBot), "6.2.4", Translate.Get(Translate.Id.RunningWoWBuildDot), 0x54ee, Translate.Get(Translate.Id.RunningWoWBuild), num, Translate.Get(Translate.Id.RunningWoWBuildDot), Environment.NewLine, Environment.NewLine, Translate.Get(Translate.Id.PleaseDownloadNewer) }), Translate.Get(Translate.Id.UpdateRequireNewerTheNoobBotTitle));
                                        }
                                    }
                                }
                                return;
                            }
                            this.JumpAddress = this.GetJumpAdresse();
                            if (this.Memory.ReadByte(this.JumpAddress) == 0xe9)
                            {
                                this.DisposeHooking();
                            }
                            try
                            {
                                if (D3D.OriginalBytes == null)
                                {
                                    byte[] buffer = this.Memory.ReadBytes(this.JumpAddress, 10);
                                    string str2 = "";
                                    byte[] buffer2 = buffer;
                                    for (int j = 0; j < buffer2.Length; j++)
                                    {
                                        uint num2 = buffer2[j];
                                        if (str2 == "")
                                        {
                                            str2 = num2.ToString();
                                        }
                                        else
                                        {
                                            str2 = str2 + ", " + num2;
                                        }
                                    }
                                    Logging.WriteFileOnly("Hooking Informations: " + str2);
                                    D3D.OriginalBytes = this.Memory.ReadBytes(this.JumpAddress, 5);
                                    if (D3D.OriginalBytes[0] == 0x55)
                                    {
                                        D3D.OriginalBytes = this.Memory.ReadBytes(this.JumpAddress, 6);
                                    }
                                    else if (D3D.OriginalBytes[0] == 0x6a)
                                    {
                                        D3D.OriginalBytes = this.Memory.ReadBytes(this.JumpAddress, 7);
                                    }
                                }
                                this._mLockRequested = this.Memory.AllocateMemory(4);
                                this._mLocked = this.Memory.AllocateMemory(4);
                                this._mResult = this.Memory.AllocateMemory(4);
                                this._mExecuteRequested = this.Memory.AllocateMemory(4);
                                this._mZeroBytesInjectionCodes = new byte[0x1000];
                                for (int i = 0; i < 0x1000; i++)
                                {
                                    this._mZeroBytesInjectionCodes[i] = 0;
                                }
                                this._mInjectionCode = this.Memory.AllocateMemory(this._mZeroBytesInjectionCodes.Length);
                                this.CreateTrampoline();
                                this.Apply();
                            }
                            catch (Exception exception)
                            {
                                Logging.WriteError("Hooking()#1: " + exception, true);
                                this.ThreadHooked = false;
                                return;
                            }
                        }
                        this.ThreadHooked = true;
                        this.AllowReHook = true;
                    }
                }
            }
            catch (Exception exception2)
            {
                Logging.WriteError("Hooking()#2: " + exception2, true);
            }
        }

        public uint InjectAndExecute(string[] asm)
        {
            lock (Locker)
            {
                if (!this.ThreadHooked)
                {
                    return 0;
                }
                ManagedFasm fasm = new ManagedFasm(this.Memory.ProcessHandle);
                fasm.SetMemorySize(0x1000);
                fasm.SetPassLimit(100);
                foreach (string str in asm)
                {
                    fasm.AddLine(str);
                }
                fasm.Inject(this._mInjectionCode);
                this.Memory.WriteUInt(this._mExecuteRequested, 1);
                nManager.Helpful.Timer timer = new nManager.Helpful.Timer(2000.0);
                timer.Reset();
                while ((this.Memory.ReadUInt(this._mExecuteRequested) == 1) && !timer.IsReady)
                {
                    Thread.Sleep(0);
                }
                if (timer.IsReady)
                {
                    string str2 = "";
                    for (int i = 10; i >= 1; i--)
                    {
                        StackFrame frame = new StackFrame(i);
                        if (frame.GetMethod() != null)
                        {
                            str2 = str2 + frame.GetMethod().Name + " => ";
                        }
                    }
                    Logging.WriteError("Injection have been aborted, execution too long from " + str2.Substring(0, str2.Length - 4), true);
                    return 0;
                }
                this.Memory.WriteBytes(this._mInjectionCode, this._mZeroBytesInjectionCodes);
                return this.Memory.ReadUInt(this._mResult);
            }
        }

        public static bool IsInGame(int processId)
        {
            try
            {
                BlackMagic magic = new BlackMagic(processId);
                System.Diagnostics.Process processById = System.Diagnostics.Process.GetProcessById(processId);
                uint baseAddress = 0;
                foreach (ProcessModule module in from v in magic.Modules.Cast<ProcessModule>()
                    where string.Equals(v.ModuleName, processById.ProcessName + ".exe", StringComparison.CurrentCultureIgnoreCase)
                    select v)
                {
                    baseAddress = (uint) ((int) module.BaseAddress);
                }
                return ((magic.ReadInt(baseAddress + 0xbf8754) == 0) && (magic.ReadByte(baseAddress + 0xda55b2) > 0));
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsInGame(int processId): " + exception, true);
            }
            return false;
        }

        public static string PlayerName(int processId)
        {
            try
            {
                if (!IsInGame(processId))
                {
                    return Translate.Get(Translate.Id.Please_connect_to_the_game);
                }
                BlackMagic magic = new BlackMagic(processId);
                System.Diagnostics.Process processById = System.Diagnostics.Process.GetProcessById(processId);
                uint baseAddress = 0;
                foreach (ProcessModule module in from v in magic.Modules.Cast<ProcessModule>()
                    where string.Equals(v.ModuleName, processById.ProcessName + ".exe", StringComparison.CurrentCultureIgnoreCase)
                    select v)
                {
                    baseAddress = (uint) ((int) module.BaseAddress);
                }
                string str = magic.ReadUTF8String(baseAddress + 0xe981d0);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("PlayerName(int processId): " + exception, true);
            }
            return "No Name";
        }

        public void Remove(uint address, byte[] originalBytes)
        {
            this.Memory.WriteBytes(address, originalBytes);
        }

        public static bool WowIsUsed(int processId)
        {
            try
            {
                uint dwAddress = 0;
                if (D3D.IsD3D11(processId))
                {
                    dwAddress = D3D.D3D11Adresse();
                }
                else
                {
                    dwAddress = D3D.D3D9Adresse(processId);
                }
                BlackMagic magic = new BlackMagic(processId);
                return (magic.ReadByte(dwAddress) == 0xe9);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WowIsUsed(int processId): " + exception, true);
            }
            return false;
        }

        public bool IsGameFrameLocked
        {
            get
            {
                return (this.Memory.ReadUInt(this._mLocked) == 1);
            }
        }

        public BlackMagic Memory
        {
            get
            {
                try
                {
                    return this._memory;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("BlackMagic Memory: " + exception, true);
                    return new BlackMagic();
                }
            }
        }
    }
}

