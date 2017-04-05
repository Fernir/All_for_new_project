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

    internal class Duogiowueqov : nManager.FiniteStateMachine.State
    {
        private bool _biotanisikutib;
        private string _ciucisoaj;
        private bool _ediumeraubabiRuibij;
        private int _ihodoapi;
        private Point _ineuquraifegi;
        private int _itaicoulaqaoci;
        private uint _naonuluUpilo;
        private readonly Channel _whisperChannel = new Channel();
        private bool _xoaqarMuic;

        private void Ojovubawoavoin()
        {
            MessageBox.Show(Translate.Get(Translate.Id.New_whisper) + ": " + this._ciucisoaj, Translate.Get(Translate.Id.New_whisper), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this._biotanisikutib = false;
        }

        private void PaeseEs(string moanehTa)
        {
            Logging.Write("Closing WoW because: " + moanehTa);
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
                    while (!Usefuls.IsLoading && !timer.IsReady)
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
            MessageBox.Show(moanehTa, Translate.Get(Translate.Id.Stop_tnb_if), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Process.GetCurrentProcess().Kill();
        }

        public override void Run()
        {
            if ((nManagerSetting.CurrentSetting.StopTNBIfBagAreFull && (Usefuls.GetContainerNumFreeSlots <= 0)) && (Usefuls.InGame && !Usefuls.IsLoading))
            {
                Thread.Sleep(800);
                if (((Usefuls.GetContainerNumFreeSlots <= 0) && Usefuls.InGame) && !Usefuls.IsLoading)
                {
                    this.PaeseEs(Translate.Get(Translate.Id.Bag_is_full));
                    return;
                }
            }
            if ((nManagerSetting.CurrentSetting.StopTNBIfHonorPointsLimitReached && (Usefuls.GetHonorPoint >= 0xfa0)) && (Usefuls.InGame && !Usefuls.IsLoading))
            {
                Thread.Sleep(800);
                if (((Usefuls.GetHonorPoint >= 0xfa0) && Usefuls.InGame) && !Usefuls.IsLoading)
                {
                    this.PaeseEs(Translate.Get(Translate.Id.Reached_4000_Honor_Points));
                    return;
                }
            }
            if (nManagerSetting.CurrentSetting.StopTNBIfPlayerHaveBeenTeleported)
            {
                if (((this._ineuquraifegi == null) && Usefuls.InGame) && !Usefuls.IsLoading)
                {
                    this._ineuquraifegi = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                }
                if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(this._ineuquraifegi) >= 450f) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    this.PaeseEs(Translate.Get(Translate.Id.Player_Teleported));
                    return;
                }
                if (Usefuls.InGame && !Usefuls.IsLoading)
                {
                    this._ineuquraifegi = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                }
            }
            if (((this._naonuluUpilo == 0) && Usefuls.InGame) && !Usefuls.IsLoading)
            {
                this._naonuluUpilo = nManager.Wow.ObjectManager.ObjectManager.Me.Level;
            }
            if ((((nManager.Wow.ObjectManager.ObjectManager.Me.Level - this._naonuluUpilo) >= nManagerSetting.CurrentSetting.StopTNBAfterXLevelup) && Usefuls.InGame) && (!Usefuls.IsLoading && nManagerSetting.CurrentSetting.ActiveStopTNBAfterXLevelup))
            {
                this.PaeseEs(string.Concat(new object[] { Translate.Get(Translate.Id.Your_player_is_now_level), " ", nManager.Wow.ObjectManager.ObjectManager.Me.Level, " (+", nManager.Wow.ObjectManager.ObjectManager.Me.Level - this._naonuluUpilo, " ", Translate.Get(Translate.Id.level_upper), ")" }));
            }
            else if ((Statistics.Stucks >= nManagerSetting.CurrentSetting.StopTNBAfterXStucks) && nManagerSetting.CurrentSetting.ActiveStopTNBAfterXStucks)
            {
                this.PaeseEs(Statistics.Stucks + " " + Translate.Get(Translate.Id.Blockages));
            }
            else
            {
                if (this._ihodoapi == 0)
                {
                    this._ihodoapi = Others.Times;
                }
                if (((this._ihodoapi + ((nManagerSetting.CurrentSetting.StopTNBAfterXMinutes * 60) * 0x3e8)) < Others.Times) && nManagerSetting.CurrentSetting.ActiveStopTNBAfterXMinutes)
                {
                    this.PaeseEs(string.Concat(new object[] { Translate.Get(Translate.Id.tnb_started_since), " ", nManagerSetting.CurrentSetting.StopTNBAfterXMinutes, " ", Translate.Get(Translate.Id.min) }));
                }
                else
                {
                    if ((nManagerSetting.CurrentSetting.PauseTNBIfNearByPlayer && Usefuls.InGame) && !Usefuls.IsLoading)
                    {
                        if (!this._xoaqarMuic && !nManager.Products.Products.InAutoPause)
                        {
                            if (((nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWPlayer().Count >= 1) && Usefuls.InGame) && !Usefuls.IsLoading)
                            {
                                this._xoaqarMuic = true;
                                nManager.Products.Products.InAutoPause = true;
                                Logging.Write("Player Nerby, pause bot");
                            }
                        }
                        else if (this._xoaqarMuic)
                        {
                            Thread.Sleep(800);
                            if (((nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWPlayer().Count <= 0) && Usefuls.InGame) && !Usefuls.IsLoading)
                            {
                                this._xoaqarMuic = false;
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
                            this._itaicoulaqaoci++;
                            if (nManagerSetting.CurrentSetting.RecordWhispsInLogFiles)
                            {
                                Logging.WriteWhispers(str);
                            }
                            if ((this._itaicoulaqaoci >= nManagerSetting.CurrentSetting.StopTNBIfReceivedAtMostXWhispers) && nManagerSetting.CurrentSetting.ActiveStopTNBIfReceivedAtMostXWhispers)
                            {
                                this.PaeseEs(Translate.Get(Translate.Id.Whisper_Egal_at) + " " + this._itaicoulaqaoci);
                            }
                            if (nManagerSetting.CurrentSetting.PlayASongIfNewWhispReceived)
                            {
                                Thread thread3 = new Thread(new ThreadStart(this.ToxitiAvetau)) {
                                    Name = "Sound alarm",
                                    IsBackground = true
                                };
                                thread3.Start();
                                this._ciucisoaj = str;
                                Thread thread4 = new Thread(new ThreadStart(this.Ojovubawoavoin)) {
                                    Name = "Messsage Box New Whisper"
                                };
                                thread4.Start();
                            }
                        }
                    }
                }
            }
        }

        private void ToxitiAvetau()
        {
            try
            {
                this._biotanisikutib = true;
                SoundPlayer player = new SoundPlayer {
                    SoundLocation = Application.StartupPath + @"\Data\newWhisper.wav"
                };
                while (this._biotanisikutib)
                {
                    player.PlaySync();
                }
                player.Stop();
            }
            catch
            {
            }
        }

        public override string _ehihireKeusoad
        {
            get
            {
                return "StopBotIf";
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
                if (nManager.Products.Products.InManualPause)
                {
                    this._ineuquraifegi = null;
                    return false;
                }
                if (!Usefuls.InGame || Usefuls.IsLoading)
                {
                    if (!this._xoaqarMuic && !nManager.Products.Products.InAutoPause)
                    {
                        Logging.Write("Game got disconnect or in loading, pausing TheNoobbot, please relog manually or make sure the relogger feature is activated.");
                        this._xoaqarMuic = true;
                        nManager.Products.Products.InAutoPause = true;
                        this._ediumeraubabiRuibij = true;
                    }
                    return false;
                }
                if (this._ediumeraubabiRuibij)
                {
                    this._ediumeraubabiRuibij = false;
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
                    this._xoaqarMuic = false;
                    nManager.Products.Products.InAutoPause = false;
                }
                return true;
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

