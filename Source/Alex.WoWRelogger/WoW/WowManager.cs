using Alex.WoWRelogger;
using Alex.WoWRelogger.FiniteStateMachine;
using Alex.WoWRelogger.FiniteStateMachine.FiniteStateMachine;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW.States;
using GreyMagic;
using HighVoltz.HBRelog.CleanPattern;
using HighVoltz.HBRelog.Source.DB;
using HighVoltz.HBRelog.Source.WoW;
using HighVoltz.HBRelog.WoW.Lua;
using iRobot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace Alex.WoWRelogger.WoW
{
	internal class WowManager : Engine
	{
		private readonly object _lockObject = new object();

		internal readonly Stopwatch LoginTimer = new Stopwatch();

		private bool _isExiting;

		private Alex.WoWRelogger.WoW.GlueScreen _lastGlueStatus;

		private DateTime _throttleTimeStamp = DateTime.Now;

		internal bool ProcessIsReadyForInput;

		private CharacterProfile _profile;

		private int _unlockerCloseAttempt;

		private Timer _unlockerCloseTimer;

		private bool _isUnlockerExiting;

		public bool NeedReload;

		public readonly static Pattern LuaStatePattern;

		private int _errorCount;

		private Alex.WoWRelogger.WoW.GlueScreen _glueScreen;

		private byte[] buffer = new byte[4];

		public string JustCreatedCharacter = "";

		internal string innerProxy;

		public AidaProtocolHandler AidaHandler
		{
			get;
			private set;
		}

		public Process GameProcess
		{
			get;
			internal set;
		}

		public LuaTable Globals
		{
			get
			{
				if (this.LockToken == null || this.LockToken.nHook == null || !this.LockToken.nHook.Memory.IsProcessOpen)
				{
					return null;
				}
				if (HbRelogManager.Settings.LuaStateOffset == 0)
				{
					HbRelogManager.Settings.LuaStateOffset = (uint)WowManager.LuaStatePattern.Find(this.LockToken.nHook.Memory);
				}
				IntPtr intPtr = this.LockToken.nHook.Memory.Read<IntPtr>((IntPtr)((ulong)HbRelogManager.Settings.LuaStateOffset), true);
				if (intPtr == IntPtr.Zero)
				{
					this.Profile.Err("Lua state is not initialized", new object[0]);
					HbRelogManager.Settings.LuaStateOffset = 0;
					this._errorCount = this._errorCount + 1;
					return null;
				}
				IntPtr intPtr1 = this.LockToken.nHook.Memory.Read<IntPtr>(intPtr + 80, false);
				if (intPtr1 != IntPtr.Zero)
				{
					this._errorCount = 0;
					return new LuaTable(this.LockToken.nHook.Memory, intPtr1);
				}
				this.Profile.Err("Lua globals is not initialized", new object[0]);
				this._errorCount = this._errorCount + 1;
				return null;
			}
		}

		public Alex.WoWRelogger.WoW.GlueScreen GlueScreen
		{
			get
			{
				if (this.InGame)
				{
					return Alex.WoWRelogger.WoW.GlueScreen.None;
				}
				if (!this.NeedGlueScreenUpdate)
				{
					return this._glueScreen;
				}
				string luaObject = this.GetLuaObject("GlueParent.currentScreen");
				this._glueScreen = Alex.WoWRelogger.WoW.GlueScreen.None;
				if (!string.IsNullOrEmpty(luaObject))
				{
					if (luaObject == "login")
					{
						this._glueScreen = Alex.WoWRelogger.WoW.GlueScreen.Login;
					}
					else if (luaObject == "realmlist")
					{
						this._glueScreen = Alex.WoWRelogger.WoW.GlueScreen.RealmList;
					}
					else if (luaObject == "charselect")
					{
						this._glueScreen = Alex.WoWRelogger.WoW.GlueScreen.CharSelect;
					}
					else if (luaObject == "charcreate")
					{
						this._glueScreen = Alex.WoWRelogger.WoW.GlueScreen.CharCreate;
					}
				}
				this.NeedGlueScreenUpdate = false;
				return this._glueScreen;
			}
		}

		public bool InGame
		{
			get
			{
				bool luaObjectRaw;
				try
				{
					luaObjectRaw = this.GetLuaObjectRaw("DEADLY_INGAME") != null;
				}
				catch
				{
					luaObjectRaw = false;
				}
				return luaObjectRaw;
			}
		}

		public bool IsConnectiongOrLoading
		{
			get
			{
				bool num;
				try
				{
					int num1 = 0;
					NativeMethods.ReadProcessMemory((int)this.GameProcess.Handle, (int)this.GameProcess.MainModule.BaseAddress + 15982318 + 1, this.buffer, 1, ref num1);
					num = BitConverter.ToUInt32(this.buffer, 0) != 0;
				}
				catch
				{
					num = false;
				}
				return num;
			}
		}

		public bool IsCustomLuaRunned
		{
			get;
			set;
		}

		public WowLockToken LockToken
		{
			get;
			internal set;
		}

		public bool NeedGlueScreenUpdate
		{
			get;
			set;
		}

		public bool NeedToDisconnect
		{
			get;
			internal set;
		}

		public CharacterProfile Profile
		{
			get
			{
				return this._profile;
			}
			private set
			{
				this._profile = value;
				this.Settings = value.Settings.WowSettings;
			}
		}

		public bool ServerHasQueue
		{
			get
			{
				bool flag;
				try
				{
					if (!this.InGame)
					{
						string str = this.LockToken.DoStringWithReturn("ret = \"\"; if GlueDialogButton1 ~= nil and GlueDialogButton1:IsVisible() then ret = GlueDialogButton1:GetText(); end", "ret");
						string luaObject = this.GetLuaObject("CHANGE_REALM");
						flag = (luaObject == null ? false : luaObject == str);
					}
					else
					{
						flag = false;
					}
				}
				catch (Exception exception)
				{
					flag = false;
				}
				return flag;
			}
		}

		public bool ServerIsOnline
		{
			get
			{
				if (!HbRelogManager.Settings.CheckRealmStatus)
				{
					return true;
				}
				return HbRelogManager.WowRealmStatus.RealmIsOnline(this.Settings.ServerName, this.Settings.Region);
			}
		}

		public WowSettings Settings
		{
			get;
			private set;
		}

		public bool StalledLogin
		{
			get
			{
				if (this.ServerIsOnline && !this.ServerHasQueue)
				{
					Alex.WoWRelogger.WoW.GlueScreen glueScreen = this.GlueScreen;
					if (glueScreen == this._lastGlueStatus)
					{
						if (!this.LoginTimer.IsRunning)
						{
							this.LoginTimer.Start();
						}
						if (this.LoginTimer.ElapsedMilliseconds > (long)40000)
						{
							this._lastGlueStatus = Alex.WoWRelogger.WoW.GlueScreen.None;
							return true;
						}
					}
					else if (this.LoginTimer.IsRunning)
					{
						this.LoginTimer.Reset();
					}
					this._lastGlueStatus = glueScreen;
				}
				return false;
			}
		}

		public bool StartupSequenceIsComplete
		{
			get;
			internal set;
		}

		public bool Throttled
		{
			get
			{
				DateTime now = DateTime.Now;
				bool flag = (now - this._throttleTimeStamp) < TimeSpan.FromSeconds((double)HbRelogManager.Settings.LoginDelay);
				if (!flag)
				{
					this._throttleTimeStamp = now;
				}
				return flag;
			}
		}

		public Process UnlockerProcesss
		{
			get;
			internal set;
		}

		static WowManager()
		{
			WowManager.LuaStatePattern = Pattern.FromTextstyle("LuaState", "? ? ? ? 6a 00 ff 75 0c 56 e8 ? ? ? ? ff 75 08 56 e8 ? ? ? ? 6a fe 56 e8 ? ? ? ? 68 ? ? ? ? 56 e8 ? ? ? ? 83 c4 24 5e 5d c3", new IModifier[] { new LeaModifier() });
		}

		public WowManager(CharacterProfile profile) : base(null)
		{
			this.Profile = profile;
			base.States = new List<State>()
			{
				new StartWowState(this),
				new WowWindowPlacementState(this),
				new LoginWowState(this),
				new RealmSelectState(this),
				new CharacterSelectState(this),
				new CharacterCreationState(this),
				new MonitorState(this)
			};
			this.AidaHandler = new AidaProtocolHandler(this);
		}

		public void CloseGameProcess()
		{
			try
			{
				this.CloseGameProcess(this.GameProcess);
			}
			catch (InvalidOperationException invalidOperationException)
			{
				Alex.WoWRelogger.Utility.Log.Err(invalidOperationException.ToString(), new object[0]);
				if (this.GameProcess != null)
				{
					this.CloseGameProcess(Process.GetProcessById(this.GameProcess.Id));
				}
			}
			this.GameProcess = null;
		}

		private void CloseGameProcess(Process proc)
		{
			if (!this._isExiting && proc != null && !proc.HasExitedSafe())
			{
				this._isExiting = true;
				this.Profile.Log("Attempting to close Wow", new object[0]);
				proc.CloseMainWindow();
				this.Profile.Log("Killing Wow", new object[0]);
				proc.Kill();
				this._isExiting = false;
			}
		}

		public void CloseUnlockerProcess()
		{
			try
			{
				this.CloseUnlockerProcess(this.UnlockerProcesss);
			}
			catch (InvalidOperationException invalidOperationException)
			{
				Alex.WoWRelogger.Utility.Log.Err(invalidOperationException.ToString(), new object[0]);
			}
			this.UnlockerProcesss = null;
		}

		private void CloseUnlockerProcess(Process proc)
		{
			if (!this._isUnlockerExiting && proc != null && !proc.HasExitedSafe())
			{
				this._isUnlockerExiting = true;
				this.Profile.Log("Attempting to close Unlocker", new object[0]);
				proc.CloseMainWindow();
				this._unlockerCloseAttempt = this._unlockerCloseAttempt + 1;
				this._unlockerCloseTimer = new Timer((object state) => {
					if (((Process)state).HasExitedSafe())
					{
						this._isUnlockerExiting = false;
						this.Profile.Log("Successfully closed Unlocker", new object[0]);
						this._unlockerCloseTimer.Dispose();
						this._unlockerCloseAttempt = 0;
					}
					else
					{
						WowManager u003cu003e4_this = this;
						int num = this._unlockerCloseAttempt;
						u003cu003e4_this._unlockerCloseAttempt = num + 1;
						if (num < 6)
						{
							proc.CloseMainWindow();
							return;
						}
						try
						{
							this.Profile.Log("Killing Unlocker", new object[0]);
							((Process)state).Kill();
						}
						catch
						{
						}
					}
				}, proc, 1000, 1000);
			}
		}

		public string GetLuaObject(string luaAccessorCode)
		{
			LuaTable globals = this.Globals;
			string[] strArrays = luaAccessorCode.Split(new char[] { '.' });
			for (int i = 0; i < (int)strArrays.Length - 1; i++)
			{
				if (globals == null)
				{
					return "";
				}
				LuaTValue value = globals.GetValue(strArrays[i]);
				if (value == null || value.Type != LuaType.Table)
				{
					return "";
				}
				globals = value.Table;
			}
			LuaTValue luaTValue = globals.GetValue(strArrays.Last<string>());
			if (luaTValue == null)
			{
				return "";
			}
			if (luaTValue.Type != LuaType.String)
			{
				return "";
			}
			return luaTValue.String.Value;
		}

		public LuaTValue GetLuaObjectRaw(string luaAccessorCode)
		{
			LuaTable globals = this.Globals;
			string[] strArrays = luaAccessorCode.Split(new char[] { '.' });
			for (int i = 0; i < (int)strArrays.Length - 1; i++)
			{
				if (globals == null)
				{
					return null;
				}
				LuaTValue value = globals.GetValue(strArrays[i]);
				if (value == null || value.Type != LuaType.Table)
				{
					return null;
				}
				globals = value.Table;
			}
			return globals.GetValue(strArrays.Last<string>());
		}

		public override void Pulse()
		{
			if (this.Profile.Account == null)
			{
				Account accountById = null;
				int paramValue = this.Settings.GetParamValue<int>("accountId");
				string str = this.Settings.GetParamValue<string>("accountEmail");
				if (paramValue != 0)
				{
					accountById = AccountManager.GetAccountById(paramValue);
				}
				if (str != null)
				{
					accountById = AccountManager.GetAccountByEmail(str);
				}
				if (accountById == null)
				{
					accountById = AccountManager.GetFreeAccount();
				}
				if (accountById == null)
				{
					if (!HbRelogManager.Settings.AutoCreateAccounts || AccountManager.HaveActivePaidNotRunningAccounts())
					{
						this.Profile.Status = "No free accounts";
						Thread.Sleep(3000);
						return;
					}
					if (Helper.GetWebmoneyBalance() < 276)
					{
						this.Profile.Status = "No money, cant create account";
						Thread.Sleep(30000);
						return;
					}
					this.Profile.Status = "Creating an account";
					AccountCreator.CreateAndPay(null, null, null, null);
					HbRelogManager.UpdateWebmoneyBalance();
					return;
				}
				this.Profile.Account = accountById;
				this.Profile.Settings.WowSettings.AccountName = accountById.AccountName;
				this.Profile.Settings.WowSettings.Login = accountById.Email;
				this.Profile.Settings.WowSettings.Password = accountById.Password;
				this.Profile.Settings.WowSettings.Proxy = (accountById.RegIp != null ? accountById.RegIp : "");
				this.Profile.Account.IsRunning = true;
			}
			if (this._errorCount > 10)
			{
				this.Stop();
				this.Start();
				return;
			}
			this.NeedGlueScreenUpdate = true;
			base.Pulse();
		}

		public void SetSettings(WowSettings settings)
		{
			this.Settings = settings;
		}

		public void SetStartupSequenceToComplete()
		{
			this.StartupSequenceIsComplete = true;
			this.Profile.Log("Login sequence complete", new object[0]);
			this.Profile.Status = "Logged into WoW";
			if (this.OnStartupSequenceIsComplete != null)
			{
				this.OnStartupSequenceIsComplete(this, new ProfileEventArgs(this.Profile));
			}
		}

		public void Start()
		{
			if (File.Exists(this.Settings.WowPath))
			{
				this.IsRunning = true;
				return;
			}
			MessageBox.Show(string.Format("path to WoW.exe does not exist: {0}", this.Settings.WowPath));
		}

		public void Stop()
		{
			if (this.IsRunning)
			{
				if (this.Profile.Account != null)
				{
					this.Profile.Account.IsRunning = false;
					this.Profile.Account = null;
				}
				this.CloseGameProcess();
				this.CloseUnlockerProcess();
				this.IsRunning = false;
				this.StartupSequenceIsComplete = false;
				if (this.LockToken != null)
				{
					this.LockToken.Dispose();
					this.LockToken = null;
				}
			}
		}

		public event EventHandler<ProfileEventArgs> OnStartupSequenceIsComplete;
	}
}