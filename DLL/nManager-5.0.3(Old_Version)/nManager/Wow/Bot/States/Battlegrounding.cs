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

    public class Battlegrounding : nManager.FiniteStateMachine.State
    {
        private WoWPlayer _unit;
        public string BattlegroundId;
        public uint MaxTargetLevel = (nManager.Wow.ObjectManager.ObjectManager.Me.Level + 3);

        public override void Run()
        {
            MountTask.DismountMount(true);
            Logging.Write(string.Concat(new object[] { "Engage fight against player ", this._unit.Name, " (lvl ", this._unit.Level, ")" }));
            UInt128 guid = Fight.StartFight(this._unit.Guid);
            if ((!this._unit.IsDead && (guid != 0)) && (this._unit.HealthPercent == 100f))
            {
                Logging.Write("Can't reach " + this._unit.Name + ", blacklisting its position.");
                nManagerSetting.AddBlackList(guid, 0x1d4c0);
            }
            else if (this._unit.IsDead)
            {
                Statistics.Kills++;
                Thread.Sleep((int) (Usefuls.Latency + 0x3e8));
                while ((!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && nManager.Wow.ObjectManager.ObjectManager.Me.InCombat) && (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0))
                {
                    Thread.Sleep(50);
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
                return "Battlegrounding";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (Usefuls.IsInBattleground)
                {
                    if (nManagerSetting.CurrentSetting.DontPullMonsters)
                    {
                        return false;
                    }
                    if ((((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))) || !nManager.Products.Products.IsStarted)
                    {
                        return false;
                    }
                    if (CustomProfile.GetSetDontStartFights)
                    {
                        return false;
                    }
                    this._unit = new WoWPlayer(0);
                    List<WoWPlayer> listWoWPlayer = new List<WoWPlayer>();
                    listWoWPlayer.AddRange((nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde") ? nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitAlliance() : nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitHorde());
                    this._unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWPlayer(listWoWPlayer);
                    if (!this._unit.IsValid)
                    {
                        return false;
                    }
                    if ((((!nManagerSetting.IsBlackListedZone(this._unit.Position) && (this._unit.GetDistance2D < nManagerSetting.CurrentSetting.GatheringSearchRadius)) && (!nManagerSetting.IsBlackListed(this._unit.Guid) && this._unit.IsValid)) && (((this._unit.Target == nManager.Wow.ObjectManager.ObjectManager.Me.Target) || (this._unit.Target == nManager.Wow.ObjectManager.ObjectManager.Pet.Target)) || ((this._unit.Target == 0) || nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight))) && (!this._unit.UnitNearest && (this._unit.Level <= this.MaxTargetLevel)))
                    {
                        return true;
                    }
                    this._unit = new WoWPlayer(0);
                }
                return false;
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

