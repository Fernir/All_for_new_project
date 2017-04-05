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

    public class FightHostileTargetDamageDealerOnly : nManager.FiniteStateMachine.State
    {
        private WoWUnit _unit;

        public WoWUnit AcquireTarger()
        {
            if (nManager.Wow.ObjectManager.ObjectManager.Me.Target != 0)
            {
                if (nManagerSetting.CurrentSetting.DontPullMonsters && !nManager.Wow.ObjectManager.ObjectManager.Target.InCombat)
                {
                    return new WoWUnit(0);
                }
                WoWUnit target = nManager.Wow.ObjectManager.ObjectManager.Target;
                if ((target.IsValid && target.IsAlive) && (target.Health > 0))
                {
                    if (target.Attackable && target.IsHostile)
                    {
                        return target;
                    }
                    if (target.IsUnitBrawlerAndTappedByMe)
                    {
                        return target;
                    }
                }
            }
            return new WoWUnit(0);
        }

        public override void Run()
        {
            if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
            {
                MountTask.DismountMount(true);
            }
            else if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
            {
                Logging.Write("Please dismount as soon as is possible ! This product is passive when you are not yet in combat.");
                Thread.Sleep(500);
            }
            Logging.Write(string.Concat(new object[] { "Currently attacking ", this._unit.Name, " (lvl ", this._unit.Level, ")" }));
            UInt128 num = Fight.StartFightDamageDealer(this._unit.Guid);
            if (!this._unit.IsDead && (num != 0))
            {
                Logging.Write("Can't reach " + this._unit.Name + ", blacklisting it.");
            }
            else if (this._unit.IsDead)
            {
                Statistics.Kills++;
                this._unit = this.AcquireTarger();
                if (this._unit.IsValid)
                {
                    this.Run();
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
                return "FightHostileTargetDamageDealerOnly";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if ((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || ((nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid) || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                this._unit = this.AcquireTarger();
                return this._unit.IsValid;
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

