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
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class Farming : nManager.FiniteStateMachine.State
    {
        private static List<WoWGameObject> _nodes = new List<WoWGameObject>();

        public static bool PlayerNearest(WoWGameObject node)
        {
            if (nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWPlayer().Any<WoWPlayer>(p => p.Position.DistanceTo2D(node.Position) <= nManagerSetting.CurrentSetting.DontHarvestIfPlayerNearRadius))
            {
                Logging.Write("Player near the node");
                nManagerSetting.AddBlackList(node.Guid, 0x3a98);
                return true;
            }
            return false;
        }

        public override void Run()
        {
            FarmingTask.Pulse(_nodes);
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
                return "Farming";
            }
        }

        public static List<WoWGameObject> GetFarmableGameObjects
        {
            get
            {
                List<WoWGameObject> list = new List<WoWGameObject>();
                List<WoWGameObject> woWGameObjectForFarm = nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectForFarm();
                for (int i = 0; i < woWGameObjectForFarm.Count; i++)
                {
                    WoWGameObject node = woWGameObjectForFarm[i];
                    if (((node.IsValid && !nManagerSetting.IsBlackListed(node.Guid)) && (!nManagerSetting.IsBlackListedZone(node.Position) && (node.GetDistance2D <= nManagerSetting.CurrentSetting.GatheringSearchRadius))) && ((node.CanOpen && !PlayerNearest(node)) && !node.UnitNearest))
                    {
                        list.Add(node);
                    }
                }
                return list;
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if ((((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))) || !nManager.Products.Products.IsStarted)
                {
                    return false;
                }
                if ((!nManagerSetting.CurrentSetting.ActivateHerbsHarvesting && !nManagerSetting.CurrentSetting.ActivateVeinsHarvesting) && !nManagerSetting.CurrentSetting.ActivateChestLooting)
                {
                    return false;
                }
                if (LongMove.IsLongMove && !nManagerSetting.CurrentSetting.HarvestDuringLongDistanceMovements)
                {
                    return false;
                }
                _nodes = GetFarmableGameObjects;
                return (_nodes.Count > 0);
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

