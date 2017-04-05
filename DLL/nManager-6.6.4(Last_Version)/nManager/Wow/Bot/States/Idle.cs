namespace nManager.Wow.Bot.States
{
    using nManager.FiniteStateMachine;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class Idle : nManager.FiniteStateMachine.State
    {
        public int TimeSleepMs = 60;

        public override void Run()
        {
            Thread.Sleep(this.TimeSleepMs);
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
                return "Idle";
            }
        }

        public override bool NeedToRun
        {
            get
            {
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

