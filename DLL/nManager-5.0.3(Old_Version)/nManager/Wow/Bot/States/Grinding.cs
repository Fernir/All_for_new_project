namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Class;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class Grinding : nManager.FiniteStateMachine.State
    {
        private WoWUnit _unit;
        public List<int> EntryTarget = new List<int>();
        public List<uint> FactionsTarget = new List<uint>();
        public uint MaxTargetLevel = 90;
        public uint MinTargetLevel;

        public override void Run()
        {
            Logging.Write(string.Concat(new object[] { "Player Attack ", this._unit.Name, " (lvl ", this._unit.Level, ")" }));
            UInt128 guid = Fight.StartFight(this._unit.Guid);
            if ((!this._unit.IsDead && (guid != 0)) && (this._unit.HealthPercent == 100f))
            {
                Logging.Write("Can't reach " + this._unit.Name + ", blacklisting it.");
                nManagerSetting.AddBlackList(guid, 0x1d4c0);
            }
            else if (this._unit.IsDead)
            {
                Statistics.Kills++;
                if (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0)
                {
                    Thread.Sleep((int) (Usefuls.Latency + 500));
                }
                while ((!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && nManager.Wow.ObjectManager.ObjectManager.Me.InCombat) && (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0))
                {
                    Thread.Sleep(50);
                }
                Fight.StopFight();
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
                return "Grinding";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.DontPullMonsters)
                {
                    if ((((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))) || !nManager.Products.Products.IsStarted)
                    {
                        return false;
                    }
                    this._unit = new WoWUnit(0);
                    List<WoWUnit> listWoWUnit = new List<WoWUnit>();
                    if (this.FactionsTarget.Count > 0)
                    {
                        listWoWUnit.AddRange(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByFaction(this.FactionsTarget, false));
                    }
                    if (this.EntryTarget.Count > 0)
                    {
                        listWoWUnit.AddRange(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(this.EntryTarget, false));
                    }
                    this._unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(listWoWUnit, false, false, false);
                    if (this._unit.IsValid)
                    {
                        if (this._unit.IsTapped && !this._unit.IsTappedByMe)
                        {
                            return false;
                        }
                        if ((((!nManagerSetting.IsBlackListedZone(this._unit.Position) && (this._unit.GetDistance < nManagerSetting.CurrentSetting.GatheringSearchRadius)) && (!nManagerSetting.IsBlackListed(this._unit.Guid) && this._unit.IsValid)) && (((this._unit.Target == nManager.Wow.ObjectManager.ObjectManager.Me.Target) || (this._unit.Target == nManager.Wow.ObjectManager.ObjectManager.Pet.Target)) || ((this._unit.Target == 0) || nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight))) && ((!this._unit.UnitNearest && (this._unit.Level <= this.MaxTargetLevel)) && (this._unit.Level >= this.MinTargetLevel)))
                        {
                            return true;
                        }
                        this._unit = new WoWUnit(0);
                    }
                }
                return false;
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

