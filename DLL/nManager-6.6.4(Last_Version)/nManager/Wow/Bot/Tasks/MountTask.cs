namespace nManager.Wow.Bot.Tasks
{
    using nManager;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow;
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
        private static string _afawake;
        private static nManager.Helpful.Timer _ahaewucuseisuoDoe = new nManager.Helpful.Timer(0.0);
        private static bool _beiloumui;
        private static int _ehurov;
        private static Spell _ejuxuemav = new Spell(0);
        private static Spell _enefoekaugaujiLogajoi = new Spell(0);
        private static bool _enicoho = true;
        private static bool _epudaeCuxoxo;
        private static bool _eraomih;
        private static string _ewiesobuawevuv;
        private static Spell _exonacofeAloiruxuj = new Spell(0);
        private static string _hiaju = string.Empty;
        private static string _iqauwArisaim;
        private static bool _iwedioduteEcioviqog;
        private static int _koaqejuabia;
        private static int _ugibuibiolaSuki;
        public static bool AllowMounting = true;
        public static nManager.Helpful.Timer DismountTimer = new nManager.Helpful.Timer(5000.0);
        private static readonly object MountLocker = new object();
        public static bool SettingsHasChanged;

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
                            _ahaewucuseisuoDoe = new nManager.Helpful.Timer(5000.0);
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
            if ((AllowMounting && !Usefuls.PlayerUsingVehicle) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
            {
                if (_enicoho || SettingsHasChanged)
                {
                    _iqauwArisaim = nManagerSetting.CurrentSetting.AquaticMountName;
                    _ewiesobuawevuv = nManagerSetting.CurrentSetting.GroundMountName;
                    _afawake = nManagerSetting.CurrentSetting.FlyingMountName;
                    if (!string.IsNullOrEmpty(_iqauwArisaim.Trim()))
                    {
                        _enefoekaugaujiLogajoi = new Spell(_iqauwArisaim);
                    }
                    if (!string.IsNullOrEmpty(_ewiesobuawevuv.Trim()))
                    {
                        _ejuxuemav = new Spell(_ewiesobuawevuv);
                    }
                    if (!string.IsNullOrEmpty(_afawake.Trim()))
                    {
                        _exonacofeAloiruxuj = new Spell(_afawake);
                    }
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x10) && (_ewiesobuawevuv != string.Empty)) && (nManagerSetting.CurrentSetting.UseGroundMount && !_ejuxuemav.KnownSpell))
                    {
                        Others.ShowMessageBox(Translate.Get(Translate.Id.ThisGroundMountDoesNotExist) + _ewiesobuawevuv, "");
                        _ewiesobuawevuv = string.Empty;
                    }
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x12) && (_iqauwArisaim != string.Empty)) && !_enefoekaugaujiLogajoi.KnownSpell)
                    {
                        Others.ShowMessageBox(Translate.Get(Translate.Id.ThisAquaticMountDoesNotExist) + _iqauwArisaim, "");
                        _iqauwArisaim = string.Empty;
                    }
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x3a) && (_afawake != string.Empty)) && !_exonacofeAloiruxuj.KnownSpell)
                    {
                        Others.ShowMessageBox(Translate.Get(Translate.Id.ThisFlyingMountDoesNotExist) + _afawake, "");
                        _afawake = string.Empty;
                    }
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 60) && (_iqauwArisaim != string.Empty)) && (_hiaju == string.Empty))
                    {
                        _hiaju = SpellManager.GetSpellInfo(0x125c7).Name;
                    }
                    Spell spell = new Spell(0x1c4c9);
                    _epudaeCuxoxo = spell.KnownSpell;
                    Spell spell2 = new Spell(0xd3b5);
                    _eraomih = spell2.KnownSpell;
                    Spell spell3 = new Spell(0x1609b);
                    _iwedioduteEcioviqog = spell3.KnownSpell;
                    _beiloumui = (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 90) && Usefuls.IsCompletedAchievement(0x2722, false);
                    _enicoho = false;
                    SettingsHasChanged = false;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombatBlizzard)
                {
                    return MountCapacity.Feet;
                }
                if (((((_ewiesobuawevuv == string.Empty) && (_afawake == string.Empty)) && (_iqauwArisaim == string.Empty)) && (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 20) && (Skill.GetValue(SkillLine.Riding) > 0)) || ((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid) && (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x10)))) && (_koaqejuabia != 1))
                {
                    MessageBox.Show(Translate.Get(Translate.Id.No_mounts_in_settings));
                    _koaqejuabia++;
                    return MountCapacity.Feet;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(WoWUnit.CombatMount))
                {
                    return MountCapacity.Feet;
                }
                if (!HaveSpaceToMount())
                {
                    return MountCapacity.Feet;
                }
                if ((((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 20) && (Skill.GetValue(SkillLine.Riding) > 0)) || ((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid) && (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x12))) && (Usefuls.IsSwimming && (_iqauwArisaim != string.Empty)))
                {
                    if ((_iqauwArisaim == _hiaju) && (((Usefuls.AreaId != 0x12cf) && (Usefuls.AreaId != 0x1419)) && (Usefuls.AreaId != 0x1418)))
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
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 60) || ((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid) && (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x3a))) && (((_afawake != string.Empty) && Usefuls.IsFlyableArea) && !nManagerSetting.CurrentSetting.DeactivateFlyingMount))
                    {
                        ContinentId continentId = (ContinentId) Usefuls.ContinentId;
                        if (_beiloumui && ((continentId == ContinentId.Draenor) || (Usefuls.ContinentNameMpqByContinentId(Usefuls.ContinentId) == "TanaanJungle")))
                        {
                            return MountCapacity.Fly;
                        }
                        if (_epudaeCuxoxo && (continentId == ContinentId.Pandaria))
                        {
                            return MountCapacity.Fly;
                        }
                        if (_eraomih && (continentId == ContinentId.Northrend))
                        {
                            return MountCapacity.Fly;
                        }
                        if (_iwedioduteEcioviqog && (((continentId == ContinentId.Azeroth) || (continentId == ContinentId.Kalimdor)) || ((continentId == ContinentId.Maelstrom) || (continentId == ContinentId.AllianceGunship))))
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
                    if (((((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 20) && (Skill.GetValue(SkillLine.Riding) > 0)) || ((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid) && (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 0x10))) || ((_ejuxuemav.KnownSpell && (_ejuxuemav.Id == 0x2bc2d)) || (_ejuxuemav.Id == 0x2bc2c))) && (_ewiesobuawevuv != string.Empty))
                    {
                        return MountCapacity.Ground;
                    }
                }
            }
            return MountCapacity.Feet;
        }

        public static bool HaveSpaceToMount()
        {
            bool flag;
            try
            {
                Memory.WowMemory.GameFrameLock();
                float degrees = 0f;
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                while (degrees < 360f)
                {
                    float x = position.X + ((float) (2.0 * System.Math.Cos((double) nManager.Helpful.Math.DegreeToRadian(degrees))));
                    float y = position.Y + ((float) (2.0 * System.Math.Sin((double) nManager.Helpful.Math.DegreeToRadian(degrees))));
                    Point from = new Point(x, y, position.Z + 3f, "None");
                    if (TraceLine.TraceLineGo(from, position, CGWorldFrameHitFlags.HitTestAllButLiquid))
                    {
                        return false;
                    }
                    degrees += 60f;
                }
                flag = true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("public static bool HaveSpaceToMount(): " + exception, true);
                flag = true;
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
            return flag;
        }

        public static bool JustDismounted()
        {
            return !_ahaewucuseisuoDoe.IsReady;
        }

        public static void Land(bool useLuaToLand = false)
        {
            Logging.WriteNavigator("Landing in progress.");
            MovementsAction.Descend(true, false, useLuaToLand);
            nManager.Helpful.Timer timer = new nManager.Helpful.Timer(60000.0);
            while (Usefuls.IsFlying && !timer.IsReady)
            {
                float z = nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z;
                Thread.Sleep(0x3e8);
                if (z == nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z)
                {
                    timer.ForceReady();
                }
            }
            Logging.WriteDebug("Still flying after 1min of landing.");
            Thread.Sleep(150);
            MovementsAction.Descend(false, false, useLuaToLand);
        }

        public static void Mount(bool stopMove = true, bool bypassForcedGround = false)
        {
            lock (MountLocker)
            {
                switch (GetMountCapacity())
                {
                    case MountCapacity.Ground:
                        if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || OnAquaticMount())
                        {
                            goto Label_0076;
                        }
                        goto Label_008C;

                    case MountCapacity.Swimm:
                        MountingAquaticMount(stopMove);
                        goto Label_008C;

                    case MountCapacity.Fly:
                        if (bypassForcedGround || !nManagerSetting.CurrentSetting.UseGroundMount)
                        {
                            goto Label_0051;
                        }
                        if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                        {
                            break;
                        }
                        goto Label_008C;

                    default:
                        goto Label_008C;
                }
                MountingGroundMount(stopMove);
                goto Label_008C;
            Label_0051:
                MountingFlyingMount(stopMove);
                goto Label_008C;
            Label_0076:
                MountingGroundMount(stopMove);
            Label_008C:;
            }
        }

        public static void MountingAquaticMount(bool stopMove = true)
        {
            try
            {
                if ((nManagerSetting.CurrentSetting.AquaticMountName != nManagerSetting.CurrentSetting.GroundMountName) && (nManagerSetting.CurrentSetting.AquaticMountName != nManagerSetting.CurrentSetting.FlyingMountName))
                {
                    MovementManager.SwimmingMountRecentlyTimer = new nManager.Helpful.Timer(120000.0);
                }
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
                    Logging.Write("Mounting aquatic mount " + _enefoekaugaujiLogajoi.NameInGame);
                    Thread.Sleep(250);
                    SpellManager.CastSpellByNameLUA(_enefoekaugaujiLogajoi.NameInGame);
                    Thread.Sleep((int) (500 + Usefuls.Latency));
                    while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(50);
                    }
                    Thread.Sleep(500);
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && nManager.Products.Products.IsStarted)
                    {
                        if (_ugibuibiolaSuki >= 2)
                        {
                            MovementManager.UnStuck();
                        }
                        _ugibuibiolaSuki++;
                    }
                    else
                    {
                        _ugibuibiolaSuki = 0;
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
                    if (!Usefuls.IsOutdoors || nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.DruidMountId()))
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
                Logging.Write("Mounting fly mount " + _exonacofeAloiruxuj.NameInGame);
                SpellManager.CastSpellByNameLUA(_exonacofeAloiruxuj.NameInGame);
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
                        if (_ehurov >= 3)
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
                            _ehurov++;
                        }
                    }
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                {
                    _ehurov = 0;
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
                    Logging.Write("Mounting gound mount " + _ejuxuemav.NameInGame);
                    Thread.Sleep(250);
                    SpellManager.CastSpellByNameLUA(_ejuxuemav.NameInGame);
                    Thread.Sleep((int) (500 + Usefuls.Latency));
                    while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(50);
                    }
                    Thread.Sleep(500);
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && nManager.Products.Products.IsStarted)
                    {
                        if (_ugibuibiolaSuki >= 2)
                        {
                            MovementManager.UnStuck();
                        }
                        _ugibuibiolaSuki++;
                    }
                    else
                    {
                        _ugibuibiolaSuki = 0;
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
            return _enefoekaugaujiLogajoi.HaveBuff;
        }

        public static bool OnFlyMount()
        {
            return _exonacofeAloiruxuj.HaveBuff;
        }

        public static bool OnGroundMount()
        {
            return _ejuxuemav.HaveBuff;
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

        public static bool CanManagePet
        {
            get
            {
                return DismountTimer.IsReady;
            }
        }

        public static bool CanUseSpeedModifiers
        {
            get
            {
                return true;
            }
        }
    }
}

