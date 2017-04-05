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
    using System.Threading;
    using System.Windows.Forms;

    public class Hook
    {
        private uint _apucuihauluiOq;
        private uint _iniugiam;
        private uint _itubuadiufaojSexexuo;
        internal uint _iwuimeu;
        private uint _jokuowes;
        private uint _kuixaihutei;
        private uint _leoxafunihouxuEw;
        private readonly BlackMagic _memory = new BlackMagic();
        private byte[] _obuodue;
        private uint _onofaijoxoul;
        private uint _pemequ;
        private int _ufainoxeugi;
        private uint _vuseomaiheta;
        internal uint _wuoxuaxiatocaAvoawado;
        public bool AllowReHook;
        public static readonly object Locker = new object();
        public bool ThreadHooked;

        public Hook()
        {
            try
            {
                this.Beruili();
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
            fasm.AddLine("jmp " + this._vuseomaiheta);
            fasm.Inject(this._iwuimeu);
        }

        public void ApplyDX()
        {
            ManagedFasm fasm = new ManagedFasm(this.Memory.ProcessHandle);
            fasm.SetMemorySize(0x1000);
            fasm.SetPassLimit(100);
            fasm.AddLine("jmp " + this._onofaijoxoul);
            if (Bagoic.get_OriginalBytesDX().Length > 5)
            {
                fasm.AddLine("nop");
            }
            if (Bagoic.get_OriginalBytesDX().Length > 6)
            {
                fasm.AddLine("nop");
            }
            fasm.Inject(this._wuoxuaxiatocaAvoawado);
        }

        private void Bareuwiaj()
        {
            byte num = this.Memory.ReadByte(this._iwuimeu);
            if ((num != 0xe9) && this.AllowReHook)
            {
                this.ThreadHooked = false;
                this.AllowReHook = false;
                Logging.WriteError("ThreadHooked: UnHooked; JmpAddress: " + num.ToString("X") + ", trying to reHook.", true);
                this.Beruili();
            }
        }

        private void Beruili()
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
                            uint num = nManager.Wow.Helpers.Usefuls.WowVersion(this.Memory.ReadUTF8String(nManager.Wow.Memory.WowProcess.WowModule + 0xdcaac0));
                            if (num != 0x5d31)
                            {
                                if (!System.Diagnostics.Process.GetProcessById(this.Memory.ProcessId).HasExited)
                                {
                                    if (((num == 0) || (num < 0x42e9)) || (num > 0x6590))
                                    {
                                        MessageBox.Show(Translate.Get(Translate.Id.UpdateRequiredCases) + Environment.NewLine + Environment.NewLine + Translate.Get(Translate.Id.UpdateRequiredCase1) + Environment.NewLine + Translate.Get(Translate.Id.UpdateRequiredCase2), Translate.Get(Translate.Id.UpdateRequiredCasesTitle));
                                    }
                                    else
                                    {
                                        if (0x5d31 > num)
                                        {
                                            MessageBox.Show(string.Concat(new object[] { Translate.Get(Translate.Id.UpdateRequireOlderTheNoobBot), "7.2.0", Translate.Get(Translate.Id.RunningWoWBuildDot), 0x5d31, Translate.Get(Translate.Id.RunningWoWBuild), num, Translate.Get(Translate.Id.RunningWoWBuildDot), Environment.NewLine, Environment.NewLine, Translate.Get(Translate.Id.PleaseDownloadOlder) }), Translate.Get(Translate.Id.UpdateRequireOlderTheNoobBotTitle));
                                        }
                                        if (0x5d31 < num)
                                        {
                                            MessageBox.Show(string.Concat(new object[] { Translate.Get(Translate.Id.UpdateRequireNewerTheNoobBot), "7.2.0", Translate.Get(Translate.Id.RunningWoWBuildDot), 0x5d31, Translate.Get(Translate.Id.RunningWoWBuild), num, Translate.Get(Translate.Id.RunningWoWBuildDot), Environment.NewLine, Environment.NewLine, Translate.Get(Translate.Id.PleaseDownloadNewer) }), Translate.Get(Translate.Id.UpdateRequireNewerTheNoobBotTitle));
                                        }
                                    }
                                }
                                return;
                            }
                            this._iwuimeu = this.CoxiafebobioUbaoxi();
                            this._wuoxuaxiatocaAvoawado = this.OsoeboeneoqiBioqaceEjiuci();
                            if ((this.Memory.ReadByte(this._iwuimeu) == 0xe9) || (this.Memory.ReadByte(this._wuoxuaxiatocaAvoawado) == 0xe9))
                            {
                                this.Pujiegeamio();
                            }
                            try
                            {
                                if (Bagoic.get_OriginalBytes() == null)
                                {
                                    byte[] buffer = this.Memory.ReadBytes(this._iwuimeu, 10);
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
                                    Bagoic.set_OriginalBytes(this.Memory.ReadBytes(this._iwuimeu, 5));
                                    Bagoic.set_OriginalBytesDX(this.Memory.ReadBytes(this._wuoxuaxiatocaAvoawado, 5));
                                    if (Bagoic.get_OriginalBytesDX()[0] == 0x55)
                                    {
                                        Bagoic.set_OriginalBytesDX(this.Memory.ReadBytes(this._wuoxuaxiatocaAvoawado, 6));
                                    }
                                    else if (Bagoic.get_OriginalBytesDX()[0] == 0x6a)
                                    {
                                        Bagoic.set_OriginalBytesDX(this.Memory.ReadBytes(this._wuoxuaxiatocaAvoawado, 7));
                                    }
                                }
                                this._leoxafunihouxuEw = this.Memory.AllocateMemory(4);
                                this._pemequ = this.Memory.AllocateMemory(4);
                                this._itubuadiufaojSexexuo = this.Memory.AllocateMemory(4);
                                this._kuixaihutei = this.Memory.AllocateMemory(4);
                                this._apucuihauluiOq = this.Memory.AllocateMemory(4);
                                this._jokuowes = this.Memory.AllocateMemory(4);
                                this._obuodue = new byte[0x1000];
                                for (int i = 0; i < 0x1000; i++)
                                {
                                    this._obuodue[i] = 0;
                                }
                                this._iniugiam = this.Memory.AllocateMemory(this._obuodue.Length);
                                this.IjoufigehikueAbedai();
                                this.Deowet();
                                this.Apply();
                                this.ApplyDX();
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

        private uint CoxiafebobioUbaoxi()
        {
            try
            {
                return (nManager.Wow.Memory.WowProcess.WowModule + 0xac08e);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetJumpAdresse(): " + exception, true);
            }
            return 0;
        }

        private void Deowet()
        {
            this._onofaijoxoul = this.Memory.AllocateMemory(0x1000);
            Console.WriteLine("m_trampoline : " + this._onofaijoxoul.ToString("X"));
            ManagedFasm fasm = new ManagedFasm(this.Memory.ProcessHandle);
            fasm.SetMemorySize(0x1000);
            fasm.SetPassLimit(100);
            fasm.AddLine("pushad");
            fasm.AddLine("pushfd");
            fasm.AddLine("mov eax, [{0}]", new object[] { this._pemequ });
            fasm.AddLine("@execution:");
            fasm.AddLine("mov eax, [{0}]", new object[] { this._jokuowes });
            fasm.AddLine("test eax, eax");
            fasm.AddLine("je @lockcheck");
            fasm.AddLine("mov ebx, [{0}]", new object[] { nManager.Wow.Memory.WowProcess.WowModule + 0xd88f20 });
            fasm.AddLine("mov eax, [ebx+" + 0xe44 + "]");
            fasm.AddLine("mov esi, [ebx+" + 0xe48 + "]");
            fasm.AddLine("mov [" + this._kuixaihutei + "], esi");
            fasm.AddLine("mov [ebx+" + 0xe48 + "], eax");
            fasm.AddLine("call {0}", new object[] { this._iniugiam });
            fasm.AddLine("mov [" + this._itubuadiufaojSexexuo + "], eax");
            fasm.AddLine("mov edx, {0}", new object[] { nManager.Wow.Memory.WowProcess.WowModule + 0x341fb9 });
            fasm.AddLine("call " + (nManager.Wow.Memory.WowProcess.WowModule + 0x7415e7));
            fasm.AddLine("push happilyeverafter");
            fasm.AddLine("push " + (nManager.Wow.Memory.WowProcess.WowModule + 0x1a5de3));
            fasm.AddLine("jmp " + (nManager.Wow.Memory.WowProcess.WowModule + 0x550b85));
            fasm.AddLine("happilyeverafter:");
            fasm.AddLine("mov ebx, [{0}]", new object[] { nManager.Wow.Memory.WowProcess.WowModule + 0xd88f20 });
            fasm.AddLine("mov esi, [" + this._kuixaihutei + "]");
            fasm.AddLine("mov [ebx+" + 0xe48 + "], esi");
            fasm.AddLine("xor eax, eax");
            fasm.AddLine("mov [" + this._jokuowes + "], eax");
            fasm.AddLine("@lockcheck:");
            fasm.AddLine("mov eax, [{0}]", new object[] { this._pemequ });
            fasm.AddLine("test eax, eax");
            fasm.AddLine("jne @execution");
            fasm.AddLine("push 0");
            fasm.AddLine("add esp, 4");
            fasm.AddLine("popfd");
            fasm.AddLine("popad");
            this.Memory.WriteBytes(this._onofaijoxoul, Bagoic.get_OriginalBytesDX());
            fasm.AddLine("jmp " + (this._wuoxuaxiatocaAvoawado + Bagoic.get_OriginalBytesDX().Length));
            fasm.Inject(this._onofaijoxoul + ((uint) Bagoic.get_OriginalBytesDX().Length));
        }

        public void GameFrameLock()
        {
            lock (Locker)
            {
                if (nManagerSetting.CurrentSetting.UseFrameLock)
                {
                    this._ufainoxeugi++;
                    this.Memory.WriteByte(this._leoxafunihouxuEw, 1);
                }
            }
        }

        public void GameFrameUnLock()
        {
            lock (Locker)
            {
                if (this._ufainoxeugi > 0)
                {
                    this.Memory.WriteByte(this._leoxafunihouxuEw, 0);
                    this._ufainoxeugi = 0;
                }
                this._ufainoxeugi--;
                if (this._ufainoxeugi < 0)
                {
                    this._ufainoxeugi = 0;
                }
            }
        }

        private void IjoufigehikueAbedai()
        {
            this._vuseomaiheta = this.Memory.AllocateMemory(0x1000);
            Console.WriteLine("m_trampoline : " + this._vuseomaiheta.ToString("X"));
            ManagedFasm fasm = new ManagedFasm(this.Memory.ProcessHandle);
            fasm.SetMemorySize(0x1000);
            fasm.SetPassLimit(100);
            fasm.AddLine("call {0}", new object[] { nManager.Wow.Memory.WowProcess.WowModule + 0x189004 });
            fasm.AddLine("pushad");
            fasm.AddLine("pushfd");
            fasm.AddLine("mov eax, [{0}]", new object[] { this._leoxafunihouxuEw });
            fasm.AddLine("@execution:");
            fasm.AddLine("mov eax, [{0}]", new object[] { this._apucuihauluiOq });
            fasm.AddLine("test eax, eax");
            fasm.AddLine("je @lockcheck");
            fasm.AddLine("call {0}", new object[] { this._iniugiam });
            fasm.AddLine("mov [" + this._itubuadiufaojSexexuo + "], eax");
            fasm.AddLine("xor eax, eax");
            fasm.AddLine("mov [" + this._apucuihauluiOq + "], eax");
            fasm.AddLine("@lockcheck:");
            fasm.AddLine("mov eax, [{0}]", new object[] { this._leoxafunihouxuEw });
            fasm.AddLine("test eax, eax");
            fasm.AddLine("jne @execution");
            fasm.AddLine("popfd");
            fasm.AddLine("popad");
            fasm.AddLine("jmp " + (this._iwuimeu + Bagoic.get_OriginalBytes().Length));
            fasm.Inject(this._vuseomaiheta);
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
                fasm.Inject(this._iniugiam);
                this.Memory.WriteByte(this._apucuihauluiOq, 1);
                nManager.Helpful.Timer timer = new nManager.Helpful.Timer(2000.0);
                timer.Reset();
                while ((this.Memory.ReadByte(this._apucuihauluiOq) == 1) && !timer.IsReady)
                {
                    Thread.Sleep(1);
                }
                if (timer.IsReady)
                {
                    Logging.WriteError("Injection have been aborted, execution too long from " + CurrentCallStack, true);
                    return 0;
                }
                this.Memory.WriteBytes(this._iniugiam, this._obuodue);
                return this.Memory.ReadUInt(this._itubuadiufaojSexexuo);
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
                return ((magic.ReadInt(baseAddress + 0xd690f0) == 0) && (magic.ReadByte(baseAddress + 0xf3de96) > 0));
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsInGame(int processId): " + exception, true);
            }
            return false;
        }

        private uint OsoeboeneoqiBioqaceEjiuci()
        {
            try
            {
                if (Bagoic.OvaijGuir(this.Memory.ProcessId))
                {
                    return Bagoic.Uboeqe();
                }
                return Bagoic.Hatesojaitaive(this.Memory.ProcessId);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetJumpAdresse(): " + exception, true);
            }
            return 0;
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
                string str = magic.ReadUTF8String(baseAddress + 0x1020550);
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

        internal void Pujiegeamio()
        {
            try
            {
                if (this.Memory.IsProcessOpen)
                {
                    this._iwuimeu = this.CoxiafebobioUbaoxi();
                    this._wuoxuaxiatocaAvoawado = this.OsoeboeneoqiBioqaceEjiuci();
                    if (this.Memory.ReadByte(this._iwuimeu) == 0xe9)
                    {
                        lock (Locker)
                        {
                            if (Bagoic.get_OriginalBytesDX() == null)
                            {
                                Bagoic.set_OriginalBytesDX(this.Memory.ReadBytes(this._iwuimeu, 5));
                                byte[] buffer = new byte[5];
                                if (Bagoic.get_OriginalBytesDX() == buffer)
                                {
                                    Others.OpenWebBrowserOrApplication("http://thenoobbot.com/community/viewtopic.php?f=43&t=464");
                                    Logging.Write("An error is detected, you must switch the DirectX version used by your WoW client !");
                                    MessageBox.Show("An error is detected, you must switch the DirectX version used by your WoW client !");
                                    Pulsator.Dispose(true);
                                    return;
                                }
                                byte[] buffer2 = this.Memory.ReadBytes(this._wuoxuaxiatocaAvoawado, 9);
                                if ((buffer2[5] != 0x90) && (buffer2[6] != 0x90))
                                {
                                    Bagoic.set_OriginalBytesDX(new byte[] { 0x8b, 0xff, 0x55, 0x8b, 0xec });
                                }
                                else if ((buffer2[5] == 0x90) && (buffer2[6] != 0x90))
                                {
                                    Bagoic.set_OriginalBytesDX(new byte[] { 0x55, 0x8b, 0xec, 0x8b, 0x45, 8 });
                                }
                                else if ((buffer2[5] == 0x90) && (buffer2[6] == 0x90))
                                {
                                    Bagoic.set_OriginalBytesDX(new byte[] { 0x6a, 20, 0xb8, 12, 0x9a, 0x44, 0x73 });
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
                            Bagoic.set_OriginalBytes(new byte[] { 0xe8, 0x71, 0xcf, 13, 0 });
                            this.Remove(this._iwuimeu, Bagoic.get_OriginalBytes());
                            this.Remove(this._wuoxuaxiatocaAvoawado, Bagoic.get_OriginalBytesDX());
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposeHooking(): " + exception, true);
            }
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
                if (Bagoic.OvaijGuir(processId))
                {
                    dwAddress = Bagoic.Uboeqe();
                }
                else
                {
                    dwAddress = Bagoic.Hatesojaitaive(processId);
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

        public static string CurrentCallStack
        {
            get
            {
                string str = "";
                for (int i = 10; i >= 1; i--)
                {
                    StackFrame frame = new StackFrame(i);
                    if (frame.GetMethod() != null)
                    {
                        str = str + frame.GetMethod().Name + " => ";
                    }
                }
                return str.Substring(0, str.Length - 4);
            }
        }

        public bool IsGameFrameLocked
        {
            get
            {
                return (this.Memory.ReadByte(this._leoxafunihouxuEw) == 1);
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

