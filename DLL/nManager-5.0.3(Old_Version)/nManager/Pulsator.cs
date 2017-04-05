namespace nManager
{
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow;
    using nManager.Wow.Helpers;
    using nManager.Wow.MemoryClass;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Pulsator
    {
        private static bool _isDisposed;
        private static string _tempSecretKey = "";
        private const string MD5Key = "5c9c3ef2f438f88b0aeea7dd152833b4";

        public static void Dispose(bool closePocess = false)
        {
            try
            {
                lock (typeof(nManager.Pulsator))
                {
                    Thread thread = new Thread(new ThreadStart(nManager.Pulsator.ThreadDispose)) {
                        Name = "Thread Dispose nManager."
                    };
                    _isDisposed = false;
                    thread.Start();
                    nManager.Helpful.Timer timer = new nManager.Helpful.Timer(3000.0);
                    while (!_isDisposed && !timer.IsReady)
                    {
                        Thread.Sleep(10);
                    }
                    if (closePocess)
                    {
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                }
            }
            catch (Exception exception)
            {
                try
                {
                    if (closePocess)
                    {
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                }
                catch
                {
                }
                Logging.WriteError("nManager > Pulstator > Dispose(): " + exception, true);
            }
        }

        public static void Pulse(int processId, string secretKey)
        {
            try
            {
                _tempSecretKey = Others.EncrypterMD5(secretKey);
                if ("5c9c3ef2f438f88b0aeea7dd152833b4" == _tempSecretKey)
                {
                    SpellManager.SpellListManager.LoadSpellList(Application.StartupPath + @"\Data\spell.txt");
                    Memory.WowProcess = new nManager.Wow.MemoryClass.Process(processId);
                    Memory.WowMemory = new Hook();
                    if (nManager.Wow.Helpers.Usefuls.WowVersion("") == 0x54ee)
                    {
                        nManager.Wow.ObjectManager.Pulsator.Initialize(true);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("nManager > Pulstator > Pulse(int processId, string secretKey): " + exception, true);
            }
        }

        public static void Reset()
        {
            try
            {
                int processId = Memory.WowProcess.ProcessId;
                Dispose(false);
                Pulse(processId, _tempSecretKey);
            }
            catch (Exception exception)
            {
                Logging.WriteError("nManager > Pulstator > Reset(): " + exception, true);
            }
        }

        private static void ThreadDispose()
        {
            try
            {
                nManager.Products.Products.DisposeProduct();
                nManager.Wow.ObjectManager.Pulsator.Shutdown();
                Memory.WowMemory.AllowReHook = false;
                Memory.WowMemory.DisposeHooking();
                Memory.WowProcess = new nManager.Wow.MemoryClass.Process();
                try
                {
                    nManager.Helpful.Timer timer = new nManager.Helpful.Timer(2000.0);
                    while ((Logging.CountNumberInQueue > 0) && !timer.IsReady)
                    {
                        Thread.Sleep(10);
                    }
                }
                catch
                {
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("nManager > Pulstator > ThreadDispose(): " + exception, true);
            }
            _isDisposed = true;
        }

        public static bool IsActive
        {
            get
            {
                try
                {
                    if (Memory.WowProcess.ProcessId <= 0)
                    {
                        return false;
                    }
                    if (!Memory.WowMemory.ThreadHooked)
                    {
                        return false;
                    }
                    if (!nManager.Wow.ObjectManager.Pulsator.IsActive)
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("nManager > Pulstator > IsActive: " + exception, true);
                }
                return false;
            }
        }
    }
}

