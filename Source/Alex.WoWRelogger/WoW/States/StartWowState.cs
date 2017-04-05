using Alex.WoWRelogger;
using Alex.WoWRelogger.FiniteStateMachine;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using System;

namespace Alex.WoWRelogger.WoW.States
{
	internal class StartWowState : State
	{
		private readonly WowManager _wowManager;

		private int _priority = 1000;

		public override bool NeedToRun
		{
			get
			{
				if (this._wowManager.GameProcess == null)
				{
					return true;
				}
				return this._wowManager.GameProcess.HasExitedSafe();
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

		public StartWowState(WowManager wowManager)
		{
			this._wowManager = wowManager;
		}

		public override void Run()
		{
			this._wowManager.Profile.Log("Running state: StartWowState", new object[0]);
			string empty = string.Empty;
			if (this._wowManager.LockToken == null || !this._wowManager.LockToken.IsValid)
			{
				this._wowManager.LockToken = WowLockToken.RequestLock(this._wowManager, out empty);
			}
			if (this._wowManager.LockToken == null)
			{
				this._wowManager.Profile.Status = empty;
				return;
			}
			if (this._wowManager.ServerIsOnline)
			{
				this._wowManager.LockToken.StartWoW();
				return;
			}
			this._wowManager.Profile.Status = string.Format("{0} is offline", this._wowManager.Settings.ServerName);
			this._wowManager.Profile.Log("Server is offline", new object[0]);
		}
	}
}