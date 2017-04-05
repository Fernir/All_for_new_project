namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Products;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Looting : nManager.FiniteStateMachine.State
    {
        private List<WoWUnit> _units;
        public static bool IsLooting;

        public Looting()
        {
            if (ItemsManager.GetItemCount(0x1aa6f) > 0)
            {
                LootingTask.LootARangeId = 0x1aa6f;
            }
            else if (ItemsManager.GetItemCount(0xedb6) > 0)
            {
                LootingTask.LootARangeId = 0xedb6;
            }
            else
            {
                LootingTask.LootARangeId = 0;
            }
        }

        public override void Run()
        {
            IsLooting = true;
            LootingTask.Pulse(this._units);
            IsLooting = false;
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
                return "Looting";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.ActivateMonsterLooting)
                {
                    return false;
                }
                if ((((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))) || (Usefuls.IsFlying || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                this._units = new List<WoWUnit>();
                List<WoWUnit> woWUnitLootable = nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitLootable();
                if (nManagerSetting.CurrentSetting.ActivateBeastSkinning && nManagerSetting.CurrentSetting.BeastNinjaSkinning)
                {
                    woWUnitLootable.AddRange(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitSkinnable(new List<UInt128>(nManagerSetting.GetListGuidBlackListed())));
                }
                foreach (WoWUnit unit in woWUnitLootable)
                {
                    if ((((unit.GetDistance2D <= nManagerSetting.CurrentSetting.GatheringSearchRadius) && !nManagerSetting.IsBlackListed(unit.Guid)) && unit.IsValid) && (!unit.UnitNearest || (unit.GetDistance2D < 15f)))
                    {
                        this._units.Add(unit);
                    }
                }
                return (this._units.Count > 0);
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

