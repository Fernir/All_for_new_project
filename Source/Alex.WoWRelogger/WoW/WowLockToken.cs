using Alex.WoWRelogger;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using iRobot;
using Microsoft.Win32;
using nManager.Wow;
using nManager.Wow.Helpers;
using nManager.Wow.MemoryClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Alex.WoWRelogger.WoW
{
	internal class WowLockToken : IDisposable
	{
		private readonly DateTime _startTime;

		private WowManager _lockOwner;

		private readonly string _key;

		private System.Diagnostics.Process _wowProcess;

		private System.Diagnostics.Process _unlockerProcess;

		public static object startWowMutex;

		private bool _useInjector;

		public iRobot.Hook nHook;

		public nManager.Wow.MemoryClass.Hook nHook2;

		private nManager.Wow.MemoryClass.Process nProcess2;

		private readonly static Dictionary<string, WowLockToken> LockInfos;

		private static DateTime _lastLockTime;

		public bool IsValid
		{
			get
			{
				if (this._lockOwner == null || !WowLockToken.LockInfos.ContainsKey(this._key))
				{
					return false;
				}
				return WowLockToken.LockInfos[this._key]._lockOwner == this._lockOwner;
			}
		}

		static WowLockToken()
		{
			WowLockToken.startWowMutex = new object();
			WowLockToken.LockInfos = new Dictionary<string, WowLockToken>();
		}

		private WowLockToken(string key, DateTime startTime, WowManager lockOwner)
		{
			this._startTime = startTime;
			this._lockOwner = lockOwner;
			this._key = key;
		}

		private static byte[] AddressToBytes(long address)
		{
			return new byte[] { (byte)(address % (long)256), (byte)(address / (long)256 % (long)256), (byte)(address / (long)65536 % (long)256), (byte)(address / (long)16777216) };
		}

		private void AdjustWoWConfig()
		{
			string str = Path.Combine(Path.GetDirectoryName(this._lockOwner.Settings.WowPath), "Wtf\\Config.wtf");
			if (!File.Exists(str))
			{
				this._lockOwner.Profile.Log("Warning: Unable to find Wow's config.wtf file. Editing this file speeds up the login process.", new object[0]);
				return;
			}
			ConfigWtf configWtf = new ConfigWtf(this._lockOwner, str);
			if (HbRelogManager.Settings.AutoAcceptTosEula)
			{
				configWtf.EnsureValue("readTOS", "1");
				configWtf.EnsureValue("readEULA", "1");
			}
			configWtf.EnsureValue("accountName", this._lockOwner.Settings.Login);
			if (string.IsNullOrEmpty(this._lockOwner.Settings.AccountName))
			{
				configWtf.DeleteSetting("accountList");
			}
			else
			{
				configWtf.EnsureAccountList(this._lockOwner.Settings.AccountName);
			}
			if (configWtf.Changed)
			{
				configWtf.Save();
			}
		}

		public bool CheckProxy(string proxy)
		{
			if (proxy == "")
			{
				return true;
			}
			Match match = (new Regex("^((\\d{1,3}\\.){3}\\d{1,3})\\:(\\d{1,6})$")).Match(proxy);
			if (!match.Success)
			{
				CharacterProfile profile = this._lockOwner.Profile;
				profile.Stop();
				profile.Status = "Wrong proxy format";
				return false;
			}
			string value = match.Groups[1].Value;
			string str = match.Groups[3].Value;
			this._lockOwner.Profile.Status = string.Concat("Cheking proxy ", proxy);
			if (!WowLockToken.CheckSock5(value, str))
			{
				return false;
			}
			this._lockOwner.Profile.Status = "Passed successfully";
			return true;
		}

		public static bool CheckSock4(string ip, string port)
		{
			bool flag = false;
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				socket.BeginConnect(ip, int.Parse(port), null, null).AsyncWaitHandle.WaitOne(1000, true);
				if (socket.Connected)
				{
					string str = "god_912";
					string str1 = "absolute123";
					byte[] bytes = new byte[9 + str.Length + str1.Length + 1];
					bytes[0] = 4;
					bytes[1] = 1;
					bytes[2] = 4;
					bytes[3] = 95;
					bytes[4] = 178;
					bytes[5] = 79;
					bytes[6] = 213;
					bytes[7] = 103;
					Array.Copy(Encoding.ASCII.GetBytes(str), 0, bytes, 8, str.Length);
					bytes[8 + str.Length] = Encoding.ASCII.GetBytes(":")[0];
					Array.Copy(Encoding.ASCII.GetBytes(str1), 0, bytes, 9 + str.Length, str1.Length);
					bytes[(int)bytes.Length - 1] = 0;
					if (socket.BeginSend(bytes, 0, (int)bytes.Length, SocketFlags.None, null, null).AsyncWaitHandle.WaitOne(500, true))
					{
						byte[] numArray = new byte[8];
						if (socket.BeginReceive(numArray, 0, 8, SocketFlags.None, null, null).AsyncWaitHandle.WaitOne(500, true) && numArray[1] == 90)
						{
							flag = true;
						}
					}
				}
				socket.Close();
			}
			catch (Exception exception)
			{
				flag = false;
				if (socket.Connected)
				{
					socket.Close();
				}
			}
			return flag;
		}

		public static bool CheckSock5(string ip, string port)
		{
			bool flag = false;
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				socket.BeginConnect(ip, int.Parse(port), null, null).AsyncWaitHandle.WaitOne(1000, true);
				if (socket.Connected)
				{
					byte[] length = new byte[] { 5, 1, 2 };
					if (socket.BeginSend(length, 0, (int)length.Length, SocketFlags.None, null, null).AsyncWaitHandle.WaitOne(500, true))
					{
						byte[] numArray = new byte[2];
						if (socket.BeginReceive(numArray, 0, (int)numArray.Length, SocketFlags.None, null, null).AsyncWaitHandle.WaitOne(500, true) && numArray[1] == 2)
						{
							string str = "LEBASU";
							string str1 = "LEBASU";
							length = new byte[3 + str.Length + str1.Length];
							length[0] = 1;
							length[1] = (byte)str.Length;
							Array.Copy(Encoding.ASCII.GetBytes(str), 0, length, 2, str.Length);
							length[2 + str.Length] = (byte)str1.Length;
							Array.Copy(Encoding.ASCII.GetBytes(str1), 0, length, 3 + str.Length, str1.Length);
							if (socket.BeginSend(length, 0, (int)length.Length, SocketFlags.None, null, null).AsyncWaitHandle.WaitOne(500, true))
							{
								numArray = new byte[2];
								if (socket.BeginReceive(numArray, 0, (int)numArray.Length, SocketFlags.None, null, null).AsyncWaitHandle.WaitOne(500, true) && numArray[1] == 0)
								{
									length = new byte[] { 5, 1, 0, 3, 14, 49, 55, 56, 46, 54, 50, 46, 50, 50, 56, 46, 49, 48, 57, 0, 80 };
									if (socket.BeginSend(length, 0, (int)length.Length, SocketFlags.None, null, null).AsyncWaitHandle.WaitOne(500, true))
									{
										numArray = new byte[21];
										if (socket.BeginReceive(numArray, 0, (int)length.Length, SocketFlags.None, null, null).AsyncWaitHandle.WaitOne(500, true) && numArray[1] == 0)
										{
											flag = true;
										}
									}
								}
							}
						}
					}
				}
				socket.Close();
			}
			catch (Exception exception)
			{
				flag = false;
				if (socket.Connected)
				{
					socket.Close();
				}
			}
			return flag;
		}

		public void Dispose()
		{
			if (Monitor.IsEntered(WowLockToken.startWowMutex))
			{
				Monitor.Exit(WowLockToken.startWowMutex);
			}
		}

		[HandleProcessCorruptedStateExceptions]
		public void DoString(string cmd)
		{
			if (!this._useInjector)
			{
				return;
			}
			lock ("123")
			{
				Memory.WowProcess = this.nProcess2;
				Memory.WowMemory = new nManager.Wow.MemoryClass.Hook();
				try
				{
					Lua.LuaDoString(cmd, !this._lockOwner.InGame, false);
				}
				catch (Exception exception)
				{
					Alex.WoWRelogger.Utility.Log.Err(string.Concat("nManager exception, command: ", cmd), new object[0]);
				}
			}
		}

		[HandleProcessCorruptedStateExceptions]
		public string DoStringWithReturn(string cmd, string var)
		{
			string luaObject;
			if (!this._useInjector)
			{
				return "";
			}
			lock ("123")
			{
				Memory.WowProcess = this.nProcess2;
				Memory.WowMemory = new nManager.Wow.MemoryClass.Hook();
				try
				{
					Lua.LuaDoString(cmd, !this._lockOwner.InGame, false);
				}
				catch (Exception exception)
				{
					Alex.WoWRelogger.Utility.Log.Err(string.Concat("nManager exception, command: ", cmd), new object[0]);
				}
				luaObject = this._lockOwner.GetLuaObject(var);
			}
			return luaObject;
		}

		private bool IsCmdProcess(System.Diagnostics.Process proc)
		{
			return proc.ProcessName.Equals("cmd", StringComparison.CurrentCultureIgnoreCase);
		}

		private bool IsWoWProcess(System.Diagnostics.Process proc)
		{
			if (proc.ProcessName.Equals("Wow", StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
			return proc.ProcessName.Equals("Wow-64", StringComparison.CurrentCultureIgnoreCase);
		}

		public void LuaDoString(string command)
		{
		}

		private static byte[] PortToBytes(int port)
		{
			return new byte[] { (byte)(port / 256), (byte)(port % 256) };
		}

		public void ProcessUnlockerWindow(object o)
		{
			System.Diagnostics.Process process = o as System.Diagnostics.Process;
			while (this._unlockerProcess.MainWindowHandle == IntPtr.Zero)
			{
			}
			Regex regex = new Regex("(\\d+):\\s+0x([0-9a-zA-Z]+)");
			while (!this._unlockerProcess.HasExitedSafe() && !this._unlockerProcess.StandardOutput.EndOfStream)
			{
				string str = this._unlockerProcess.StandardOutput.ReadLine();
				Match match = regex.Match(str);
				if (!match.Success)
				{
					continue;
				}
				Group item = match.Groups[1];
				Group group = match.Groups[2];
				if (process.Id != int.Parse(group.Value, NumberStyles.AllowHexSpecifier))
				{
					continue;
				}
				this._unlockerProcess.StandardInput.Write(item.Value);
				this._unlockerProcess.StandardInput.Write(Environment.NewLine);
				Helper.ShowWindow(this._unlockerProcess.MainWindowHandle, Alex.WoWRelogger.Utility.NativeMethods.ShowWindowCommands.Hide);
				return;
			}
		}

		public void ReleaseLock()
		{
			this.Dispose();
		}

		public static WowLockToken RequestLock(WowManager wowManager, out string reason)
		{
			reason = string.Empty;
			string profileName = wowManager.Profile.Settings.ProfileName;
			DateTime now = DateTime.Now;
			if ((now - WowLockToken._lastLockTime) < TimeSpan.FromSeconds((double)HbRelogManager.Settings.WowDelay))
			{
				reason = "Waiting to start WoW";
				return null;
			}
			if (WowLockToken.LockInfos.ContainsKey(profileName))
			{
				WowLockToken item = WowLockToken.LockInfos[profileName];
				if ((item._lockOwner == null ? false : item._lockOwner != wowManager))
				{
					reason = string.Format("Waiting on profile: {0} to release lock", item._lockOwner.Profile.Settings.ProfileName);
					return null;
				}
			}
			WowLockToken._lastLockTime = now;
			Dictionary<string, WowLockToken> lockInfos = WowLockToken.LockInfos;
			WowLockToken wowLockToken = new WowLockToken(profileName, DateTime.Now, wowManager);
			WowLockToken wowLockToken1 = wowLockToken;
			lockInfos[profileName] = wowLockToken;
			return wowLockToken1;
		}

		public void StartWoW()
		{
			if (this._lockOwner.GameProcess != null)
			{
				this._lockOwner.CloseGameProcess();
			}
			if (this._lockOwner.UnlockerProcesss != null)
			{
				this._lockOwner.CloseUnlockerProcess();
			}
			HbRelogManager.ReloadProxyList();
			string str = this._lockOwner.Settings.Proxy.Trim();
			Regex regex = new Regex("^((\\d{1,3}\\.){3}\\d{1,3})\\:(\\d{1,6})$");
			Match match = regex.Match(str);
			string randomProxy = HbRelogManager.GetRandomProxy();
			if (!match.Success)
			{
				str = randomProxy;
				match = regex.Match(randomProxy);
			}
			if (!this._lockOwner.Settings.GetParamValue<bool>("useProxy"))
			{
				str = "";
			}
			this._lockOwner.innerProxy = str;
			Monitor.Enter(WowLockToken.startWowMutex);
			Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Blizzard Entertainment", false);
			this.AdjustWoWConfig();
			this._lockOwner.Profile.Log("Starting {0}", new object[] { this._lockOwner.Settings.WowPath });
			this._lockOwner.Profile.Status = "Starting WoW";
			this._lockOwner.StartupSequenceIsComplete = false;

            //todo
          //  int num =
		        //(int)
		        //    ((this._lockOwner.Settings.WowPath.IndexOf("WoW.exe", StringComparison.InvariantCultureIgnoreCase) != -1 ||
		        //      this._lockOwner.Settings.WowPath.IndexOf("WoW-64.exe", StringComparison.InvariantCultureIgnoreCase) !=
		        //      -1 ||
		        //      this._lockOwner.Settings.WowPath.IndexOf("WoWB.exe", StringComparison.InvariantCultureIgnoreCase) != -1
		        //        ? true
		        //        : this._lockOwner.Settings.WowPath.IndexOf("WoWT.exe", StringComparison.InvariantCultureIgnoreCase) !=
		        //          -1));
			ProcessStartInfo processStartInfo = new ProcessStartInfo()
			{
				UseShellExecute = false,
				FileName = this._lockOwner.Settings.WowPath,
				Arguments = "-noautolaunch64bit"
			};
			this._wowProcess = Alex.WoWRelogger.Utility.NativeMethods.StartSuspended(processStartInfo.FileName, processStartInfo.Arguments);
			this._lockOwner.ProcessIsReadyForInput = false;
			this._lockOwner.LoginTimer.Reset();
			this._lockOwner.Profile.Status = "Waiting for Wow to start";
			this._lockOwner.Profile.Log(this._lockOwner.Profile.Status, new object[0]);
			if (!string.IsNullOrEmpty(str))
			{
				string value = match.Groups[1].Value;
				string value1 = match.Groups[3].Value;
				processStartInfo = new ProcessStartInfo()
				{
					UseShellExecute = false,
					CreateNoWindow = true,
					FileName = Path.Combine(Helper.AssemblyDirectory, "Injector.exe"),
					Arguments = string.Format("{0} {1} {2}", this._wowProcess.Id, value, value1)
				};
				System.Diagnostics.Process.Start(processStartInfo);
				this._lockOwner.Profile.Log("Using proxy: {0}", new object[] { str });
			}
			Thread.Sleep(2000);
			Alex.WoWRelogger.Utility.NativeMethods.ResumeProcess(this._wowProcess);
			int num1 = 0;
			while (this._wowProcess.MainWindowHandle == IntPtr.Zero)
			{
				this._wowProcess.Refresh();
				Thread.Sleep(3000);
				num1 = num1 + 100;
				if (num1 <= 31000)
				{
					continue;
				}
				Monitor.Exit(WowLockToken.startWowMutex);
				this._wowProcess.Kill();
				return;
			}
			if (this._lockOwner.Profile.Settings.WowSettings.GetParamValue<bool>("useUnlocker"))
			{
				processStartInfo = new ProcessStartInfo()
				{
					UseShellExecute = false,
					CreateNoWindow = true,
					FileName = Path.Combine(Helper.AssemblyDirectory, "Unlocker.exe"),
					Arguments = this._wowProcess.Id.ToString()
				};
				this._unlockerProcess = System.Diagnostics.Process.Start(processStartInfo);
			}
			this._lockOwner.GameProcess = this._wowProcess;
			this._lockOwner.UnlockerProcesss = this._unlockerProcess;
			this._lockOwner.Profile.Log("Wow is ready to login.", new object[0]);
			this.nHook = new iRobot.Hook(this._wowProcess, this._lockOwner);
			this._useInjector = this._lockOwner.Settings.GetParamValue<bool>("useInjector");
			if (this._useInjector)
			{
				this.nProcess2 = new nManager.Wow.MemoryClass.Process(this._wowProcess.Id);
				Memory.WowProcess = this.nProcess2;
				this.nHook2 = new nManager.Wow.MemoryClass.Hook();
			}
			Monitor.Exit(WowLockToken.startWowMutex);
			this._lockOwner.Profile.Log("Exit WowLockToken:StartWoW()", new object[0]);
		}
	}
}