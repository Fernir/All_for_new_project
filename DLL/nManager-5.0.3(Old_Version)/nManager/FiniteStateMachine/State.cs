namespace nManager.FiniteStateMachine
{
    using nManager.Helpful;
    using System;
    using System.Collections.Generic;

    public abstract class State : IComparable<nManager.FiniteStateMachine.State>, IComparer<nManager.FiniteStateMachine.State>
    {
        protected State()
        {
        }

        public int Compare(nManager.FiniteStateMachine.State x, nManager.FiniteStateMachine.State y)
        {
            try
            {
                return -x.Priority.CompareTo(y.Priority);
            }
            catch (Exception exception)
            {
                Logging.WriteError("State > Compare(State x, State y): " + exception, true);
            }
            return 0;
        }

        public int CompareTo(nManager.FiniteStateMachine.State other)
        {
            try
            {
                return -this.Priority.CompareTo(other.Priority);
            }
            catch (Exception exception)
            {
                Logging.WriteError("State > CompareTo(State other): " + exception, true);
            }
            return 0;
        }

        public abstract void Run();

        public abstract List<nManager.FiniteStateMachine.State> BeforeStates { get; }

        public abstract string DisplayName { get; }

        public abstract bool NeedToRun { get; }

        public abstract List<nManager.FiniteStateMachine.State> NextStates { get; }

        public abstract int Priority { get; set; }
    }
}

