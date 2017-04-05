namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class FightHostileTarget : nManager.FiniteStateMachine.State
    {
        private WoWUnit _unit;

        public override void Run()
        {
            MountTask.DismountMount(true);
            if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
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
                    Thread.Sleep((int) (Usefuls.Latency + 0x3e8));
                    while (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0))
                    {
                        Thread.Sleep(50);
                    }
                    Fight.StopFight();
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
                return "FightHostileTarget";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.DontPullMonsters)
                {
                    if ((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || ((nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid) || !nManager.Products.Products.IsStarted))
                    {
                        return false;
                    }
                    this._unit = nManager.Wow.ObjectManager.ObjectManager.Target;
                    if (((this._unit.IsValid && !this._unit.IsDead) && (this._unit.IsAlive && (this._unit.Health > 0))) && this._unit.Attackable)
                    {
                        return true;
                    }
                    if (Party.IsInGroup())
                    {
                        List<WoWUnit> list = new List<WoWUnit>();
                        foreach (UInt128 num in Party.GetPartyPlayersGUID())
                        {
                            if (num != nManager.Wow.ObjectManager.ObjectManager.Me.Guid)
                            {
                                WoWPlayer objectWoWPlayer = nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWPlayer(num);
                                if (((objectWoWPlayer != null) && (objectWoWPlayer.Target != 0)) && (objectWoWPlayer.InCombatBlizzard && (objectWoWPlayer.GetDistance < 40f)))
                                {
                                    WoWObject objectByGuid = nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(objectWoWPlayer.Target);
                                    if (objectByGuid == null)
                                    {
                                        break;
                                    }
                                    WoWUnit item = new WoWUnit(objectByGuid.GetBaseAddress);
                                    if (((item.IsValid && !item.IsDead) && (item.IsAlive && (item.Health > 0))) && item.Attackable)
                                    {
                                        list.Add(item);
                                    }
                                }
                            }
                        }
                        if (list.Count > 0)
                        {
                            this._unit = (from i in list
                                group i by i into grp
                                orderby grp.Count<WoWUnit>() descending
                                select grp.Key).First<WoWUnit>();
                            return true;
                        }
                    }
                    this._unit = new WoWUnit(0);
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

