namespace nManager.Wow.Bot.States
{
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class FlightMasterDiscovery : nManager.FiniteStateMachine.State
    {
        private bool _abitieloluge;
        private WoWUnit _doxiokal = new WoWUnit(0);
        private readonly nManager.Helpful.Timer _timerCheck = new nManager.Helpful.Timer(10000.0);

        public override void Run()
        {
            if (!this._abitieloluge)
            {
                MovementManager.StopMove();
                Logging.Write(string.Concat(new object[] { "Nearby Flight Master ", this._doxiokal.Name, " (", this._doxiokal.Entry, ") is not yet discovered." }));
            }
            this._abitieloluge = true;
            Npc target = new Npc {
                Entry = this._doxiokal.Entry,
                Position = this._doxiokal.Position,
                Name = this._doxiokal.Name,
                ContinentIdInt = Usefuls.ContinentId,
                Faction = (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde") ? Npc.FactionType.Horde : Npc.FactionType.Alliance
            };
            uint baseAddress = MovementManager.FindTarget(ref target, 5f, true, false, 0f, false);
            if ((!MovementManager.InMovement && (this._doxiokal.GetDistance <= 5f)) && (baseAddress > 0))
            {
                MovementManager.StopMove();
                Thread.Sleep(150);
                Interact.InteractWith(baseAddress, false);
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
                return "FlightMasterDiscovery";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                bool flag;
                if (((!Usefuls.InGame || Usefuls.IsLoading) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                if (!this._timerCheck.IsReady && !this._abitieloluge)
                {
                    return false;
                }
                this._timerCheck.Reset();
                WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitFlightMasterUndiscovered(), false, false, false);
                if (!unit.IsValid || (unit.UnitFlightMasteStatus != UnitFlightMasterStatus.FlightUndiscovered))
                {
                    this._abitieloluge = false;
                    return false;
                }
                this._doxiokal = unit;
                PathFinder.FindPath(this._doxiokal.Position, out flag, true, false);
                return flag;
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

