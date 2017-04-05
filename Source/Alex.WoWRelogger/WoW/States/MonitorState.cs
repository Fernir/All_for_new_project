using Alex.WoWRelogger;
using Alex.WoWRelogger.FiniteStateMachine;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using HighVoltz.HBRelog.Source.WoW;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Alex.WoWRelogger.WoW.States
{
	internal class MonitorState : State
	{
		private readonly WowManager _wowManager;

		private int _priority = 100;

		private DateTime _crashTimeStamp = DateTime.Now;

		private readonly Stopwatch _loggedOutSw = new Stopwatch();

		private DateTime _loggedoutTimeStamp = DateTime.Now;

		private bool _wowIsLoggedOutForTooLong;

		private readonly Stopwatch _wowRespondingSw = new Stopwatch();

		public override bool NeedToRun
		{
			get
			{
				if (this._wowManager.GameProcess == null || this._wowManager.GameProcess.HasExitedSafe())
				{
					return false;
				}
				return this._wowManager.InGame;
			}
		}

		public override int Priority
		{
			get
			{
				return this._priority;
			}
			set
			{
				this._priority = value;
			}
		}

		public bool WowHasCrashed
		{
			get
			{
				bool flag;
				if ((DateTime.Now - this._crashTimeStamp) >= TimeSpan.FromSeconds(10))
				{
					try
					{
						if (!this._wowManager.GameProcess.HasExitedSafe())
						{
							this._crashTimeStamp = DateTime.Now;
							if (!NativeMethods.EnumerateProcessWindowHandles(this._wowManager.GameProcess.Id).Select<IntPtr, string>(new Func<IntPtr, string>(NativeMethods.GetWindowText)).Any<string>((string caption) => caption == "Wow"))
							{
								return false;
							}
							else
							{
								flag = true;
							}
						}
						else
						{
							flag = true;
						}
					}
					catch (InvalidOperationException invalidOperationException)
					{
						if (!this._wowManager.ProcessIsReadyForInput)
						{
							return false;
						}
						else
						{
							flag = true;
						}
					}
					return flag;
				}
				return false;
			}
		}

		public bool WowIsLoggedOutForTooLong
		{
			get
			{
				if ((DateTime.Now - this._loggedoutTimeStamp) >= TimeSpan.FromSeconds(5))
				{
					if (!this._wowManager.InGame)
					{
						if (!this._loggedOutSw.IsRunning)
						{
							this._loggedOutSw.Start();
						}
						this._wowIsLoggedOutForTooLong = this._loggedOutSw.ElapsedMilliseconds >= (long)40000;
						if (this._wowIsLoggedOutForTooLong)
						{
							this._loggedOutSw.Reset();
						}
					}
					else if (this._loggedOutSw.IsRunning)
					{
						this._loggedOutSw.Reset();
					}
					this._loggedoutTimeStamp = DateTime.Now;
				}
				return this._wowIsLoggedOutForTooLong;
			}
		}

		public bool WowIsUnresponsive
		{
			get
			{
				bool flag;
				try
				{
					bool responding = this._wowManager.GameProcess.Responding;
					if (this._wowManager.GameProcess != null && !this._wowManager.GameProcess.HasExitedSafe() && !this._wowManager.GameProcess.Responding)
					{
						if (!this._wowRespondingSw.IsRunning)
						{
							this._wowRespondingSw.Start();
						}
						if (this._wowRespondingSw.ElapsedMilliseconds >= (long)20000)
						{
							flag = true;
							return flag;
						}
					}
					else if (responding && this._wowRespondingSw.IsRunning)
					{
						this._wowRespondingSw.Reset();
					}
					return false;
				}
				catch (InvalidOperationException invalidOperationException)
				{
					if (!this._wowManager.ProcessIsReadyForInput)
					{
						return false;
					}
					else
					{
						flag = true;
					}
				}
				return flag;
			}
		}

		public MonitorState(WowManager wowManager)
		{
			this._wowManager = wowManager;
		}

		private MonitorState.WowProblem FindTrouble()
		{
			if (!this._wowManager.StartupSequenceIsComplete)
			{
				return MonitorState.WowProblem.None;
			}
			if (this._wowManager.GlueScreen == GlueScreen.Login)
			{
				return MonitorState.WowProblem.Disconnected;
			}
			if (this.WowIsLoggedOutForTooLong)
			{
				return MonitorState.WowProblem.LoggedOutForTooLong;
			}
			if (HbRelogManager.Settings.CheckWowResponsiveness && this.WowIsUnresponsive)
			{
				return MonitorState.WowProblem.Unresponsive;
			}
			if (this.WowHasCrashed)
			{
				return MonitorState.WowProblem.Crash;
			}
			return MonitorState.WowProblem.None;
		}

		public override void Run()
		{
			this._wowManager.JustCreatedCharacter = "";
			if (!this._wowManager.StartupSequenceIsComplete)
			{
				this._wowManager.NeedToDisconnect = true;
				this._wowManager.SetStartupSequenceToComplete();
				this._loggedOutSw.Reset();
				this._wowIsLoggedOutForTooLong = false;
				this._wowManager.IsCustomLuaRunned = false;
			}
			if (this._wowManager.NeedReload)
			{
				this._wowManager.LockToken.DoString("ReloadUI()");
				this._wowManager.NeedReload = false;
			}
			if (this._wowManager.InGame)
			{
				string paramValue = this._wowManager.Settings.GetParamValue<string>("customLua");
				if (!string.IsNullOrEmpty(paramValue))
				{
					this._wowManager.LockToken.DoString(paramValue);
				}
				this._wowManager.AidaHandler.Check();
			}
			MonitorState.WowProblem wowProblem = this.FindTrouble();
			if (wowProblem == MonitorState.WowProblem.None)
			{
				return;
			}
			switch (wowProblem)
			{
				case MonitorState.WowProblem.Disconnected:
				{
					this._wowManager.Profile.Log("WoW has disconnected.. So lets restart WoW", new object[0]);
					this._wowManager.Profile.Status = "WoW has DCed. restarting";
					break;
				}
				case MonitorState.WowProblem.LoggedOutForTooLong:
				{
					this._wowManager.Profile.Log("Restarting wow because it was logged out for more than 40 seconds", new object[0]);
					this._wowManager.Profile.Status = "WoW was logged out for too long. restarting";
					break;
				}
				case MonitorState.WowProblem.Unresponsive:
				{
					this._wowManager.Profile.Status = "WoW is not responding. restarting";
					this._wowManager.Profile.Log("WoW is not responding.. So lets restart WoW", new object[0]);
					break;
				}
				case MonitorState.WowProblem.Crash:
				{
					this._wowManager.Profile.Status = "WoW has crashed. restarting";
					this._wowManager.Profile.Log("WoW has crashed.. So lets restart WoW", new object[0]);
					break;
				}
			}
			this._wowManager.CloseGameProcess();
			this._wowManager.CloseUnlockerProcess();
		}

		private enum WowProblem
		{
			None,
			Disconnected,
			LoggedOutForTooLong,
			Unresponsive,
			Crash
		}
	}
}