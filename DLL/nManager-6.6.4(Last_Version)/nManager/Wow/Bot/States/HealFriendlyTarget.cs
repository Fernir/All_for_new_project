namespace nManager.Wow.Bot.States
{
    using nManager.FiniteStateMachine;
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

    public class HealFriendlyTarget : nManager.FiniteStateMachine.State
    {
        public override void Run()
        {
            MountTask.DismountMount(true);
            Heal.StartHealBot();
            while ((nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent < 100f) || (Party.IsInGroup() && Party.GetPartyPlayersGUID().Any<UInt128>(playerInMyParty => (new WoWUnit((uint) playerInMyParty).HealthPercent < 100f))))
            {
                Thread.Sleep(200);
            }
            Heal.StopHeal();
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
                return "HealFriendlyTarget";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if ((!Usefuls.InGame || Usefuls.IsLoading) || ((nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid) || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent < 100f)
                {
                    return true;
                }
                if (!Party.IsInGroup())
                {
                    return false;
                }
                return Party.GetPartyPlayersGUID().Any<UInt128>(playerInMyParty => (new WoWUnit((uint) playerInMyParty).HealthPercent < 100f));
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

