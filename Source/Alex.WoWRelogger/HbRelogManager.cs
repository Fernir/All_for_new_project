using Alex.WoWRelogger.Controls;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Alex.WoWRelogger
{
	internal class HbRelogManager
	{
		private static DateTime _killWowErrsTimeStamp;

		public static KeyboardHook hook;

		public static List<CharacterProfile> CharactersById;

		public static List<string> ProxyList;

		public static Random r;

		private static bool _shuttingDown;

		private static List<ScheduleUnit> Schedule;

		public static Thread HelperThread
		{
			get;
			private set;
		}

		public static bool IsInitialized
		{
			get;
			private set;
		}

		public static System.Threading.Timer ScheduleTimer
		{
			get;
			private set;
		}

		public static GlobalSettings Settings
		{
			get;
			internal set;
		}

		public static Dictionary<int, Thread> WorkerThreads
		{
			get;
			private set;
		}

		public static Alex.WoWRelogger.WowRealmStatus WowRealmStatus
		{
			get;
			private set;
		}

		static HbRelogManager()
		{
			HbRelogManager._killWowErrsTimeStamp = DateTime.Now;
			HbRelogManager.hook = new KeyboardHook();
			HbRelogManager.CharactersById = new List<CharacterProfile>();
			HbRelogManager.ProxyList = new List<string>();
			HbRelogManager.r = new Random();
			HbRelogManager._shuttingDown = false;
			HbRelogManager.Schedule = new List<ScheduleUnit>();
			try
			{
				Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\Windows Error Reporting", true).SetValue("DontShowUI", 1, RegistryValueKind.DWord);
				Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\Windows Error Reporting", true).SetValue("Disabled", 1, RegistryValueKind.DWord);
				if (MainWindow.Instance != null && !DesignerProperties.GetIsInDesignMode(MainWindow.Instance))
				{
					HbRelogManager.Settings = GlobalSettings.Load(null);
					try
					{
						AccountManager.Reload();
					}
					catch (Exception exception)
					{
					}
					HttpServer.Start();
					AccountManager.CreateRecheckTimer();
					HbRelogManager.ReloadCommands();
					HbRelogManager.hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(HbRelogManager.hook_KeyPressed);
					HbRelogManager.HelperThread = new Thread(new ThreadStart(HbRelogManager.DoHelper));
					HbRelogManager.HelperThread.Start();
					HbRelogManager.ScheduleTimer = new System.Threading.Timer(new TimerCallback(Scheduler.ScheduleThreadStart), HbRelogManager.Schedule, 1000, 1000);
					HbRelogManager.WorkerThreads = new Dictionary<int, Thread>();
					int num = 0;
					foreach (CharacterProfile characterProfile in HbRelogManager.Settings.CharacterProfiles)
					{
						Thread thread = new Thread(() => HbRelogManager.DoWork(characterProfile))
						{
							IsBackground = true,
							Name = characterProfile.Settings.ProfileName
						};
						thread.Start();
						HbRelogManager.CharactersById.Add(characterProfile);
						HbRelogManager.WorkerThreads.Add(characterProfile.Id, thread);
						num++;
					}
					HbRelogManager.hook.RegisterHotKey(ModifierKeys.Control, Keys.T);
					HbRelogManager.hook.RegisterHotKey(ModifierKeys.Control, Keys.R);
					HbRelogManager.WowRealmStatus = new Alex.WoWRelogger.WowRealmStatus();
					if (HbRelogManager.Settings.CheckRealmStatus)
					{
						HbRelogManager.WowRealmStatus.Update();
					}
				}
			}
			catch (Exception exception1)
			{
				System.Windows.MessageBox.Show(exception1.ToString());
			}
		}

		public HbRelogManager()
		{
		}

		private static void DoHelper()
		{
			int tickCount = 0;
			do
			{
				try
				{
					try
					{
						tickCount = Environment.TickCount;
						if (Helper.HasInternetConnection && (DateTime.Now - HbRelogManager._killWowErrsTimeStamp) >= TimeSpan.FromMinutes(1))
						{
							if (HbRelogManager.Settings.CheckRealmStatus)
							{
								HbRelogManager.WowRealmStatus.Update();
							}
							Process[] processesByName = Process.GetProcessesByName("BlizzardError");
							for (int i = 0; i < (int)processesByName.Length; i++)
							{
								processesByName[i].Kill();
								Alex.WoWRelogger.Utility.Log.Write("Killing WowError process", new object[0]);
							}
							HbRelogManager._killWowErrsTimeStamp = DateTime.Now;
						}
					}
					catch (Exception exception)
					{
						Alex.WoWRelogger.Utility.Log.Err(exception.ToString(), new object[0]);
					}
				}
				finally
				{
					int num = 1000 - (Environment.TickCount - tickCount);
					if (num > 0)
					{
						Thread.Sleep(num);
					}
				}
			}
			while (!HbRelogManager._shuttingDown);
		}

		public static void DoWork(CharacterProfile character)
		{
			int tickCount = 0;
			do
			{
				try
				{
					try
					{
						tickCount = Environment.TickCount;
						if (Helper.HasInternetConnection)
						{
							if (character.IsRunning && !character.IsPaused)
							{
								character.Pulse();
							}
							if (character.NeedToStop)
							{
								character.Stop();
								character.NeedToStop = false;
							}
						}
					}
					catch (Exception exception)
					{
						Alex.WoWRelogger.Utility.Log.Err(exception.ToString(), new object[0]);
					}
				}
				finally
				{
					int num = 1000 - (Environment.TickCount - tickCount);
					if (num > 0)
					{
						Thread.Sleep(num);
					}
				}
			}
			while (!HbRelogManager._shuttingDown);
		}

		public static void ExitThread()
		{
			while (true)
			{
				bool flag = true;
				foreach (KeyValuePair<int, Thread> workerThread in HbRelogManager.WorkerThreads)
				{
					if (!workerThread.Value.IsAlive)
					{
						continue;
					}
					flag = false;
				}
				Thread.Sleep(10);
				if (flag)
				{
					Process.GetCurrentProcess().Kill();
				}
			}
		}

		public static Keys GetKeyByNumber(int number)
		{
			return (Keys)(112 + number);
		}

		public static string GetRandomProxy()
		{
			if (HbRelogManager.ProxyList.Count == 0)
			{
				return "";
			}
			int num = 0;
			lock ("lockObj")
			{
				num = HbRelogManager.r.Next(0, HbRelogManager.ProxyList.Count);
			}
			return HbRelogManager.ProxyList[num];
		}

		public static void hook_KeyPressed(object sender, KeyPressedEventArgs e)
		{
			if (e.Modifier == ModifierKeys.Control && e.Key == Keys.R)
			{
				HbRelogManager.StartAllActive();
				return;
			}
			if (e.Modifier == ModifierKeys.Control && e.Key == Keys.T)
			{
				HbRelogManager.StopAllActive();
				return;
			}
			if (e.Modifier == ModifierKeys.Control)
			{
				CharacterProfile item = HbRelogManager.CharactersById[(int)e.Key - (int)Keys.F1];
				if ((!item.IsRunning || item.IsPaused) && item.Settings.IsEnabled)
				{
					item.Start();
				}
			}
			if (e.Modifier == ModifierKeys.Shift)
			{
				HbRelogManager.CharactersById[(int)e.Key - (int)Keys.F1].Stop();
			}
		}

		public static void ReloadCommands()
		{
			HbRelogManager.Schedule = Scheduler.ReadCommands();
		}

		public static void ReloadProxyList()
		{
			if (!File.Exists("proxy.txt"))
			{
				HbRelogManager.ProxyList = new List<string>();
				return;
			}
			HbRelogManager.ProxyList = File.ReadAllLines("proxy.txt").ToList<string>();
		}

		public static void Shutdown()
		{
			int i;
			HbRelogManager._shuttingDown = true;
			try
			{
				Process[] processesByName = Process.GetProcessesByName("Wow");
				for (i = 0; i < (int)processesByName.Length; i++)
				{
					processesByName[i].Kill();
					Alex.WoWRelogger.Utility.Log.Write("Killing Wow process", new object[0]);
				}
				processesByName = Process.GetProcessesByName("Unlocker");
				for (i = 0; i < (int)processesByName.Length; i++)
				{
					processesByName[i].Kill();
					Alex.WoWRelogger.Utility.Log.Write("Killing Unlocker process", new object[0]);
				}
				Environment.Exit(0);
			}
			catch
			{
			}
		}

		public static void StartAllActive()
		{
			foreach (CharacterProfile charactersById in HbRelogManager.CharactersById)
			{
				if (charactersById.IsRunning && !charactersById.IsPaused || !charactersById.Settings.IsEnabled)
				{
					continue;
				}
				charactersById.Start();
			}
		}

		public static void StartProfile(CharacterProfile profile)
		{
			foreach (CharacterProfile charactersById in HbRelogManager.CharactersById)
			{
				if (charactersById != profile)
				{
					continue;
				}
				charactersById.Start();
			}
		}

		public static void StopAllActive()
		{
			foreach (CharacterProfile charactersById in HbRelogManager.CharactersById)
			{
				if (!charactersById.IsRunning && !charactersById.IsPaused)
				{
					continue;
				}
				charactersById.NeedToStop = true;
			}
		}

		public static void StopProfile(CharacterProfile profile)
		{
			foreach (CharacterProfile charactersById in HbRelogManager.CharactersById)
			{
				if (charactersById != profile || !charactersById.IsRunning && !charactersById.IsPaused)
				{
					continue;
				}
				charactersById.NeedToStop = true;
			}
		}

		public static void UpdateWebmoneyBalance()
		{
			MainWindow.Instance.Dispatcher.Invoke(() => MainWindow.Instance.HbrelogOptions.WmrBalance.Content = string.Concat(Helper.GetWebmoneyBalance(), "p").ToString(new CultureInfo("en-US")));
		}
	}
}