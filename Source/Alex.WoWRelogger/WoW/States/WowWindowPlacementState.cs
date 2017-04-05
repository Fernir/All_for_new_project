using Alex.WoWRelogger;
using Alex.WoWRelogger.FiniteStateMachine;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using System;
using System.Diagnostics;

namespace Alex.WoWRelogger.WoW.States
{
	internal class WowWindowPlacementState : State
	{
		private readonly WowManager _wowManager;

		private int _priority = 800;

		public override bool NeedToRun
		{
			get
			{
				if (this._wowManager.GameProcess == null || this._wowManager.GameProcess.HasExitedSafe() || this._wowManager.StartupSequenceIsComplete || this._wowManager.InGame || this._wowManager.IsConnectiongOrLoading)
				{
					return false;
				}
				return !this._wowManager.ProcessIsReadyForInput;
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

		public WowWindowPlacementState(WowManager wowManager)
		{
			this._wowManager = wowManager;
		}

		public override void Run()
		{
			this._wowManager.Profile.Log("Running state: WowWindowPlacementState", new object[0]);
			if (this._wowManager.Settings.WowWindowWidth > 0 && this._wowManager.Settings.WowWindowHeight > 0)
			{
				this._wowManager.Profile.Log("Setting Window location to X:{0}, Y:{1} and Size to Width {2}, Height:{3}", new object[] { this._wowManager.Settings.WowWindowX, this._wowManager.Settings.WowWindowY, this._wowManager.Settings.WowWindowWidth, this._wowManager.Settings.WowWindowHeight });
				Helper.ResizeAndMoveWindow(this._wowManager.GameProcess.MainWindowHandle, this._wowManager.Settings.WowWindowX, this._wowManager.Settings.WowWindowY, this._wowManager.Settings.WowWindowWidth, this._wowManager.Settings.WowWindowHeight);
			}
			this._wowManager.ProcessIsReadyForInput = true;
		}
	}
}