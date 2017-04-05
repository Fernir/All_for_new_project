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
    using System.Windows.Forms;

    public class Looting : nManager.FiniteStateMachine.State
    {
        private List<WoWUnit> _giliucuecaxideJou;
        public static List<int> ForceLootCreatureList = new List<int>();
        public static bool IsLooting;

        public Looting()
        {
            if (ItemsManager.HasToy(0x1aa6f))
            {
                LootingTask.LootARangeId = 0x1aa6f;
            }
            else if (ItemsManager.HasToy(0xedb6))
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
            LootingTask.Pulse(this._giliucuecaxideJou);
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
                if (!nManagerSetting.CurrentSetting.ActivateMonsterLooting && (ForceLootCreatureList.Count <= 0))
                {
                    Logging.Write("Loading ForceLootCreatureList...");
                    string[] strArray = Others.ReadFileAllLines(Application.StartupPath + @"\Data\ForceLootCreatureList.txt");
                    for (int i = 0; i <= (strArray.Length - 1); i++)
                    {
                        int item = Others.ToInt32(strArray[i]);
                        if ((item > 0) && !ForceLootCreatureList.Contains(item))
                        {
                            ForceLootCreatureList.Add(item);
                        }
                    }
                    if (ForceLootCreatureList.Count > 0)
                    {
                        Logging.Write("Loaded " + ForceLootCreatureList.Count + " creatures to force loot even if loot is deactivated. (high reward)");
                    }
                }
                if (((!Usefuls.InGame || Usefuls.IsLoading) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || ((nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || Usefuls.IsFlying) || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                if (Usefuls.PlayerUsingVehicle)
                {
                    return false;
                }
                if (Usefuls.GetContainerNumFreeSlots <= 0)
                {
                    return false;
                }
                this._giliucuecaxideJou = new List<WoWUnit>();
                List<WoWUnit> woWUnitLootable = nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitLootable();
                if (nManagerSetting.CurrentSetting.ActivateBeastSkinning && nManagerSetting.CurrentSetting.BeastNinjaSkinning)
                {
                    woWUnitLootable.AddRange(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitSkinnable(new List<UInt128>(nManagerSetting.GetListGuidBlackListed())));
                }
                foreach (WoWUnit unit in woWUnitLootable)
                {
                    if ((nManagerSetting.CurrentSetting.ActivateMonsterLooting || ForceLootCreatureList.Contains(unit.Entry)) && ((((unit.GetDistance2D <= nManagerSetting.CurrentSetting.GatheringSearchRadius) && !nManagerSetting.IsBlackListed(unit.Guid)) && unit.IsValid) && (!unit.UnitNearest || (unit.GetDistance2D < 15f))))
                    {
                        this._giliucuecaxideJou.Add(unit);
                    }
                }
                return (this._giliucuecaxideJou.Count > 0);
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

