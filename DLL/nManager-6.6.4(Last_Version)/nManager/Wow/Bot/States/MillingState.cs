namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class MillingState : nManager.FiniteStateMachine.State
    {
        private int _ujuofaimiduUvaotue;

        public override void Run()
        {
            this._ujuofaimiduUvaotue = Others.Times;
            MovementManager.StopMove();
            Thread.Sleep(500);
            MountTask.DismountMount(true);
            Logging.Write("Milling in progress");
            nManager.Helpful.Timer timer = new nManager.Helpful.Timer(900000.0);
            while (((Milling.NeedRun(nManagerSetting.CurrentSetting.HerbsToBeMilled) && nManager.Products.Products.IsStarted) && (Usefuls.InGame && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)) && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && !timer.IsReady))
            {
                Thread.Sleep(200);
                Milling.Pulse(nManagerSetting.CurrentSetting.HerbsToBeMilled);
                Thread.Sleep(750);
                while (((nManager.Wow.ObjectManager.ObjectManager.Me.IsCast && nManager.Products.Products.IsStarted) && (Usefuls.InGame && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)) && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && !timer.IsReady))
                {
                    Thread.Sleep(100);
                }
                Thread.Sleep((int) (Others.Random(600, 0x640) + Usefuls.Latency));
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
                return "Milling";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.ActivateAutoMilling)
                {
                    return false;
                }
                if (nManagerSetting.CurrentSetting.OnlyUseMillingInTown)
                {
                    return false;
                }
                if (nManagerSetting.CurrentSetting.HerbsToBeMilled.Count <= 0)
                {
                    return false;
                }
                if ((this._ujuofaimiduUvaotue + ((nManagerSetting.CurrentSetting.TimeBetweenEachMillingAttempt * 60) * 0x3e8)) > Others.Times)
                {
                    return false;
                }
                if (((!Usefuls.InGame || Usefuls.IsLoading) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || ((nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted) || (MovementManager.InMovement || !nManager.Products.Products.IsStarted)))
                {
                    return false;
                }
                return Milling.NeedRun(nManagerSetting.CurrentSetting.HerbsToBeMilled);
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

