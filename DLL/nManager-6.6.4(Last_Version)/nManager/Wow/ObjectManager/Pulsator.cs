namespace nManager.Wow.ObjectManager
{
    using nManager;
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Helpers;
    using System;
    using System.Collections.Concurrent;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class Pulsator
    {
        private static Thread _moaviPoaweam;

        public static void Initialize(bool useThread = true)
        {
            try
            {
                if (useThread)
                {
                    if (_moaviPoaweam != null)
                    {
                        Shutdown();
                    }
                    Thread thread = new Thread(new ThreadStart(nManager.Wow.ObjectManager.Pulsator.Pulse)) {
                        IsBackground = true,
                        Name = "ObjectManager"
                    };
                    _moaviPoaweam = thread;
                    _moaviPoaweam.Start();
                }
                else
                {
                    Pulse();
                }
                Thread.Sleep(300);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Pulsator > Initialize(bool useThread = true): " + exception, true);
            }
        }

        public static void Pulse()
        {
            try
            {
                if (_moaviPoaweam == null)
                {
                    nManager.Wow.ObjectManager.ObjectManager.Foamei();
                }
                else
                {
                    while (true)
                    {
                        if ((Memory.WowMemory.ThreadHooked && Memory.WowMemory.Memory.IsProcessOpen) && (Usefuls.InGame || nManagerSetting.AutoStartLoggingInfoProvided))
                        {
                            nManager.Wow.ObjectManager.ObjectManager.Foamei();
                            if ((!nManagerSetting.AutoStartLoggingInfoProvided && Usefuls.InGame) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)
                            {
                                nManager.Pulsator.Reset();
                            }
                        }
                        else
                        {
                            nManager.Wow.ObjectManager.ObjectManager.ObjectDictionary = new ConcurrentDictionary<UInt128, WoWObject>();
                        }
                        Thread.Sleep(650);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Pulsator > Pulse(): " + exception, true);
                _moaviPoaweam = null;
            }
        }

        public static void Shutdown()
        {
            try
            {
                if (_moaviPoaweam != null)
                {
                    _moaviPoaweam.Abort();
                    while ((_moaviPoaweam != null) && _moaviPoaweam.IsAlive)
                    {
                    }
                    _moaviPoaweam = null;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Pulsator > Shutdown(): " + exception, true);
                _moaviPoaweam = null;
            }
        }

        public static bool IsActive
        {
            get
            {
                try
                {
                    return (_moaviPoaweam != null);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Pulsator > IsActive: " + exception, true);
                }
                return false;
            }
        }
    }
}

