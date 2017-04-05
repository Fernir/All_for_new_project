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

    public class LevelupCheck : nManager.FiniteStateMachine.State
    {
        private static uint _uqedeoboIwuaseisa;

        public override void Run()
        {
            _uqedeoboIwuaseisa = nManager.Wow.ObjectManager.ObjectManager.Me.Level;
            Logging.Write("Congratulations: Player is now level " + _uqedeoboIwuaseisa);
            if ((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 15) && nManagerSetting.CurrentSetting.AutoAssignTalents)
            {
                Talent.DoTalents();
            }
            SpellManager.UpdateSpellBook();
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
                return "LevelupCheck";
            }
        }

        public static uint GetLastLevel
        {
            get
            {
                return _uqedeoboIwuaseisa;
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (((!Usefuls.InGame || Usefuls.IsLoading) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.Level == 0)
                {
                    return false;
                }
                if (_uqedeoboIwuaseisa <= 0)
                {
                    _uqedeoboIwuaseisa = nManager.Wow.ObjectManager.ObjectManager.Me.Level;
                }
                return (_uqedeoboIwuaseisa < nManager.Wow.ObjectManager.ObjectManager.Me.Level);
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

