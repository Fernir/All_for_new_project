namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class Resurrect : nManager.FiniteStateMachine.State
    {
        private bool _erapunaehuAcaineihi;
        private const uint _iguibeu = 0x3a9f;
        private bool _paohoTeAcio;
        private readonly Spell _shamanReincarnation = new Spell("Reincarnation");
        private readonly Spell _warlockSoulstone = new Spell("Soulstone");
        private nManager.Helpful.Timer _xonaidiebaAriogi = new nManager.Helpful.Timer(0.0);

        public override void Run()
        {
            MovementManager.StopMove();
            MovementManager.StopMoveTo(true, false);
            if (nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff((uint) 0x3a9f))
            {
                Logging.Write("Resurrection Sickness detected, we will now wait its full duration to avoid dying in chain.");
                while (nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff((uint) 0x3a9f))
                {
                    Thread.Sleep(0x3e8);
                }
            }
            else
            {
                Logging.Write("The player has died. Starting the resurrection process.");
                if (((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Shaman) && this._shamanReincarnation.KnownSpell) && this._shamanReincarnation.IsSpellUsable)
                {
                    Thread.Sleep(0xdac);
                    Lua.RunMacroText("/click StaticPopup1Button2");
                    Thread.Sleep(0x3e8);
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        this._paohoTeAcio = false;
                        Logging.Write("The player have been resurrected using Shaman Reincarnation.");
                        Statistics.Deaths++;
                        return;
                    }
                }
                if ((((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Warlock) && this._warlockSoulstone.KnownSpell) && this._warlockSoulstone.HaveBuff) || nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff((uint) 0x183b))
                {
                    Thread.Sleep(0xdac);
                    Lua.RunMacroText("/click StaticPopup1Button2");
                    Thread.Sleep(0x3e8);
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        this._paohoTeAcio = false;
                        Logging.Write((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Warlock) ? "The player have been resurrected using his Soulstone." : "The player have been resurrected using a Soulstone offered by a Warlock.");
                        Statistics.Deaths++;
                        return;
                    }
                }
                Interact.Repop();
                Thread.Sleep(0x3e8);
                while ((!nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse.IsValid && (nManager.Wow.ObjectManager.ObjectManager.Me.Health <= 0)) && (nManager.Products.Products.IsStarted && Usefuls.InGame))
                {
                    Interact.Repop();
                    Thread.Sleep(0x3e8);
                }
                Thread.Sleep(0x3e8);
                if (Usefuls.IsInBattleground)
                {
                    this._xonaidiebaAriogi = new nManager.Helpful.Timer(35000.0);
                    while ((Usefuls.IsLoading && nManager.Products.Products.IsStarted) && Usefuls.InGame)
                    {
                        Thread.Sleep(100);
                    }
                    Thread.Sleep(0xfa0);
                    while (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        if (this._xonaidiebaAriogi.IsReady)
                        {
                            Interact.TeleportToSpiritHealer();
                            this._xonaidiebaAriogi = new nManager.Helpful.Timer(35000.0);
                            Logging.Write("The player have not been resurrected by any Battleground Spirit Healer in a reasonable time, Teleport back to the cimetary.");
                            Thread.Sleep(0x1388);
                        }
                        Thread.Sleep(0x3e8);
                    }
                    this._paohoTeAcio = false;
                    Logging.Write("The player have been resurrected by the Battleground Spirit Healer.");
                    Statistics.Deaths++;
                }
                else
                {
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level <= 10)
                    {
                        this._erapunaehuAcaineihi = true;
                        Logging.Write("We have no penalty for using Spirit Healer, so let's use it.");
                    }
                    else if ((nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse.IsValid && !nManagerSetting.CurrentSetting.UseSpiritHealer) && !this._erapunaehuAcaineihi)
                    {
                        Point positionCorpse;
                        while ((Usefuls.IsLoading && nManager.Products.Products.IsStarted) && Usefuls.InGame)
                        {
                            Thread.Sleep(100);
                        }
                        Thread.Sleep(0x3e8);
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                        {
                            MovementsAction.Ascend(true, false, false);
                            Thread.Sleep(500);
                            MovementsAction.Ascend(false, false, false);
                            positionCorpse = nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse;
                            positionCorpse.Z += 10f;
                            LongMove.LongMoveByNewThread(positionCorpse);
                        }
                        else
                        {
                            bool flag;
                            positionCorpse = nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse;
                            positionCorpse.Z = PathFinder.GetZPosition(positionCorpse, false);
                            List<Point> points = PathFinder.FindPath(positionCorpse, out flag, true, false);
                            if (!flag)
                            {
                                this._erapunaehuAcaineihi = true;
                                Logging.Write("There in no easy acces to the corpse, use Spirit Healer instead.");
                                return;
                            }
                            if ((points.Count > 1) || ((points.Count <= 1) && !nManagerSetting.CurrentSetting.UseSpiritHealer))
                            {
                                MovementManager.Go(points);
                            }
                        }
                        while ((MovementManager.InMovement || LongMove.IsLongMove) && ((nManager.Products.Products.IsStarted && Usefuls.InGame) && nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe))
                        {
                            if ((((positionCorpse.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 25f) && !this._paohoTeAcio) || ((Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xf3ed64) > 0) && !this._paohoTeAcio)) || (nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 5f))
                            {
                                LongMove.StopLongMove();
                                MovementManager.StopMove();
                            }
                            Thread.Sleep(100);
                        }
                        if (Usefuls.IsFlying)
                        {
                            MountTask.Land(false);
                        }
                        if (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xf3ed64) <= 0)
                        {
                            this._paohoTeAcio = true;
                        }
                        Point safeResPoint = Usefuls.GetSafeResPoint();
                        if (!safeResPoint.IsValid || !nManagerSetting.CurrentSetting.ActivateSafeResurrectionSystem)
                        {
                            if ((positionCorpse.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= 30f) || (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xf3ed64) > 0))
                            {
                                while (((positionCorpse.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= 30f) || (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xf3ed64) > 0)) && ((nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && nManager.Products.Products.IsStarted) && Usefuls.InGame))
                                {
                                    Interact.RetrieveCorpse();
                                    Thread.Sleep(0x3e8);
                                }
                            }
                        }
                        else
                        {
                            bool flag2;
                            MovementManager.StopMove();
                            List<Point> list2 = PathFinder.FindPath(safeResPoint, out flag2, true, false);
                            if (!flag2)
                            {
                                return;
                            }
                            MovementManager.Go(list2);
                            nManager.Helpful.Timer timer = null;
                            while (safeResPoint.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 5f)
                            {
                                if (!MovementManager.InMovement)
                                {
                                    MovementManager.Go(list2);
                                }
                                if ((timer == null) && (positionCorpse.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= 39f))
                                {
                                    timer = new nManager.Helpful.Timer(10000.0);
                                }
                                if ((timer != null) && timer.IsReady)
                                {
                                    break;
                                }
                                Thread.Sleep(0x3e8);
                            }
                            MovementManager.StopMove();
                            while (((positionCorpse.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= 39f) || (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xf3ed64) > 0)) && ((nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && nManager.Products.Products.IsStarted) && Usefuls.InGame))
                            {
                                Interact.RetrieveCorpse();
                                Thread.Sleep(0x3e8);
                            }
                        }
                    }
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        this._paohoTeAcio = false;
                        Logging.Write("The player have been resurrected when retrieving his corpse.");
                        Statistics.Deaths++;
                    }
                    else if ((nManagerSetting.CurrentSetting.UseSpiritHealer || this._erapunaehuAcaineihi) || nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff((uint) 0x3a9f))
                    {
                        Thread.Sleep(0xfa0);
                        WoWUnit unit = new WoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitSpiritHealer(), false, false, false).GetBaseAddress);
                        int num = 5;
                        if (!unit.IsValid)
                        {
                            Logging.Write("Spirit Healer not found, teleport back to the cimetery.");
                            Interact.TeleportToSpiritHealer();
                            Thread.Sleep(0x1388);
                        }
                        else
                        {
                            if (unit.GetDistance > 25f)
                            {
                                Interact.TeleportToSpiritHealer();
                                Thread.Sleep(0x1388);
                            }
                            MovementManager.MoveTo(unit.Position, false);
                            while (((unit.GetDistance > 5f) && nManager.Products.Products.IsStarted) && ((num >= 0) && Usefuls.InGame))
                            {
                                Thread.Sleep(300);
                                if (!nManager.Wow.ObjectManager.ObjectManager.Me.GetMove && (unit.GetDistance > 5f))
                                {
                                    MovementManager.MoveTo(unit.Position, false);
                                    num--;
                                }
                            }
                            Interact.InteractWith(unit.GetBaseAddress, false);
                            Thread.Sleep(0x7d0);
                            Interact.SpiritHealerAccept();
                            Thread.Sleep(0x3e8);
                            if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                            {
                                this._erapunaehuAcaineihi = false;
                                Logging.Write("The player have been resurrected by the Spirit Healer.");
                                Statistics.Deaths++;
                            }
                        }
                    }
                }
            }
        }

        public override List<nManager.FiniteStateMachine.State> BeforeStates
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Resurrect";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame || Usefuls.IsLoading)
                {
                    return false;
                }
                return (((nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && nManager.Wow.ObjectManager.ObjectManager.Me.IsValid) && nManager.Products.Products.IsStarted) || nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff((uint) 0x3a9f));
            }
        }

        public override List<nManager.FiniteStateMachine.State> NextStates
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public override int Priority { get; set; }
    }
}

