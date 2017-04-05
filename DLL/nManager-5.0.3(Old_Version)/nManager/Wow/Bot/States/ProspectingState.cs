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

    public class ProspectingState : nManager.FiniteStateMachine.State
    {
        private int _lastTimeRunning;

        public override void Run()
        {
            this._lastTimeRunning = Others.Times;
            MovementManager.StopMove();
            Thread.Sleep(500);
            MountTask.DismountMount(true);
            Logging.Write("Prospecting in progress");
            nManager.Helpful.Timer timer = new nManager.Helpful.Timer(900000.0);
            while (((Prospecting.NeedRun(nManagerSetting.CurrentSetting.MineralsToProspect) && nManager.Products.Products.IsStarted) && (Usefuls.InGame && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)) && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && !timer.IsReady))
            {
                Thread.Sleep(200);
                Prospecting.Pulse(nManagerSetting.CurrentSetting.MineralsToProspect);
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
                return "Prospecting";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.ActivateAutoProspecting)
                {
                    return false;
                }
                if (nManagerSetting.CurrentSetting.OnlyUseProspectingInTown)
                {
                    return false;
                }
                if (nManagerSetting.CurrentSetting.MineralsToProspect.Count <= 0)
                {
                    return false;
                }
                if ((this._lastTimeRunning + ((nManagerSetting.CurrentSetting.TimeBetweenEachProspectingAttempt * 60) * 0x3e8)) > Others.Times)
                {
                    return false;
                }
                if ((((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))) || ((nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || MovementManager.InMovement) || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                return Prospecting.NeedRun(nManagerSetting.CurrentSetting.MineralsToProspect);
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

