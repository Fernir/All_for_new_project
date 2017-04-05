namespace nManager.Wow.MemoryClass
{
    using nManager.Helpful;
    using nManager.Helpful.Win32;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Process
    {
        private int _processId;
        internal static List<int> InjectionCount = new List<int>();

        public Process()
        {
            this.ProcessId = 0;
        }

        public Process(int processId)
        {
            this.ProcessId = processId;
            this.OpenProcess();
        }

        public void CloseProcessHandle()
        {
            try
            {
                Native.CloseHandle(this.ProcessHandle);
                System.Diagnostics.Process.LeaveDebugMode();
                this.ProcessHandle = IntPtr.Zero;
                this.MainWindowHandle = IntPtr.Zero;
                this.ProcessId = (int) IntPtr.Zero;
            }
            catch (Exception exception)
            {
                Logging.WriteError("CloseProcessHandle(): " + exception, true);
            }
        }

        public static int GetInjectionBySec()
        {
            try
            {
                for (int i = InjectionCount.Count - 1; i >= 0; i--)
                {
                    if (InjectionCount[i] < (Others.Times - 0x3e8))
                    {
                        InjectionCount.RemoveAt(i);
                    }
                }
                return InjectionCount.Count;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetInjectionByMin(): " + exception, true);
                return 0;
            }
        }

        public uint GetModule(string moduleName)
        {
            try
            {
                ProcessModuleCollection modules = System.Diagnostics.Process.GetProcessById(this.ProcessId).Modules;
                for (int i = 0; i < modules.Count; i++)
                {
                    if (modules[i].ModuleName.ToLower() == moduleName.ToLower())
                    {
                        return (uint) ((int) modules[i].BaseAddress);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetModule(string moduleName): " + exception, true);
            }
            return 0;
        }

        public void KillWowProcess()
        {
            try
            {
                System.Diagnostics.Process.GetProcessById(this.ProcessId).Kill();
            }
            catch (Exception exception)
            {
                Logging.WriteError("KillWowProcess(): " + exception, true);
            }
        }

        public static System.Diagnostics.Process[] ListeProcessIdByName(string processName)
        {
            try
            {
                return System.Diagnostics.Process.GetProcessesByName(processName);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ListeProcessIdByName(string processName = \"Wow\"): " + exception, true);
            }
            return new System.Diagnostics.Process[0];
        }

        public IntPtr OpenProcess()
        {
            try
            {
                System.Diagnostics.Process.EnterDebugMode();
                this.ProcessHandle = Native.OpenProcess(0x1f0fff, false, this.ProcessId);
                System.Diagnostics.Process processById = System.Diagnostics.Process.GetProcessById(this.ProcessId);
                this.MainWindowHandle = processById.MainWindowHandle;
                this.WowModule = this.GetModule(processById.ProcessName + ".exe");
                return this.ProcessHandle;
            }
            catch (Exception exception)
            {
                Logging.WriteError("OpenProcess(): " + exception, true);
            }
            return IntPtr.Zero;
        }

        public bool ProcessExist()
        {
            try
            {
                return (System.Diagnostics.Process.GetProcessById(this.ProcessId).Id == this.ProcessId);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ProcessExist(): " + exception, true);
                return false;
            }
        }

        public IntPtr MainWindowHandle { get; internal set; }

        public IntPtr ProcessHandle { get; set; }

        public int ProcessId
        {
            get
            {
                return this._processId;
            }
            set
            {
                this._processId = value;
            }
        }

        public uint WowModule { get; internal set; }
    }
}

