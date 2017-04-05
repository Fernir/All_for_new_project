namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Regeneration : nManager.FiniteStateMachine.State
    {
        private Spell _ebenugiurAgeca;
        private List<int> _iwouloutelAs = new List<int> { 0xffdb, 0xaa03, 0xa9fe, 0xffed, 0xffec, 0xffeb, 0xffdc };

        public void EatOrDrink(string itemName, bool isMana = false)
        {
            try
            {
                if (FishingTask.IsLaunched)
                {
                    FishingTask.StopLoopFish();
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                {
                    Usefuls.DisMount();
                }
                MovementManager.StopMove();
                Thread.Sleep(500);
                nManager.Wow.ObjectManager.ObjectManager.Me.ForceIsCasting = true;
                ItemsManager.UseItem(itemName);
                for (int i = 0; i < 30; i++)
                {
                    if ((nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || nManager.Wow.ObjectManager.ObjectManager.Me.InCombat) || ((!isMana && (nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent > 95f)) || (isMana && (nManager.Wow.ObjectManager.ObjectManager.Me.ManaPercentage > 0x5f))))
                    {
                        return;
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("public void EatOrDrink(string itemName, bool isMana = false): " + exception, true);
            }
            finally
            {
                nManager.Wow.ObjectManager.ObjectManager.Me.ForceIsCasting = false;
            }
        }

        private int LoupuUfiatuiv()
        {
            List<WoWItem> allFoodItems = ItemsManager.GetAllFoodItems();
            List<int> list2 = new List<int>();
            foreach (WoWItem item in allFoodItems)
            {
                if (this._iwouloutelAs.Contains(item.Entry))
                {
                    list2.Add(item.Entry);
                }
            }
            using (IEnumerator<int> enumerator2 = this._iwouloutelAs.Where<int>(new Func<int, bool>(list2.Contains)).GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    return enumerator2.Current;
                }
            }
            return 0;
        }

        public override void Run()
        {
            try
            {
                if ((!string.IsNullOrEmpty(nManagerSetting.CurrentSetting.FoodName) && (ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.FoodName) > 0)) && (nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent <= nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent))
                {
                    Logging.Write("Health regeneration started: Food mode");
                    this.EatOrDrink(nManagerSetting.CurrentSetting.FoodName, false);
                    Logging.Write("Health regeneration done: Food mode");
                }
                if ((!string.IsNullOrEmpty(nManagerSetting.CurrentSetting.BeverageName) && (ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.BeverageName) > 0)) && (nManagerSetting.CurrentSetting.DoRegenManaIfLow && (nManager.Wow.ObjectManager.ObjectManager.Me.ManaPercentage <= nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent)))
                {
                    Logging.Write("Mana regeneration started: Beverage mode");
                    this.EatOrDrink(nManagerSetting.CurrentSetting.BeverageName, true);
                    Logging.Write("Mana regeneration done: Beverage mode");
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Mage)
                {
                    if (this._ebenugiurAgeca.IsSpellUsable)
                    {
                        this._ebenugiurAgeca.Cast(true, true, false, null);
                        Thread.Sleep(300);
                    }
                    int entry = this.LoupuUfiatuiv();
                    while (entry <= 0)
                    {
                        if (this._ebenugiurAgeca.IsSpellUsable)
                        {
                            this._ebenugiurAgeca.Cast(true, true, false, null);
                            Thread.Sleep(300);
                        }
                        Thread.Sleep(0x3e8);
                        entry = this.LoupuUfiatuiv();
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat)
                        {
                            return;
                        }
                    }
                    if (entry > 0)
                    {
                        Logging.Write("Health regeneration started: Mage Food mode");
                        this.EatOrDrink(ItemsManager.GetItemNameById(entry), false);
                        Logging.Write("Health regeneration done: Mage Food mode");
                    }
                }
                if ((CombatClass.GetLightHealingSpell != null) && (CombatClass.GetLightHealingSpell.IsSpellUsable && (nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent <= 85f)))
                {
                    Logging.Write("Regeneration started: Light Heal mode");
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                    {
                        Usefuls.DisMount();
                    }
                    MovementManager.StopMove();
                    Thread.Sleep(500);
                    nManager.Helpful.Timer timer = new nManager.Helpful.Timer(10000.0);
                    while (CombatClass.GetLightHealingSpell.IsSpellUsable && (nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent <= 85f))
                    {
                        Thread.Sleep(250);
                        if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCasting)
                        {
                            if ((System.Math.Abs(nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent) < 0.001f) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe))
                            {
                                return;
                            }
                            if (timer.IsReady)
                            {
                                break;
                            }
                            CombatClass.GetLightHealingSpell.CastOnSelf(true, true, false);
                        }
                    }
                    Logging.Write((nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent > 85f) ? "Regeneration done (success): Light Heal mode" : "Regeneration done (timer): Light Heal mode");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Regeneration.Run(): " + exception, true);
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
                if (((!Usefuls.InGame || Usefuls.IsLoading) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || ((nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || Usefuls.IsFlying) || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                if (System.Math.Abs(nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent) < 0.001f)
                {
                    return false;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent > 85f)
                {
                    return false;
                }
                if (!Usefuls.IsSwimming)
                {
                    if ((!string.IsNullOrEmpty(nManagerSetting.CurrentSetting.FoodName) && (ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.FoodName) > 0)) && (nManager.Wow.ObjectManager.ObjectManager.Me.HealthPercent <= nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent))
                    {
                        return true;
                    }
                    if ((nManagerSetting.CurrentSetting.DoRegenManaIfLow && !string.IsNullOrEmpty(nManagerSetting.CurrentSetting.BeverageName)) && ((ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.BeverageName) > 0) && (nManager.Wow.ObjectManager.ObjectManager.Me.ManaPercentage <= nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent)))
                    {
                        return true;
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.Mage)
                    {
                        if ((this._ebenugiurAgeca == null) || ((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 13) && !this._ebenugiurAgeca.KnownSpell))
                        {
                            this._ebenugiurAgeca = new Spell("Conjure Refreshment");
                        }
                        if (this._ebenugiurAgeca.KnownSpell)
                        {
                            return true;
                        }
                    }
                }
                return ((CombatClass.GetLightHealingSpell != null) && CombatClass.GetLightHealingSpell.IsSpellUsable);
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

