using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace Alex.WoWRelogger
{
	internal class Program
	{
		private static Dictionary<string, string> CmdLineArgs;

		public static Application app;

		static Program()
		{
			Program.CmdLineArgs = new Dictionary<string, string>();
		}

		public Program()
		{
		}

		private static void CurrentDomain_UnhandledException(object exceptionObject, object args)
		{
			if (exceptionObject != null)
			{
				Exception exception = exceptionObject as Exception;
				if (exception != null)
				{
					Alex.WoWRelogger.Utility.Log.Err(exception.Message, new object[0]);
					return;
				}
			}
			Alex.WoWRelogger.Utility.Log.Err("NULL EXCEPtion", new object[0]);
		}

		private static void CurrentDomainProcessExit(object sender, EventArgs e)
		{
			HbRelogManager.Shutdown();
		}

		private static T GetCmdLineArgVal<T>(string arg)
		{
			T t;
			try
			{
				t = (T)Convert.ChangeType(arg, typeof(T));
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Alex.WoWRelogger.Utility.Log.Err("Unable to convert {0} to type: {1}\n{2}", new object[] { arg, typeof(T), exception });
				t = default(T);
			}
			return t;
		}

		[STAThread]
		public static void Main(params string[] args)
		{
			bool flag = false;
			using (Mutex mutex = new Mutex(true, "WoWRelogger", out flag))
			{
				if (!flag)
				{
					Process currentProcess = Process.GetCurrentProcess();
					Process process = Process.GetProcessesByName(currentProcess.ProcessName).FirstOrDefault<Process>((Process p) => p.Id != currentProcess.Id);
					if (process != null)
					{
						NativeMethods.SetForegroundWindow(process.MainWindowHandle);
					}
				}
				else
				{
					AppDomain.CurrentDomain.ProcessExit += new EventHandler(Program.CurrentDomainProcessExit);
					AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.CurrentDomain_UnhandledException);
					Program.CmdLineArgs = Program.ProcessCmdLineArgs(args);
					if (Program.CmdLineArgs.ContainsKey("AUTOSTART"))
					{
						HbRelogManager.Settings.AllowTrials = true;
					}
					if (Program.CmdLineArgs.ContainsKey("WOWDELAY"))
					{
						HbRelogManager.Settings.WowDelay = Program.GetCmdLineArgVal<int>(Program.CmdLineArgs["WOWDELAY"]);
					}
					Program.app = new Application();
					Window mainWindow = new MainWindow();
					mainWindow.Show();
					Program.app.Run(mainWindow);
				}
			}
		}

		private static Dictionary<string, string> ProcessCmdLineArgs(IEnumerable<string> args)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>();
			foreach (string arg in args)
			{
				string[] strArrays = arg.Split(new char[] { '=' });
				string upperInvariant = ((strArrays[0][0] == '/' ? strArrays[0].Substring(1) : strArrays[0])).ToUpperInvariant();
				strs.Add(upperInvariant, ((int)strArrays.Length > 1 ? strArrays[1] : ""));
			}
			return strs;
		}
	}
}