using System;
using System.Collections.Generic;

namespace Alex.WoWRelogger.FiniteStateMachine
{
	internal abstract class State : IComparable<State>, IComparer<State>
	{
		public abstract bool NeedToRun
		{
			get;
		}

		public abstract int Priority
		{
			get;
			set;
		}

		protected State()
		{
		}

		public int Compare(State x, State y)
		{
			return -x.Priority.CompareTo(y.Priority);
		}

		public int CompareTo(State other)
		{
			return -this.Priority.CompareTo(other.Priority);
		}

		public abstract void Run();
	}
}