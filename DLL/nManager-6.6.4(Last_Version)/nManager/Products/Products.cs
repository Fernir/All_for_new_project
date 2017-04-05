namespace nManager.Products
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Bot.States;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.MemoryClass;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Products
    {
        private static Thread _aputieleraosi;
        private static bool _emeimiotoatute;
        private static bool _emonire;
        private static IProduct _ihuohod;
        private static Assembly _jataveipujumuUleivo;
        private static bool _oxekoifoeqini;
        private static string _panuka = "";
        private static bool _tiraiIx;
        private static bool _vafeqeidau;
        private static object _ximuvGox;
        private static readonly Engine Fsm = new Engine(false);

        public static  event IsAliveProductChangeEventHandler OnChangedIsAliveProduct;

        public static  event IsStartedChangeEventHandler OnChangedIsStarted;

        private static void Ajuga()
        {
            try
            {
                ProductStop();
                if (_ihuohod != null)
                {
                    _ihuohod.Dispose();
                }
                _ihuohod = null;
                _jataveipujumuUleivo = null;
                _ximuvGox = null;
            }
            catch (Exception exception)
            {
                Logging.WriteError("ThreadDisposeProduct(): " + exception, true);
                _ihuohod = null;
                _jataveipujumuUleivo = null;
                _ximuvGox = null;
            }
            _tiraiIx = true;
        }

        public static void DisposeProduct()
        {
            try
            {
                lock (typeof(nManager.Products.Products))
                {
                    Thread thread = new Thread(new ThreadStart(nManager.Products.Products.Ajuga)) {
                        Name = "Thread Dispose Product."
                    };
                    _tiraiIx = false;
                    thread.Start();
                    nManager.Helpful.Timer timer = new nManager.Helpful.Timer(2000.0);
                    while (!_tiraiIx && !timer.IsReady)
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposeProduct(): " + exception, true);
            }
        }

        private static void IvoanafIgel()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if ((_emeimiotoatute != IsStarted) && (_oxaimiukuReu != null))
                        {
                            _emeimiotoatute = IsStarted;
                            IsStartedChangeEventArgs e = new IsStartedChangeEventArgs {
                                IsStarted = IsStarted
                            };
                            _oxaimiukuReu(e);
                        }
                        if ((_oxekoifoeqini != IsAliveProduct) && (_hapeqeaw != null))
                        {
                            _oxekoifoeqini = IsAliveProduct;
                            IsAliveProductChangeEventArgs args3 = new IsAliveProductChangeEventArgs {
                                IsAliveProduct = IsAliveProduct
                            };
                            _hapeqeaw(args3);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("ThreadEventChangeProduct()#1: " + exception, true);
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception exception2)
            {
                Logging.WriteError("ThreadEventChangeProduct()#2: " + exception2, true);
            }
        }

        public static void LoadProducts(string nameDll)
        {
            try
            {
                lock (typeof(nManager.Products.Products))
                {
                    if (_aputieleraosi == null)
                    {
                        Thread thread = new Thread(new ThreadStart(nManager.Products.Products.IvoanafIgel)) {
                            Name = "ThreadEventChangeProduct"
                        };
                        _aputieleraosi = thread;
                        _aputieleraosi.Start();
                    }
                    Dofica(nameDll);
                    _ihuohod = null;
                    _jataveipujumuUleivo = null;
                    _ximuvGox = null;
                    _jataveipujumuUleivo = Assembly.LoadFrom(Application.StartupPath + @"\Products\" + nameDll + ".dll");
                    _ximuvGox = _jataveipujumuUleivo.CreateInstance("Main", false);
                    _ihuohod = _ximuvGox as IProduct;
                    if (_ihuohod != null)
                    {
                        _ihuohod.Initialize();
                    }
                }
            }
            catch (Exception exception)
            {
                Dofica("");
                MessageBox.Show(string.Format("Exception: {0}", exception));
                Logging.WriteError("LoadProducts(string nameDll): " + exception, true);
                DisposeProduct();
                _ihuohod = null;
                _jataveipujumuUleivo = null;
                _ximuvGox = null;
            }
        }

        public static IProduct LoadProductsWithoutInit(string nameDll)
        {
            try
            {
                return (Assembly.LoadFrom(Others.GetCurrentDirectory + @"\Products\" + nameDll + ".dll").CreateInstance("Main", true) as IProduct);
            }
            catch (Exception exception)
            {
                Dofica("");
                MessageBox.Show(string.Format("Exception: {0}", exception));
                Logging.WriteError("LoadProductsWithoutInit(string nameDll)(string nameDll): " + exception, true);
            }
            return null;
        }

        public static bool ProductRemoteStart(string[] args)
        {
            try
            {
                if (_ihuohod != null)
                {
                    _vafeqeidau = false;
                    _emonire = false;
                    TravelToContinentId = 0x98967f;
                    TravelFromContinentId = 0x98967f;
                    TravelTo = new Point();
                    TravelFrom = new Point();
                    ForceTravel = false;
                    nManagerSetting.ActivateProductTipOff = false;
                    _ihuohod.RemoteStart(args);
                    if (!_ihuohod.IsStarted)
                    {
                        return false;
                    }
                    EventsListener.HookEvent(WoWEventsType.LOOT_READY, callback => FarmingTask.TakeFarmingLoots(), false, false);
                    EventsListener.HookEvent(WoWEventsType.LOOT_OPENED, callback => FarmingTask.TakeFarmingLoots(), false, false);
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_START, callback => ToggleCinematic(true), false, false);
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_STOP, callback => ToggleCinematic(false), false, false);
                    Statistics.Reset();
                    Fsm.States.Clear();
                    Unaxakoawemeu tempState = new Unaxakoawemeu {
                        Priority = 10
                    };
                    Fsm.AddState(tempState);
                    Duogiowueqov duogiowueqov = new Duogiowueqov {
                        Priority = 5
                    };
                    Fsm.AddState(duogiowueqov);
                    Idle idle = new Idle {
                        Priority = 1
                    };
                    Fsm.AddState(idle);
                    Fsm.States.Sort();
                    Fsm.StartEngine(1, "FSM nManager");
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ProductRemoteStart(): " + exception, true);
            }
            return false;
        }

        public static bool ProductRestart()
        {
            try
            {
                ProductStop();
                ProductStart();
            }
            catch (Exception exception)
            {
                Logging.WriteError("ProductRestart(): " + exception, true);
            }
            return false;
        }

        public static bool ProductSettings()
        {
            try
            {
                if (_ihuohod != null)
                {
                    _ihuohod.Settings();
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ProductSettings(): " + exception, true);
            }
            return false;
        }

        public static bool ProductStart()
        {
            try
            {
                if (_ihuohod != null)
                {
                    _vafeqeidau = false;
                    _emonire = false;
                    TravelToContinentId = 0x98967f;
                    TravelFromContinentId = 0x98967f;
                    TravelTo = new Point();
                    TravelFrom = new Point();
                    ForceTravel = false;
                    _ihuohod.Start();
                    if (!_ihuohod.IsStarted)
                    {
                        return false;
                    }
                    EventsListener.HookEvent(WoWEventsType.LOOT_READY, callback => FarmingTask.TakeFarmingLoots(), false, false);
                    EventsListener.HookEvent(WoWEventsType.LOOT_OPENED, callback => FarmingTask.TakeFarmingLoots(), false, false);
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_START, callback => ToggleCinematic(true), false, false);
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_STOP, callback => ToggleCinematic(false), false, false);
                    Statistics.Reset();
                    Fsm.States.Clear();
                    Unaxakoawemeu tempState = new Unaxakoawemeu {
                        Priority = 10
                    };
                    Fsm.AddState(tempState);
                    Duogiowueqov duogiowueqov = new Duogiowueqov {
                        Priority = 5
                    };
                    Fsm.AddState(duogiowueqov);
                    Idle idle = new Idle {
                        Priority = 1
                    };
                    Fsm.AddState(idle);
                    Fsm.States.Sort();
                    Fsm.StartEngine(1, "FSM nManager");
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ProductStart(): " + exception, true);
            }
            return false;
        }

        public static bool ProductStop()
        {
            try
            {
                Fsm.StopEngine();
                if (_ihuohod != null)
                {
                    EventsListener.UnHookEvent(WoWEventsType.LOOT_READY, callback => FarmingTask.TakeFarmingLoots(), false);
                    EventsListener.UnHookEvent(WoWEventsType.LOOT_OPENED, callback => FarmingTask.TakeFarmingLoots(), false);
                    EventsListener.UnHookEvent(WoWEventsType.CINEMATIC_START, callback => ToggleCinematic(true), false);
                    EventsListener.UnHookEvent(WoWEventsType.CINEMATIC_STOP, callback => ToggleCinematic(false), false);
                    _ihuohod.Stop();
                    Thread.Sleep(500);
                    MovementManager.StopMove();
                    Fight.StopFight();
                    CombatClass.DisposeCombatClass();
                    LongMove.StopLongMove();
                    Memory.WowMemory.GameFrameUnLock();
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ProductStop(): " + exception, true);
            }
            return false;
        }

        public static void ProductStopFromFSM()
        {
            ProductStop();
        }

        public static void ToggleCinematic(bool started = true)
        {
            if (started)
            {
                Lua.LuaDoString("StopCinematic()", false, true);
                Thread.Sleep(0x3e8);
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = tostring(InCinematic())", false, true);
                if (Lua.GetLocalizedText(randomString) == "false")
                {
                    return;
                }
            }
            _vafeqeidau = started;
        }

        public static bool ForceTravel
        {
            [CompilerGenerated]
            get
            {
                return <ForceTravel>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ForceTravel>k__BackingField = value;
            }
        }

        public static bool InAutoPause
        {
            get
            {
                return _vafeqeidau;
            }
            set
            {
                _vafeqeidau = value;
                Logging.WriteFileOnly("Paused from: " + Hook.CurrentCallStack);
            }
        }

        public static bool InManualPause
        {
            get
            {
                return _emonire;
            }
            set
            {
                _emonire = value;
                Logging.WriteFileOnly("Paused from: " + Hook.CurrentCallStack);
            }
        }

        public static bool IsAliveProduct
        {
            get
            {
                return (_ihuohod != null);
            }
        }

        public static bool IsStarted
        {
            get
            {
                try
                {
                    if (_ihuohod != null)
                    {
                        return _ihuohod.IsStarted;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Products > IsStarted: " + exception, true);
                }
                return false;
            }
        }

        public static string ProductName
        {
            get
            {
                return _panuka;
            }
            private set
            {
                _panuka = value;
            }
        }

        public static Func<Point, bool> TargetValidationFct
        {
            [CompilerGenerated]
            get
            {
                return <TargetValidationFct>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <TargetValidationFct>k__BackingField = value;
            }
        }

        public static Point TravelFrom
        {
            [CompilerGenerated]
            get
            {
                return <TravelFrom>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <TravelFrom>k__BackingField = value;
            }
        }

        public static int TravelFromContinentId
        {
            [CompilerGenerated]
            get
            {
                return <TravelFromContinentId>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <TravelFromContinentId>k__BackingField = value;
            }
        }

        public static Point TravelTo
        {
            [CompilerGenerated]
            get
            {
                return <TravelTo>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <TravelTo>k__BackingField = value;
            }
        }

        public static int TravelToContinentId
        {
            [CompilerGenerated]
            get
            {
                return <TravelToContinentId>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <TravelToContinentId>k__BackingField = value;
            }
        }

        public class IsAliveProductChangeEventArgs : EventArgs
        {
            public bool IsAliveProduct { get; set; }
        }

        public delegate void IsAliveProductChangeEventHandler(nManager.Products.Products.IsAliveProductChangeEventArgs e);

        public class IsStartedChangeEventArgs : EventArgs
        {
            public bool IsStarted { get; set; }
        }

        public delegate void IsStartedChangeEventHandler(nManager.Products.Products.IsStartedChangeEventArgs e);
    }
}

