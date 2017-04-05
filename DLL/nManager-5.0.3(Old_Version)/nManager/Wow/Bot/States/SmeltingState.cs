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

    public class SmeltingState : nManager.FiniteStateMachine.State
    {
        public bool IgnoreSmeltingZone;

        public override void Run()
        {
            if (!this.IgnoreSmeltingZone)
            {
                Logging.Write("Smelting in progress");
                Npc npcNearby = NpcDB.GetNpcNearby(Npc.NpcType.SmeltingForge, false);
                if (npcNearby.Entry <= 0)
                {
                    return;
                }
                if (npcNearby.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 10f)
                {
                    List<Point> points = new List<Point>();
                    if ((npcNearby.Position.Type.ToLower() == "flying") && (nManagerSetting.CurrentSetting.FlyingMountName != ""))
                    {
                        points.Add(npcNearby.Position);
                    }
                    else
                    {
                        points = PathFinder.FindPath(npcNearby.Position);
                    }
                    MovementManager.Go(points);
                    nManager.Helpful.Timer timer = new nManager.Helpful.Timer((double) (((int) ((nManager.Helpful.Math.DistanceListPoint(points) / 3f) * 1000f)) + 0x1388));
                    Thread.Sleep(700);
                    while ((((MovementManager.InMovement && nManager.Products.Products.IsStarted) && Usefuls.InGame) && (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)))) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        if (timer.IsReady)
                        {
                            MovementManager.StopMove();
                        }
                        if (npcNearby.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 3.7f)
                        {
                            MovementManager.StopMove();
                        }
                        Thread.Sleep(100);
                    }
                }
                MovementManager.StopMove();
                MountTask.DismountMount(true);
                Thread.Sleep(500);
                if (npcNearby.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 15f)
                {
                    return;
                }
            }
            Smelting.OpenSmeltingWindow();
            nManager.Helpful.Timer timer2 = new nManager.Helpful.Timer(900000.0);
            while (((Smelting.NeedRun(false) && nManager.Products.Products.IsStarted) && (Usefuls.InGame && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)) && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && !timer2.IsReady))
            {
                Smelting.Pulse();
                Thread.Sleep(0x5dc);
                while (((nManager.Wow.ObjectManager.ObjectManager.Me.IsCast && nManager.Products.Products.IsStarted) && (Usefuls.InGame && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)) && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && !timer2.IsReady))
                {
                    Thread.Sleep(700);
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(700);
                    }
                }
                Thread.Sleep(Usefuls.Latency);
            }
            Smelting.CloseSmeltingWindow();
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
                return "Smelting";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.ActivateAutoSmelting)
                {
                    return false;
                }
                if ((((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))) || !nManager.Products.Products.IsStarted)
                {
                    return false;
                }
                return ((NpcDB.GetNpcNearby(Npc.NpcType.SmeltingForge, false).Entry > 0) && Smelting.NeedRun(true));
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

