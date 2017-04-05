namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class Regeneration : nManager.FiniteStateMachine.State
    {
        public override void Run()
        {
            try
            {
                if ((nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent <= nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent) || ((nManager.Wow.ObjectManager.ObjectManager.Me.ManaPercentage <= nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent) && nManagerSetting.CurrentSetting.DoRegenManaIfLow))
                {
                    Logging.Write("Regen started");
                    MovementManager.StopMove();
                    Thread.Sleep(500);
                    if ((nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent <= nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent) && (nManagerSetting.CurrentSetting.FoodName != ""))
                    {
                        nManager.Wow.ObjectManager.ObjectManager.Me.forceIsCast = true;
                        ItemsManager.UseItem(nManagerSetting.CurrentSetting.FoodName);
                        Thread.Sleep(500);
                    }
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.ManaPercentage <= nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent) && (nManagerSetting.CurrentSetting.BeverageName != "")) && nManagerSetting.CurrentSetting.DoRegenManaIfLow)
                    {
                        nManager.Wow.ObjectManager.ObjectManager.Me.forceIsCast = true;
                        ItemsManager.UseItem(nManagerSetting.CurrentSetting.BeverageName);
                        Thread.Sleep(500);
                    }
                    while ((nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent <= 95f) && nManager.Products.Products.IsStarted)
                    {
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                        {
                            nManager.Wow.ObjectManager.ObjectManager.Me.forceIsCast = false;
                            return;
                        }
                        Thread.Sleep(500);
                    }
                    while (((nManager.Wow.ObjectManager.ObjectManager.Me.ManaPercentage <= 0x5f) && nManager.Products.Products.IsStarted) && nManagerSetting.CurrentSetting.DoRegenManaIfLow)
                    {
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                        {
                            nManager.Wow.ObjectManager.ObjectManager.Me.forceIsCast = false;
                            return;
                        }
                        Thread.Sleep(500);
                    }
                    nManager.Wow.ObjectManager.ObjectManager.Me.forceIsCast = false;
                    Logging.Write("Regen finished");
                }
            }
            catch
            {
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
                return "Regeneration";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || ((nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || Usefuls.IsFlying) || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                {
                    return false;
                }
                return ((nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent <= nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent) || ((nManager.Wow.ObjectManager.ObjectManager.Me.ManaPercentage <= nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent) && nManagerSetting.CurrentSetting.DoRegenManaIfLow));
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

