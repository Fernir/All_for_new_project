namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SpecializationCheck : nManager.FiniteStateMachine.State
    {
        private WoWSpecialization _lastSpecialization = nManager.Wow.ObjectManager.ObjectManager.Me.WowSpecialization(false);

        public override void Run()
        {
            Logging.Write("We have detected that your Wow Specialization has changed, reloading it.");
            this._lastSpecialization = nManager.Wow.ObjectManager.ObjectManager.Me.WowSpecialization(false);
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
                return "SpecializationCheck";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                uint getLastLevel = LevelupCheck.GetLastLevel;
                return (((getLastLevel > 0) && (getLastLevel == nManager.Wow.ObjectManager.ObjectManager.Me.Level)) && (this._lastSpecialization != nManager.Wow.ObjectManager.ObjectManager.Me.WowSpecialization(false)));
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

