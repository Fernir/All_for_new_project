namespace nManager.Wow.Bot.Tasks
{
    using nManager;
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class FishingTask
    {
        private static Thread _afasonuemFojeqo;
        private static string _doewua = "";
        private static bool _eheunohUbas;
        private const float _horaoneIfawexuk = 4f;
        private static Spell _ikaheAq;
        private static bool _itaociwuitTao;
        private static string _itesereriusoho = "";
        private static bool _jameadiepuarenEfefo;
        public static int _lastSuccessfullFishing;
        private static UInt128 _omaokuikakaeve;
        private static bool _uladujuabaob;

        private static void Cegueduaj()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        while (_eheunohUbas)
                        {
                            WoWGameObject obj2;
                            if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                            {
                                StopLoopFish();
                            }
                            else
                            {
                                Fishing.EquipFishingPoles(_itesereriusoho);
                                if (_jameadiepuarenEfefo)
                                {
                                    Fishing.UseLure(_doewua, _itaociwuitTao);
                                }
                                if (_ikaheAq == null)
                                {
                                    _ikaheAq = new Spell("Fishing");
                                }
                                _ikaheAq.Launch(false, false, true, null);
                                Thread.Sleep(0xfa0);
                                obj2 = new WoWGameObject(Fishing.SearchBobber());
                                if (obj2.IsValid)
                                {
                                    WoWGameObject obj3 = new WoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(_omaokuikakaeve).GetBaseAddress);
                                    if (((obj3.Position.DistanceTo2D(obj2.Position) <= 4f) || !obj3.IsValid) || ((_omaokuikakaeve <= 0) || !_uladujuabaob))
                                    {
                                        goto Label_00DC;
                                    }
                                }
                            }
                            continue;
                        Label_00D2:
                            Thread.Sleep(250);
                        Label_00DC:
                            if ((_eheunohUbas && nManager.Wow.ObjectManager.ObjectManager.Me.IsCast) && (obj2.IsValid && (1 != Memory.WowMemory.Memory.ReadShort(obj2.GetBaseAddress + 0xf8))))
                            {
                                goto Label_00D2;
                            }
                            if ((_eheunohUbas && nManager.Wow.ObjectManager.ObjectManager.Me.IsCast) && obj2.IsValid)
                            {
                                FarmingTask.CountThisLoot = true;
                                FarmingTask.NodeOrUnit = true;
                                Interact.InteractWith(obj2.GetBaseAddress, false);
                                _lastSuccessfullFishing = Environment.TickCount;
                                Statistics.Farms++;
                                Thread.Sleep((int) (Usefuls.Latency + 400));
                                for (int i = 0; i < 10; i++)
                                {
                                    if (!Others.IsFrameVisible("LootFrame"))
                                    {
                                        continue;
                                    }
                                    Thread.Sleep(150);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(400);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FishingTask > LoopFishThread(): " + exception, true);
            }
        }

        public static void LoopFish(UInt128 guidNode = new UInt128(), bool useLure = false, string lureName = "", string fishingPoleName = "", bool precision = false, bool automaticallyUseDraenorSecondaryBait = true)
        {
            try
            {
                lock (typeof(FishingTask))
                {
                    _omaokuikakaeve = guidNode;
                    _uladujuabaob = precision;
                    _jameadiepuarenEfefo = useLure;
                    _doewua = lureName;
                    _itesereriusoho = fishingPoleName;
                    _itaociwuitTao = automaticallyUseDraenorSecondaryBait;
                    Fishing.ReCheckFishingPoleTimer.ForceReady();
                    if (_afasonuemFojeqo == null)
                    {
                        Thread thread = new Thread(new ThreadStart(FishingTask.Cegueduaj)) {
                            Name = "Fish"
                        };
                        _afasonuemFojeqo = thread;
                        _afasonuemFojeqo.Start();
                    }
                    _eheunohUbas = true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FishingTask > LoopFish(UInt128 guidNode = 0, bool useLure = false, string lureName = \"\", bool precision = false): " + exception, true);
            }
        }

        public static void StopLoopFish()
        {
            try
            {
                lock (typeof(FishingTask))
                {
                    _omaokuikakaeve = 0;
                    _lastSuccessfullFishing = 0;
                    _eheunohUbas = false;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FishingTask > StopLoopFish(): " + exception, true);
            }
        }

        public static bool IsLaunched
        {
            get
            {
                return _eheunohUbas;
            }
        }
    }
}

