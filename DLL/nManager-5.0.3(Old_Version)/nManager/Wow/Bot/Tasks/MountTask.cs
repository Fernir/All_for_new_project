namespace nManager.Wow.Bot.Tasks
{
    using nManager;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class MountTask
    {
        private static string _aquaMount;
        private static bool _coldWeather;
        private static bool _dreaneorFly;
        private static bool _flightMasterLicense;
        private static string _flyMount;
        private static string _groundMount;
        private static string _localizedAbysalMountName = string.Empty;
        private static int _nbTry;
        private static int _noMountsInSettings;
        private static Spell _spellAquaMount = new Spell(0);
        private static Spell _spellFlyMount = new Spell(0);
        private static Spell _spellGroundMount = new Spell(0);
        private static bool _startupCheck = true;
        private static bool _wisdom4Winds;
        private static nManager.Helpful.Timer dismountTimer = new nManager.Helpful.Timer(0.0);
        public static bool SettingsHasChanged;
        private static int tryMounting;

        public static void DismountMount(bool stopMove = true)
        {
            try
            {
                if (nManager.Products.Products.IsStarted)
                {
                    if (stopMove)
                    {
                        MovementManager.StopMove();
                    }
                    else
                    {
                        MovementManager.StopMoveTo(true, false);
                    }
                    Thread.Sleep(200);
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                    {
                        Logging.Write("Dismount in progress.");
                        bool isFlying = Usefuls.IsFlying;
                        if (isFlying)
                        {
                            Land(false);
                        }
                        Usefuls.DisMount();
                        if (isFlying)
                        {
                            dismountTimer = new nManager.Helpful.Timer(5000.0);
                        }
                        Thread.Sleep((int) (200 + Usefuls.Latency));
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("MountTask > DismountMount(): " + exception, true);
            }
        }

        public static MountCapacity GetMountCapacity()
        {
            if (_startupCheck || SettingsHasChanged)
            {
                _aquaMount = nManagerSetting.CurrentSetting.AquaticMountName;
                _groundMount = nManagerSetting.CurrentSetting.GroundMountName;
                _flyMount = nManagerSetting.CurrentSetting.FlyingMountName;
                if (!string.IsNullOrEmpty(_aquaMount.Trim()))
                {
                    _spellAquaMount = new Spell(_aquaMount);
                }
                if (!string.IsNullOrEmpty(_groundMount.Trim()))
                {
                    _spellGroundMount = new Spell(_groundMount);
                }
                if (!string.IsNullOrEmpty(_flyMount.Trim()))
                {
                    _spellFlyMount = new Spell(_flyMount);
                }
                if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x10) && (_groundMount != string.Empty)) && (nManagerSetting.CurrentSetting.UseGroundMount && !_spellGroundMount.KnownSpell))
                {
                    Others.ShowMessageBox(Translate.Get(Translate.Id.ThisGroundMountDoesNotExist) + _groundMount, "");
                    _groundMount = string.Empty;
                }
                if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x12) && (_aquaMount != string.Empty)) && !_spellAquaMount.KnownSpell)
                {
                    Others.ShowMessageBox(Translate.Get(Translate.Id.ThisAquaticMountDoesNotExist) + _aquaMount, "");
                    _aquaMount = string.Empty;
                }
                if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x3a) && (_flyMount != string.Empty)) && !_spellFlyMount.KnownSpell)
                {
                    Others.ShowMessageBox(Translate.Get(Translate.Id.ThisFlyingMountDoesNotExist) + _flyMount, "");
                    _flyMount = string.Empty;
                }
                if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 60) && (_aquaMount != string.Empty)) && (_localizedAbysalMountName == string.Empty))
                {
                    _localizedAbysalMountName = SpellManager.GetSpellInfo(0x125c7).Name;
                }
                Spell spell = new Spell(0x1c4c9);
                _wisdom4Winds = spell.KnownSpell;
                Spell spell2 = new Spell(0xd3b5);
                _coldWeather = spell2.KnownSpell;
                Spell spell3 = new Spell(0x1609b);
                _flightMasterLicense = spell3.KnownSpell;
                _dreaneorFly = (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 90) && Usefuls.IsCompletedAchievement(0x2722, false);
                _startupCheck = false;
                SettingsHasChanged = false;
            }
            if (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombatBlizzard)
            {
                if (((!_spellGroundMount.KnownSpell && (_spellGroundMount.Id == 0x2bc2d)) || (((_spellGroundMount.Id == 0x2bc2c) || (nManager.Wow.ObjectManager.ObjectManager.Me.Level < 20)) || ((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid) && (nManager.Wow.ObjectManager.ObjectManager.Me.Level < 0x10)))) || (((_groundMount == string.Empty) && (_flyMount == string.Empty)) && (_aquaMount == string.Empty)))
                {
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 20) || ((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid) && (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x10))) && (_noMountsInSettings != 1))
                    {
                        MessageBox.Show(Translate.Get(Translate.Id.No_mounts_in_settings));
                        _noMountsInSettings++;
                    }
                    return MountCapacity.Feet;
                }
                if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 20) || ((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid) && (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x12))) && (Usefuls.IsSwimming && (_aquaMount != string.Empty)))
                {
                    if ((_aquaMount == _localizedAbysalMountName) && (((Usefuls.AreaId != 0x12cf) && (Usefuls.AreaId != 0x1419)) && (Usefuls.AreaId != 0x1418)))
                    {
                        return MountCapacity.Feet;
                    }
                    return MountCapacity.Swimm;
                }
                if (Usefuls.IsOutdoors)
                {
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff((uint) 0x2b850))
                    {
                        return MountCapacity.Feet;
                    }
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 60) || ((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid) && (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x3a))) && ((_flyMount != string.Empty) && Usefuls.IsFlyableArea))
                    {
                        ContinentId continentId = (ContinentId) Usefuls.ContinentId;
                        if (_dreaneorFly && ((continentId == ContinentId.Draenor) || (Usefuls.ContinentNameMpqByContinentId(Usefuls.ContinentId) == "TanaanJungle")))
                        {
                            return MountCapacity.Fly;
                        }
                        if (_wisdom4Winds && (continentId == ContinentId.Pandaria))
                        {
                            return MountCapacity.Fly;
                        }
                        if (_coldWeather && (continentId == ContinentId.Northrend))
                        {
                            return MountCapacity.Fly;
                        }
                        if (_flightMasterLicense && (((continentId == ContinentId.Azeroth) || (continentId == ContinentId.Kalimdor)) || (continentId == ContinentId.Maelstrom)))
                        {
                            return MountCapacity.Fly;
                        }
                        Spell spell4 = new Spell(0x852a);
                        Spell spell5 = new Spell(0x852b);
                        Spell spell6 = new Spell(0x16099);
                        if ((continentId == ContinentId.Outland) && ((spell4.KnownSpell || spell5.KnownSpell) || spell6.KnownSpell))
                        {
                            return MountCapacity.Fly;
                        }
                    }
                    if (((((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 20) || ((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid) && (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x10))) || (_spellGroundMount.KnownSpell && (_spellGroundMount.Id == 0x2bc2d))) || (_spellGroundMount.Id == 0x2bc2c)) && (_groundMount != string.Empty))
                    {
                        return MountCapacity.Ground;
                    }
                }
            }
            return MountCapacity.Feet;
        }

        public static bool JustDismounted()
        {
            return !dismountTimer.IsReady;
        }

        public static void Land(bool useLuaToLand = false)
        {
            Logging.WriteNavigator("Landing in progress.");
            MovementsAction.Descend(true, false, useLuaToLand);
            nManager.Helpful.Timer timer = new nManager.Helpful.Timer(15000.0);
            while (Usefuls.IsFlying && !timer.IsReady)
            {
                float z = nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z;
                Thread.Sleep(100);
                if (z == nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z)
                {
                    timer.ForceReady();
                }
            }
            Thread.Sleep(150);
            MovementsAction.Descend(false, false, useLuaToLand);
        }

        public static void Mount(bool stopMove = true)
        {
            switch (GetMountCapacity())
            {
                case MountCapacity.Ground:
                    MountingGroundMount(stopMove);
                    return;

                case MountCapacity.Swimm:
                    MountingAquaticMount(stopMove);
                    return;

                case MountCapacity.Fly:
                    MountingFlyingMount(stopMove);
                    return;
            }
        }

        public static void MountingAquaticMount(bool stopMove = true)
        {
            try
            {
                if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && !OnAquaticMount())
                {
                    DismountMount(stopMove);
                }
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                {
                    if (stopMove)
                    {
                        MovementManager.StopMove();
                    }
                    else
                    {
                        MovementManager.StopMoveTo(true, false);
                    }
                    Logging.Write("Mounting aquatic mount " + _spellAquaMount.NameInGame);
                    Thread.Sleep(250);
                    SpellManager.CastSpellByNameLUA(_spellAquaMount.NameInGame);
                    Thread.Sleep((int) (500 + Usefuls.Latency));
                    while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(50);
                    }
                    Thread.Sleep(500);
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && nManager.Products.Products.IsStarted)
                    {
                        if (_nbTry >= 2)
                        {
                            MovementManager.UnStuck();
                        }
                        _nbTry++;
                    }
                    else
                    {
                        _nbTry = 0;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("MountTask > MountingAquaticMount(bool stopMove = true): " + exception, true);
            }
        }

        public static void MountingFlyingMount(bool stopMove = true)
        {
            try
            {
                if ((nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && !OnFlyMount()) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    DismountMount(stopMove);
                }
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                {
                    if (stopMove)
                    {
                        MovementManager.StopMove();
                    }
                    else
                    {
                        MovementManager.StopMoveTo(true, false);
                    }
                    Thread.Sleep(100);
                    if (Usefuls.IsSwimming)
                    {
                        Logging.WriteNavigator("Going out of water");
                    }
                    while (Usefuls.IsSwimming)
                    {
                        MovementsAction.Ascend(true, false, false);
                        Thread.Sleep(500);
                        MovementsAction.Ascend(false, false, false);
                    }
                    if (!Usefuls.IsOutdoors || nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                    {
                        goto Label_0152;
                    }
                    if (stopMove)
                    {
                        MovementManager.StopMove();
                    }
                    else
                    {
                        MovementManager.StopMoveTo(true, false);
                    }
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                    {
                        goto Label_00CD;
                    }
                }
                return;
            Label_00C0:
                Thread.Sleep((int) (50 + Usefuls.Latency));
            Label_00CD:
                if (nManager.Wow.ObjectManager.ObjectManager.Me.GetMove)
                {
                    goto Label_00C0;
                }
                Logging.Write("Mounting fly mount " + _spellFlyMount.NameInGame);
                SpellManager.CastSpellByNameLUA(_spellFlyMount.NameInGame);
                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                {
                    return;
                }
                Thread.Sleep((int) (500 + Usefuls.Latency));
                while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(50);
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                {
                    return;
                }
                Thread.Sleep(500);
            Label_0152:
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && nManager.Products.Products.IsStarted)
                {
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                    {
                        return;
                    }
                    Thread.Sleep(500);
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                    {
                        return;
                    }
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && nManager.Products.Products.IsStarted)
                    {
                        if (tryMounting >= 3)
                        {
                            Logging.Write("Mounting failed");
                            MovementManager.UnStuckFly();
                        }
                        else
                        {
                            nManager.Helpful.Timer timer = new nManager.Helpful.Timer(1000.0);
                            while (!timer.IsReady && !Usefuls.IsOutdoors)
                            {
                                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                Thread.Sleep(200);
                            }
                            tryMounting++;
                        }
                    }
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                {
                    tryMounting = 0;
                    Thread.Sleep(100);
                    Takeoff();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("MountTask > MountingFlyingMount(): " + exception, true);
            }
        }

        public static void MountingGroundMount(bool stopMove = true)
        {
            try
            {
                if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && !OnGroundMount())
                {
                    DismountMount(stopMove);
                }
                if (((nManagerSetting.CurrentSetting.GroundMountName != "") && !nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted) && (nManagerSetting.CurrentSetting.UseGroundMount && Usefuls.IsOutdoors))
                {
                    if (stopMove)
                    {
                        MovementManager.StopMove();
                    }
                    else
                    {
                        MovementManager.StopMoveTo(true, false);
                    }
                    Logging.Write("Mounting gound mount " + _spellGroundMount.NameInGame);
                    Thread.Sleep(250);
                    SpellManager.CastSpellByNameLUA(_spellGroundMount.NameInGame);
                    Thread.Sleep((int) (500 + Usefuls.Latency));
                    while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(50);
                    }
                    Thread.Sleep(500);
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && nManager.Products.Products.IsStarted)
                    {
                        if (_nbTry >= 2)
                        {
                            MovementManager.UnStuck();
                        }
                        _nbTry++;
                    }
                    else
                    {
                        _nbTry = 0;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("MountTask > MountingGroundMount(): " + exception, true);
            }
        }

        public static bool OnAquaticMount()
        {
            return _spellAquaMount.HaveBuff;
        }

        public static bool OnFlyMount()
        {
            return _spellFlyMount.HaveBuff;
        }

        public static bool OnGroundMount()
        {
            return _spellGroundMount.HaveBuff;
        }

        public static void Takeoff()
        {
            Logging.WriteNavigator("Take-off in progress.");
            MovementsAction.Ascend(true, false, false);
            nManager.Helpful.Timer timer = new nManager.Helpful.Timer(950.0);
            while (!Usefuls.IsFlying && !timer.IsReady)
            {
                Thread.Sleep(50);
            }
            Thread.Sleep(150);
            MovementsAction.Ascend(false, false, false);
        }
    }
}

