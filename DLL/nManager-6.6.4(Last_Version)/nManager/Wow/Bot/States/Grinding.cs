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
        private WoWUnit _haruobuaxefea;
        public List<int> EntryTarget = new List<int>();
        public List<uint> FactionsTarget = new List<uint>();
        public uint MaxTargetLevel = 0x71;
        public uint MinTargetLevel;

        public override void Run()
        {
            Logging.Write(string.Concat(new object[] { "Player Attack ", this._haruobuaxefea.Name, " (lvl ", this._haruobuaxefea.Level, ")" }));
            UInt128 guid = Fight.StartFight(this._haruobuaxefea.Guid);
            if ((!this._haruobuaxefea.IsDead && (guid != 0)) && (this._haruobuaxefea.HealthPercent == 100f))
            {
                Logging.Write("Can't reach " + this._haruobuaxefea.Name + ", blacklisting it.");
                nManagerSetting.AddBlackList(guid, 0x1d4c0);
            }
            else if (this._haruobuaxefea.IsDead)
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
                    if (((!Usefuls.InGame || Usefuls.IsLoading) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || !nManager.Products.Products.IsStarted))
                    {
                        return false;
                    }
                    this._haruobuaxefea = new WoWUnit(0);
                    List<WoWUnit> listWoWUnit = new List<WoWUnit>();
                    if (this.FactionsTarget.Count > 0)
                    {
                        listWoWUnit.AddRange(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByFaction(this.FactionsTarget, false));
                    }
                    if (this.EntryTarget.Count > 0)
                    {
                        listWoWUnit.AddRange(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(this.EntryTarget, false));
                    }
                    this._haruobuaxefea = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(listWoWUnit, false, false, false);
                    if (!this._haruobuaxefea.IsValid)
                    {
                        return false;
                    }
                    if (!this._haruobuaxefea.Attackable)
                    {
                        Logging.Write("Unit " + this._haruobuaxefea.Name + " is non-attackable, blacklisting it.");
                        nManagerSetting.AddBlackList(this._haruobuaxefea.Guid, 0x927c0);
                        return false;
                    }
                    if (this._haruobuaxefea.IsTapped)
                    {
                        return false;
                    }
                    if ((((!nManagerSetting.IsBlackListedZone(this._haruobuaxefea.Position) && (this._haruobuaxefea.GetDistance < nManagerSetting.CurrentSetting.GatheringSearchRadius)) && (!nManagerSetting.IsBlackListed(this._haruobuaxefea.Guid) && this._haruobuaxefea.IsValid)) && (((this._haruobuaxefea.Target == nManager.Wow.ObjectManager.ObjectManager.Me.Target) || (this._haruobuaxefea.Target == nManager.Wow.ObjectManager.ObjectManager.Pet.Target)) || ((this._haruobuaxefea.Target == 0) || nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight))) && ((!this._haruobuaxefea.UnitNearest && (this._haruobuaxefea.Level <= this.MaxTargetLevel)) && (this._haruobuaxefea.Level >= this.MinTargetLevel)))
                    {
                        return true;
                    }
                    this._haruobuaxefea = new WoWUnit(0);
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

