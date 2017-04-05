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
        private nManager.Helpful.Timer _battlegroundResurrect = new nManager.Helpful.Timer(0.0);
        private bool _failed;
        private bool _forceSpiritHealer;
        private readonly Spell _shamanReincarnation = new Spell("Reincarnation");
        private readonly Spell _warlockSoulstone = new Spell("Soulstone");

        public override void Run()
        {
            MovementManager.StopMove();
            MovementManager.StopMoveTo(true, false);
            Logging.Write("The player has died. Starting the resurrection process.");
            if (((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Shaman) && this._shamanReincarnation.KnownSpell) && this._shamanReincarnation.IsSpellUsable)
            {
                Thread.Sleep(0xdac);
                Lua.RunMacroText("/click StaticPopup1Button2");
                Thread.Sleep(0x3e8);
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    this._failed = false;
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
                    this._failed = false;
                    Logging.Write((nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Warlock) ? "The player have been resurrected using his Soulstone." : "The player have been resurrected using a Soulstone offered by a Warlock.");
                    Statistics.Deaths++;
                    return;
                }
            }
            Interact.Repop();
            Thread.Sleep(0x3e8);
            while (((nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse.X == 0f) && (nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse.Y == 0f)) && (((nManager.Wow.ObjectManager.ObjectManager.Me.Health <= 0) && nManager.Products.Products.IsStarted) && Usefuls.InGame))
            {
                Interact.Repop();
                Thread.Sleep(0x3e8);
            }
            Thread.Sleep(0x3e8);
            if (Usefuls.IsInBattleground)
            {
                this._battlegroundResurrect = new nManager.Helpful.Timer(35000.0);
                while ((Usefuls.IsLoadingOrConnecting && nManager.Products.Products.IsStarted) && Usefuls.InGame)
                {
                    Thread.Sleep(100);
                }
                Thread.Sleep(0xfa0);
                while (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    if (this._battlegroundResurrect.IsReady)
                    {
                        Interact.TeleportToSpiritHealer();
                        this._battlegroundResurrect = new nManager.Helpful.Timer(35000.0);
                        Logging.Write("The player have not been resurrected by any Battleground Spirit Healer in a reasonable time, Teleport back to the cimetary.");
                        Thread.Sleep(0x1388);
                    }
                    Thread.Sleep(0x3e8);
                }
                this._failed = false;
                Logging.Write("The player have been resurrected by the Battleground Spirit Healer.");
                Statistics.Deaths++;
            }
            else
            {
                if (((nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse.X != 0f) && (nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse.Y != 0f)) && (!nManagerSetting.CurrentSetting.UseSpiritHealer && !this._forceSpiritHealer))
                {
                    Point positionCorpse;
                    while ((Usefuls.IsLoadingOrConnecting && nManager.Products.Products.IsStarted) && Usefuls.InGame)
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
                        List<Point> points = PathFinder.FindPath(positionCorpse, out flag, true, false);
                        if (!flag)
                        {
                            this._forceSpiritHealer = true;
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
                        if ((((positionCorpse.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 25f) && !this._failed) || ((Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xda5614) > 0) && !this._failed)) || (nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 5f))
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
                    if (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xda5614) <= 0)
                    {
                        this._failed = true;
                    }
                    if ((positionCorpse.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 26f) || (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xda5614) > 0))
                    {
                        while (((positionCorpse.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 27f) || (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xda5614) > 0)) && ((nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && nManager.Products.Products.IsStarted) && Usefuls.InGame))
                        {
                            Interact.RetrieveCorpse();
                            Thread.Sleep(0x3e8);
                        }
                    }
                }
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    this._failed = false;
                    Logging.Write("The player have been resurrected when retrieving his corpse.");
                    Statistics.Deaths++;
                }
                else if (nManagerSetting.CurrentSetting.UseSpiritHealer || this._forceSpiritHealer)
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
                            this._forceSpiritHealer = false;
                            Logging.Write("The player have been resurrected by the Spirit Healer.");
                            Statistics.Deaths++;
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
                if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting)
                {
                    return false;
                }
                return ((nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && nManager.Wow.ObjectManager.ObjectManager.Me.IsValid) && nManager.Products.Products.IsStarted);
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

