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

    internal class Unaxakoawemeu : nManager.FiniteStateMachine.State
    {
        private bool _ekiuneoko;
        private nManager.Helpful.Timer _lumaoxiexeaUjaed;
        private bool _pouvueWeadoise;

        public override void Run()
        {
            if (!Usefuls.InGame)
            {
                if (!this._pouvueWeadoise)
                {
                    Logging.Write("Initiate player relogging.");
                    this._lumaoxiexeaUjaed = new nManager.Helpful.Timer(300000.0);
                    this._lumaoxiexeaUjaed.Reset();
                }
                while (nManager.Products.Products.IsStarted)
                {
                    Logging.Status = "relogger";
                    Login.SettingsLogin settings = new Login.SettingsLogin {
                        Login = nManagerSetting.CurrentSetting.EmailOfTheBattleNetAccount,
                        Password = nManagerSetting.CurrentSetting.PasswordOfTheBattleNetAccount,
                        Realm = Usefuls.RealmName,
                        Character = Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule + 0x1020550),
                        BNetName = nManagerSetting.CurrentSetting.BattleNetSubAccount
                    };
                    Login.Pulse(settings);
                    this._pouvueWeadoise = true;
                    if ((this._pouvueWeadoise && Usefuls.InGame) && !Usefuls.IsLoading)
                    {
                        Thread.Sleep(0x1388);
                        if (Usefuls.InGame && !Usefuls.IsLoading)
                        {
                            Logging.Write("Ending player relogging with success.");
                            this._lumaoxiexeaUjaed = null;
                            this._pouvueWeadoise = false;
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

        public override string _ehihireKeusoad
        {
            get
            {
                return "relogger";
            }
        }

        public override int _eluacudaVuwina { get; set; }

        public override List<nManager.FiniteStateMachine.State> _qausiaqaoFohepaiq
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public override bool _upioqourijoux
        {
            get
            {
                if (!nManager.Products.Products.IsStarted)
                {
                    return false;
                }
                if ((this._lumaoxiexeaUjaed != null) && this._lumaoxiexeaUjaed.IsReady)
                {
                    if (!this._ekiuneoko)
                    {
                        Logging.Write("We have tryed to relog for 5minutes without success, stopping Relogger system.");
                        this._ekiuneoko = true;
                    }
                    return false;
                }
                return ((nManagerSetting.CurrentSetting.ActivateReloggerFeature && (nManagerSetting.CurrentSetting.EmailOfTheBattleNetAccount != string.Empty)) && ((nManagerSetting.CurrentSetting.PasswordOfTheBattleNetAccount != string.Empty) && !Usefuls.InGame));
            }
        }

        public override List<nManager.FiniteStateMachine.State> _wanujosop
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }
    }
}

