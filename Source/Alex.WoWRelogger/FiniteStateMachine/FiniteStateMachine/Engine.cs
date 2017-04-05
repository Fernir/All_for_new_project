using Alex.WoWRelogger.FiniteStateMachine;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Alex.WoWRelogger.FiniteStateMachine.FiniteStateMachine
{
	internal class Engine
	{
		private List<State> _states;

		public virtual bool IsRunning
		{
			get;
			protected set;
		}

		public List<State> States
		{
			get
			{
				return this._states;
			}
			protected set
			{
				this._states = value;
				if (this._states != null)
				{
					this._states.Sort();
				}
			}
		}

		protected Engine(List<State> states = null)
		{
			this.States = states ?? new List<State>();
		}

		public virtual void Pulse()
		{
			foreach (State state in this.States)
			{
				if (!state.NeedToRun)
				{
					continue;
				}
				state.Run();
				return;
			}
		}
	}
}