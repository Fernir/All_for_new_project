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
    using System.Windows.Forms;

    public class DungeonFarming : nManager.FiniteStateMachine.State
    {
        private List<Instance> _instanceList;

        public override void Run()
        {
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
                return "DungeonFarming";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if ((((Usefuls.InGame && !Usefuls.IsLoadingOrConnecting) && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) && (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)))) && nManager.Products.Products.IsStarted)
                {
                    if (this._instanceList == null)
                    {
                        this._instanceList = XmlSerializer.Deserialize<List<Instance>>(Application.StartupPath + @"\Data\DfInstanceList.xml");
                    }
                    int count = this._instanceList.Count;
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

