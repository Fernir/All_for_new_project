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
        private static bool _automaticallyUseDraenorSecondaryBait;
        private static bool _fishBotLaunched;
        private static string _fishingPoleName = "";
        private static UInt128 _guidNode;
        private static string _lureName = "";
        private static bool _precision;
        private static bool _useLure;
        private static Thread _worker2;
        private const float distanceBobber = 4f;
        private static Spell fishingSpell;

        public static void LoopFish(UInt128 guidNode = new UInt128(), bool useLure = false, string lureName = "", string fishingPoleName = "", bool precision = false, bool automaticallyUseDraenorSecondaryBait = true)
        {
            try
            {
                lock (typeof(FishingTask))
                {
                    _guidNode = guidNode;
                    _precision = precision;
                    _useLure = useLure;
                    _lureName = lureName;
                    _fishingPoleName = fishingPoleName;
                    _automaticallyUseDraenorSecondaryBait = automaticallyUseDraenorSecondaryBait;
                    Fishing.ReCheckFishingPoleTimer.ForceReady();
                    if (_worker2 == null)
                    {
                        Thread thread = new Thread(new ThreadStart(FishingTask.LoopFishThread)) {
                            Name = "Fish"
                        };
                        _worker2 = thread;
                        _worker2.Start();
                    }
                    _fishBotLaunched = true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FishingTask > LoopFish(UInt128 guidNode = 0, bool useLure = false, string lureName = \"\", bool precision = false): " + exception, true);
            }
        }

        private static void LoopFishThread()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        while (_fishBotLaunched)
                        {
                            Fishing.EquipFishingPoles(_fishingPoleName);
                            if (_useLure)
                            {
                                Fishing.UseLure(_lureName, _automaticallyUseDraenorSecondaryBait);
                            }
                            if (fishingSpell == null)
                            {
                                fishingSpell = new Spell("Fishing");
                            }
                            fishingSpell.Launch(false, false, true, null);
                            Thread.Sleep(0xfa0);
                            WoWGameObject obj2 = new WoWGameObject(Fishing.SearchBobber());
                            if (obj2.IsValid)
                            {
                                WoWGameObject obj3 = new WoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(_guidNode).GetBaseAddress);
                                if (((obj3.Position.DistanceTo2D(obj2.Position) <= 4f) || !obj3.IsValid) || ((_guidNode <= 0) || !_precision))
                                {
                                    goto Label_00C6;
                                }
                            }
                            continue;
                        Label_00BC:
                            Thread.Sleep(250);
                        Label_00C6:
                            if ((_fishBotLaunched && nManager.Wow.ObjectManager.ObjectManager.Me.IsCast) && (obj2.IsValid && (1 != Memory.WowMemory.Memory.ReadShort(obj2.GetBaseAddress + 260))))
                            {
                                goto Label_00BC;
                            }
                            if ((_fishBotLaunched && nManager.Wow.ObjectManager.ObjectManager.Me.IsCast) && obj2.IsValid)
                            {
                                Interact.InteractWith(obj2.GetBaseAddress, false);
                                Statistics.Farms++;
                                Thread.Sleep(0x3e8);
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

        public static void StopLoopFish()
        {
            try
            {
                lock (typeof(FishingTask))
                {
                    _guidNode = 0;
                    _fishBotLaunched = false;
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
                return _fishBotLaunched;
            }
        }
    }
}

