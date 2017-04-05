namespace nManager.Wow.Bot.States
{
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PositionTrackingSystem : nManager.FiniteStateMachine.State
    {
        private readonly Timer _canWrite = new Timer(5000.0);
        private string _currPosition;
        private string _lastPosition;

        public override void Run()
        {
            Logging.WriteFileOnly(this._currPosition);
            this._canWrite.Reset();
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
                return "PositionTrackingSystem";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting)
                {
                    return false;
                }
                this._currPosition = string.Format("CurrentPositionTracker({0}, {1}, {2}, {3});", new object[] { nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Type });
                if (this._currPosition == this._lastPosition)
                {
                    return false;
                }
                this._lastPosition = this._currPosition;
                return this._canWrite.IsReady;
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

