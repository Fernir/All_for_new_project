namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Media;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    internal class StopBotIf : nManager.FiniteStateMachine.State
    {
        private bool _gameOffline;
        private bool _inPause;
        private Point _lastPos;
        private string _msgNewWhisper;
        private int _numberWhisper;
        private uint _startedLevel;
        private int _startedTime;
        private bool _threadSound;
        private readonly Channel _whisperChannel = new Channel();

        private void closeWow(string reason)
        {
            Logging.Write("Closing WoW because: " + reason);
            if (nManagerSetting.CurrentSetting.UseHearthstone)
            {
                Logging.Write("Loading Hearthstone informations");
                if (ItemsManager.GetItemCount(0x1b24) <= 0)
                {
                    Logging.Write(Translate.Get(Translate.Id.HearthstoneNotFound));
                }
                else if (!ItemsManager.IsItemOnCooldown(0x1b24) && ItemsManager.IsItemUsable(0x1b24))
                {
                    nManager.Helpful.Timer timer = new nManager.Helpful.Timer(45000.0);
                    MountTask.DismountMount(true);
                    MovementManager.StopMove();
                    MovementManager.StopMove();
                    timer.Reset();
                    Logging.Write("Hearthstone available, using it.");
                    while (!Usefuls.IsLoadingOrConnecting && !timer.IsReady)
                    {
                        ItemsManager.UseItem(ItemsManager.GetItemNameById(0x1b24));
                        Thread.Sleep(0x3e8);
                    }
                }
                else
                {
                    Logging.Write("Hearthstone found but on cooldown.");
                }
            }
            Memory.WowProcess.KillWowProcess();
            MessageBox.Show(reason, Translate.Get(Translate.Id.Stop_tnb_if), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Process.GetCurrentProcess().Kill();
        }

        public override void Run()
        {
            if ((nManagerSetting.CurrentSetting.StopTNBIfBagAreFull && (Usefuls.GetContainerNumFreeSlots <= 0)) && (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting))
            {
                Thread.Sleep(800);
                if (((Usefuls.GetContainerNumFreeSlots <= 0) && Usefuls.InGame) && !Usefuls.IsLoadingOrConnecting)
                {
                    this.closeWow(Translate.Get(Translate.Id.Bag_is_full));
                    return;
                }
            }
            if ((nManagerSetting.CurrentSetting.StopTNBIfHonorPointsLimitReached && (Usefuls.GetHonorPoint >= 0xfa0)) && (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting))
            {
                Thread.Sleep(800);
                if (((Usefuls.GetHonorPoint >= 0xfa0) && Usefuls.InGame) && !Usefuls.IsLoadingOrConnecting)
                {
                    this.closeWow(Translate.Get(Translate.Id.Reached_4000_Honor_Points));
                    return;
                }
            }
            if (nManagerSetting.CurrentSetting.StopTNBIfPlayerHaveBeenTeleported)
            {
                if (((this._lastPos == null) && Usefuls.InGame) && !Usefuls.IsLoadingOrConnecting)
                {
                    this._lastPos = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                }
                if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(this._lastPos) >= 450f) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    this.closeWow(Translate.Get(Translate.Id.Player_Teleported));
                    return;
                }
                if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                {
                    this._lastPos = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                }
            }
            if (((this._startedLevel == 0) && Usefuls.InGame) && !Usefuls.IsLoadingOrConnecting)
            {
                this._startedLevel = nManager.Wow.ObjectManager.ObjectManager.Me.Level;
            }
            if ((((nManager.Wow.ObjectManager.ObjectManager.Me.Level - this._startedLevel) >= nManagerSetting.CurrentSetting.StopTNBAfterXLevelup) && Usefuls.InGame) && (!Usefuls.IsLoadingOrConnecting && nManagerSetting.CurrentSetting.ActiveStopTNBAfterXLevelup))
            {
                this.closeWow(string.Concat(new object[] { Translate.Get(Translate.Id.Your_player_is_now_level), " ", nManager.Wow.ObjectManager.ObjectManager.Me.Level, " (+", nManager.Wow.ObjectManager.ObjectManager.Me.Level - this._startedLevel, " ", Translate.Get(Translate.Id.level_upper), ")" }));
            }
            else if ((Statistics.Stucks >= nManagerSetting.CurrentSetting.StopTNBAfterXStucks) && nManagerSetting.CurrentSetting.ActiveStopTNBAfterXStucks)
            {
                this.closeWow(Statistics.Stucks + " " + Translate.Get(Translate.Id.Blockages));
            }
            else
            {
                if (this._startedTime == 0)
                {
                    this._startedTime = Others.Times;
                }
                if (((this._startedTime + ((nManagerSetting.CurrentSetting.StopTNBAfterXMinutes * 60) * 0x3e8)) < Others.Times) && nManagerSetting.CurrentSetting.ActiveStopTNBAfterXMinutes)
                {
                    this.closeWow(string.Concat(new object[] { Translate.Get(Translate.Id.tnb_started_since), " ", nManagerSetting.CurrentSetting.StopTNBAfterXMinutes, " ", Translate.Get(Translate.Id.min) }));
                }
                else
                {
                    if ((nManagerSetting.CurrentSetting.PauseTNBIfNearByPlayer && Usefuls.InGame) && !Usefuls.IsLoadingOrConnecting)
                    {
                        if (!this._inPause && !nManager.Products.Products.InAutoPause)
                        {
                            if (((nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWPlayer().Count >= 1) && Usefuls.InGame) && !Usefuls.IsLoadingOrConnecting)
                            {
                                this._inPause = true;
                                nManager.Products.Products.InAutoPause = true;
                                Logging.Write("Player Nerby, pause bot");
                            }
                        }
                        else if (this._inPause)
                        {
                            Thread.Sleep(800);
                            if (((nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWPlayer().Count <= 0) && Usefuls.InGame) && !Usefuls.IsLoadingOrConnecting)
                            {
                                this._inPause = false;
                                nManager.Products.Products.InAutoPause = false;
                                Logging.Write("No Player Nerby, unpause bot");
                            }
                        }
                    }
                    while (this._whisperChannel.CurrentMsg < this._whisperChannel.GetCurrentMsgInWow)
                    {
                        string str = this._whisperChannel.ReadWhisperChannel();
                        if (!string.IsNullOrWhiteSpace(str))
                        {
                            this._numberWhisper++;
                            if (nManagerSetting.CurrentSetting.RecordWhispsInLogFiles)
                            {
                                Logging.WriteWhispers(str);
                            }
                            if ((this._numberWhisper >= nManagerSetting.CurrentSetting.StopTNBIfReceivedAtMostXWhispers) && nManagerSetting.CurrentSetting.ActiveStopTNBIfReceivedAtMostXWhispers)
                            {
                                this.closeWow(Translate.Get(Translate.Id.Whisper_Egal_at) + " " + this._numberWhisper);
                            }
                            if (nManagerSetting.CurrentSetting.PlayASongIfNewWhispReceived)
                            {
                                Thread thread3 = new Thread(new ThreadStart(this.ThreadSoundNewWhisper)) {
                                    Name = "Sound alarm",
                                    IsBackground = true
                                };
                                thread3.Start();
                                this._msgNewWhisper = str;
                                Thread thread4 = new Thread(new ThreadStart(this.ThreadMessageBoxNewWhisper)) {
                                    Name = "Messsage Box New Whisper"
                                };
                                thread4.Start();
                            }
                        }
                    }
                }
            }
        }

        private void ThreadMessageBoxNewWhisper()
        {
            MessageBox.Show(Translate.Get(Translate.Id.New_whisper) + ": " + this._msgNewWhisper, Translate.Get(Translate.Id.New_whisper), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this._threadSound = false;
        }

        private void ThreadSoundNewWhisper()
        {
            try
            {
                this._threadSound = true;
                SoundPlayer player = new SoundPlayer {
                    SoundLocation = Application.StartupPath + @"\Data\newWhisper.wav"
                };
                while (this._threadSound)
                {
                    player.PlaySync();
                }
                player.Stop();
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
                return "StopBotIf";
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
                if (nManager.Products.Products.InManualPause)
                {
                    this._lastPos = null;
                    return false;
                }
                if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting)
                {
                    if (!this._inPause && !nManager.Products.Products.InAutoPause)
                    {
                        Logging.Write("Game got disconnect or in loading, pausing TheNoobbot, please relog manually or make sure the relogger feature is activated.");
                        this._inPause = true;
                        nManager.Products.Products.InAutoPause = true;
                        this._gameOffline = true;
                    }
                    return false;
                }
                if (this._gameOffline)
                {
                    this._gameOffline = false;
                    ConfigWowForThisBot.ConfigWow();
                    if ((nManager.Products.Products.ProductName == "Damage Dealer") && !nManagerSetting.CurrentSetting.ActivateMovementsDamageDealer)
                    {
                        ConfigWowForThisBot.StartStopClickToMove(false);
                    }
                    if ((nManager.Products.Products.ProductName == "Heal Bot") && !nManagerSetting.CurrentSetting.ActivateMovementsHealerBot)
                    {
                        ConfigWowForThisBot.StartStopClickToMove(false);
                    }
                    SpellManager.UpdateSpellBook();
                    Logging.Write("Game is back online, unpausing, reloading SpellBook.");
                    this._inPause = false;
                    nManager.Products.Products.InAutoPause = false;
                }
                return true;
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

