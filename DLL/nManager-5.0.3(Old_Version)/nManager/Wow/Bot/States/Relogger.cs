namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow;
    using nManager.Wow.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class Relogger : nManager.FiniteStateMachine.State
    {
        private bool _relogger;

        public override void Run()
        {
            if (!Usefuls.InGame)
            {
                if (!this._relogger)
                {
                    Logging.Write("Initiate player relogging.");
                }
                while (nManager.Products.Products.IsStarted)
                {
                    Logging.Status = "relogger";
                    Login.SettingsLogin settings = new Login.SettingsLogin {
                        Login = nManagerSetting.CurrentSetting.EmailOfTheBattleNetAccount,
                        Password = nManagerSetting.CurrentSetting.PasswordOfTheBattleNetAccount,
                        Realm = Usefuls.RealmName,
                        Character = Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule + 0xe981d0),
                        BNetName = nManagerSetting.CurrentSetting.BattleNetSubAccount
                    };
                    Login.Pulse(settings);
                    this._relogger = true;
                    if ((this._relogger && Usefuls.InGame) && !Usefuls.IsLoadingOrConnecting)
                    {
                        Thread.Sleep(0x1388);
                        if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                        {
                            Logging.Write("Ending player relogging with success.");
                            this._relogger = false;
                            ConfigWowForThisBot.ConfigWow();
                            if ((nManager.Products.Products.ProductName == "Damage Dealer") && !nManagerSetting.CurrentSetting.ActivateMovementsDamageDealer)
                            {
                                ConfigWowForThisBot.StartStopClickToMove(false);
                            }
                            if ((nManager.Products.Products.ProductName == "Heal Bot") && nManagerSetting.CurrentSetting.ActivateMovementsHealerBot)
                            {
                                ConfigWowForThisBot.StartStopClickToMove(false);
                            }
                            SpellManager.UpdateSpellBook();
                            return;
                        }
                    }
                }
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
                return "relogger";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!nManager.Products.Products.IsStarted)
                {
                    return false;
                }
                return ((nManagerSetting.CurrentSetting.ActivateReloggerFeature && (nManagerSetting.CurrentSetting.EmailOfTheBattleNetAccount != string.Empty)) && ((nManagerSetting.CurrentSetting.PasswordOfTheBattleNetAccount != string.Empty) && !Usefuls.InGame));
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

