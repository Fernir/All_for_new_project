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
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class IsAttacked : nManager.FiniteStateMachine.State
    {
        private WoWUnit _unit;

        public override void Run()
        {
            MountTask.DismountMount(true);
            Logging.Write(string.Concat(new object[] { "Player Attacked by ", this._unit.Name, " (lvl ", this._unit.Level, ")" }));
            UInt128 guid = Fight.StartFight(this._unit.Guid);
            if ((!this._unit.IsDead && (guid != 0)) && (this._unit.HealthPercent == 100f))
            {
                Logging.Write("Blacklisting " + this._unit.Name);
                nManagerSetting.AddBlackList(guid, 0x1d4c0);
            }
            else if (this._unit.IsDead)
            {
                Statistics.Kills++;
                if ((nManager.Products.Products.ProductName == "Quester") && (!this._unit.IsTapped || (this._unit.IsTapped && this._unit.IsTappedByMe)))
                {
                    Quest.KilledMobsToCount.Add(this._unit.Entry);
                }
                if (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0)
                {
                    Thread.Sleep((int) (Usefuls.Latency + 500));
                }
                while ((!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && nManager.Wow.ObjectManager.ObjectManager.Me.InCombat) && (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0))
                {
                    Thread.Sleep(150);
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
                return "IsAttacked";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (((((Usefuls.InGame && !Usefuls.IsLoadingOrConnecting) && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying))) && nManager.Products.Products.IsStarted) && (!CustomProfile.GetSetIgnoreFight && !Quest.GetSetIgnoreFight))
                {
                    this._unit = null;
                    if (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0)
                    {
                        this._unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetHostileUnitAttackingPlayer(), false, false, false);
                    }
                    if ((this._unit != null) && this._unit.IsValid)
                    {
                        return true;
                    }
                    if (!nManagerSetting.CurrentSetting.DontPullMonsters)
                    {
                        this._unit = nManager.Wow.ObjectManager.ObjectManager.GetUnitInAggroRange();
                        if (this._unit != null)
                        {
                            Logging.Write("Pulling " + this._unit.Name);
                            return true;
                        }
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

