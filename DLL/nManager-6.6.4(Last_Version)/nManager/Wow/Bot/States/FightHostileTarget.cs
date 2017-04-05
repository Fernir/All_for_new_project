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
        private WoWUnit _haruobuaxefea;

        public override void Run()
        {
            MountTask.DismountMount(true);
            if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
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
                    if ((!Usefuls.InGame || Usefuls.IsLoading) || ((nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid) || !nManager.Products.Products.IsStarted))
                    {
                        return false;
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InTransport && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombatBlizzard)
                    {
                        Fight.StopFight();
                        return false;
                    }
                    this._haruobuaxefea = nManager.Wow.ObjectManager.ObjectManager.Target;
                    if (((this._haruobuaxefea.IsValid && !this._haruobuaxefea.IsDead) && (this._haruobuaxefea.IsAlive && (this._haruobuaxefea.Health > 0))) && this._haruobuaxefea.Attackable)
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
                            this._haruobuaxefea = (from i in list
                                group i by i into grp
                                orderby grp.Count<WoWUnit>() descending
                                select grp.Key).First<WoWUnit>();
                            return true;
                        }
                    }
                    this._haruobuaxefea = new WoWUnit(0);
                    Fight.StopFight();
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

