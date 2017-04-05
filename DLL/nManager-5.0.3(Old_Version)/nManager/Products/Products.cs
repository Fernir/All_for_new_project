namespace nManager.Products
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Bot.States;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Products
    {
        private static Assembly _assembly;
        private static bool _inAutoPause;
        private static bool _inManualPause;
        private static IProduct _instanceFromOtherAssembly;
        private static bool _isDisposed;
        private static object _obj;
        private static bool _oldIsAliveProduc;
        private static bool _oldIsStarted;
        private static string _productName = "";
        private static Thread _threadEventChangeProduct;
        private static readonly Engine Fsm = new Engine(false);

        public static  event IsAliveProductChangeEventHandler OnChangedIsAliveProduct;

        public static  event IsStartedChangeEventHandler OnChangedIsStarted;

        public static void DisposeProduct()
        {
            try
            {
                lock (typeof(nManager.Products.Products))
                {
                    Thread thread = new Thread(new ThreadStart(nManager.Products.Products.ThreadDisposeProduct)) {
                        Name = "Thread Dispose Product."
                    };
                    _isDisposed = false;
                    thread.Start();
                    nManager.Helpful.Timer timer = new nManager.Helpful.Timer(2000.0);
                    while (!_isDisposed && !timer.IsReady)
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

        public static void LoadProducts(string nameDll)
        {
            try
            {
                lock (typeof(nManager.Products.Products))
                {
                    if (_threadEventChangeProduct == null)
                    {
                        Thread thread = new Thread(new ThreadStart(nManager.Products.Products.ThreadEventChangeProduct)) {
                            Name = "ThreadEventChangeProduct"
                        };
                        _threadEventChangeProduct = thread;
                        _threadEventChangeProduct.Start();
                    }
                    ProductName = nameDll;
                    _instanceFromOtherAssembly = null;
                    _assembly = null;
                    _obj = null;
                    _assembly = Assembly.LoadFrom(Application.StartupPath + @"\Products\" + nameDll + ".dll");
                    _obj = _assembly.CreateInstance("Main", false);
                    _instanceFromOtherAssembly = _obj as IProduct;
                    if (_instanceFromOtherAssembly != null)
                    {
                        _instanceFromOtherAssembly.Initialize();
                    }
                }
            }
            catch (Exception exception)
            {
                ProductName = "";
                MessageBox.Show(string.Format("Exception: {0}", exception));
                Logging.WriteError("LoadProducts(string nameDll): " + exception, true);
                DisposeProduct();
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
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
                ProductName = "";
                MessageBox.Show(string.Format("Exception: {0}", exception));
                Logging.WriteError("LoadProductsWithoutInit(string nameDll)(string nameDll): " + exception, true);
            }
            return null;
        }

        public static bool ProductRemoteStart(string[] args)
        {
            try
            {
                if (_instanceFromOtherAssembly != null)
                {
                    _inAutoPause = false;
                    _inManualPause = false;
                    TravelToContinentId = 0x98967f;
                    TravelTo = new Point();
                    nManagerSetting.ActivateProductTipOff = false;
                    _instanceFromOtherAssembly.RemoteStart(args);
                    if (!_instanceFromOtherAssembly.IsStarted)
                    {
                        return false;
                    }
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_START, callback => ToggleCinematic(true), false, false);
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_STOP, callback => ToggleCinematic(false), false, false);
                    Statistics.Reset();
                    Fsm.States.Clear();
                    Relogger tempState = new Relogger {
                        Priority = 10
                    };
                    Fsm.AddState(tempState);
                    StopBotIf @if = new StopBotIf {
                        Priority = 5
                    };
                    Fsm.AddState(@if);
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
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Settings();
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
                if (_instanceFromOtherAssembly != null)
                {
                    _inAutoPause = false;
                    _inManualPause = false;
                    TravelToContinentId = 0x98967f;
                    TravelTo = new Point();
                    _instanceFromOtherAssembly.Start();
                    if (!_instanceFromOtherAssembly.IsStarted)
                    {
                        return false;
                    }
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_START, callback => ToggleCinematic(true), false, false);
                    EventsListener.HookEvent(WoWEventsType.CINEMATIC_STOP, callback => ToggleCinematic(false), false, false);
                    Statistics.Reset();
                    Fsm.States.Clear();
                    Relogger tempState = new Relogger {
                        Priority = 10
                    };
                    Fsm.AddState(tempState);
                    StopBotIf @if = new StopBotIf {
                        Priority = 5
                    };
                    Fsm.AddState(@if);
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
                if (_instanceFromOtherAssembly != null)
                {
                    EventsListener.UnHookEvent(WoWEventsType.CINEMATIC_START, callback => ToggleCinematic(true), false);
                    EventsListener.UnHookEvent(WoWEventsType.CINEMATIC_STOP, callback => ToggleCinematic(false), false);
                    _instanceFromOtherAssembly.Stop();
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

        private static void ThreadDisposeProduct()
        {
            try
            {
                ProductStop();
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Dispose();
                }
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
            catch (Exception exception)
            {
                Logging.WriteError("ThreadDisposeProduct(): " + exception, true);
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
            _isDisposed = true;
        }

        private static void ThreadEventChangeProduct()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if ((_oldIsStarted != IsStarted) && (OnChangedIsStarted != null))
                        {
                            _oldIsStarted = IsStarted;
                            IsStartedChangeEventArgs e = new IsStartedChangeEventArgs {
                                IsStarted = IsStarted
                            };
                            OnChangedIsStarted(e);
                        }
                        if ((_oldIsAliveProduc != IsAliveProduct) && (OnChangedIsAliveProduct != null))
                        {
                            _oldIsAliveProduc = IsAliveProduct;
                            IsAliveProductChangeEventArgs args3 = new IsAliveProductChangeEventArgs {
                                IsAliveProduct = IsAliveProduct
                            };
                            OnChangedIsAliveProduct(args3);
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

        public static void ToggleCinematic(bool started = true)
        {
            _inAutoPause = started;
        }

        public static bool InAutoPause
        {
            get
            {
                return _inAutoPause;
            }
            set
            {
                _inAutoPause = value;
            }
        }

        public static bool InManualPause
        {
            get
            {
                return _inManualPause;
            }
            set
            {
                _inManualPause = value;
            }
        }

        public static bool IsAliveProduct
        {
            get
            {
                return (_instanceFromOtherAssembly != null);
            }
        }

        public static bool IsStarted
        {
            get
            {
                try
                {
                    if (_instanceFromOtherAssembly != null)
                    {
                        return _instanceFromOtherAssembly.IsStarted;
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
                return _productName;
            }
            private set
            {
                _productName = value;
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

