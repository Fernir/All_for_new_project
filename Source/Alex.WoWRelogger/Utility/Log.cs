using Alex.WoWRelogger;
using Alex.WoWRelogger.Settings;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace Alex.WoWRelogger.Utility
{
	internal class Log
	{
		private readonly static string LogPath;

		public static string ApplicationPath
		{
			get
			{
				return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
			}
		}

		static Log()
		{
			string str = Path.Combine(Alex.WoWRelogger.Utility.Log.ApplicationPath, "Logs");
			if (!Directory.Exists(str))
			{
				Directory.CreateDirectory(str);
			}
			Alex.WoWRelogger.Utility.Log.LogPath = Path.Combine(str, string.Format("Log[{0:yyyy-MM-dd_hh-mm-ss}].txt", DateTime.Now));
		}

		public Log()
		{
		}

		public static void Debug(string format, params object[] args)
		{
			Alex.WoWRelogger.Utility.Log.Debug((HbRelogManager.Settings.UseDarkStyle ? Colors.White : Colors.Black), format, args);
		}

		public static void Debug(Color color, string format, params object[] args)
		{
			if (MainWindow.Instance == null)
			{
				return;
			}
			if (Thread.CurrentThread != MainWindow.Instance.Dispatcher.Thread)
			{
				MainWindow.Instance.Dispatcher.BeginInvoke(new Action(() => {
					Alex.WoWRelogger.Utility.Log.InternalWrite(color, string.Format(format, args));
					Alex.WoWRelogger.Utility.Log.WriteToLog(format, args);
				}), new object[0]);
				return;
			}
			Alex.WoWRelogger.Utility.Log.InternalWrite(color, string.Format(format, args));
			Alex.WoWRelogger.Utility.Log.WriteToLog(format, args);
		}

		public static void Err(string format, params object[] args)
		{
			Alex.WoWRelogger.Utility.Log.Write(Colors.Red, format, args);
		}

		private static void InternalWrite(Color color, string text)
		{
			try
			{
				RichTextBox logTextBox = MainWindow.Instance.LogTextBox;
				Color color1 = Color.FromArgb(color.A, color.R, color.G, color.B);
				(new TextRange(logTextBox.Document.ContentEnd, logTextBox.Document.ContentEnd)
				{
					Text = string.Format("[{0:T}] {1}\r", DateTime.Now, text)
				}).ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color1));
			}
			catch
			{
			}
		}

		private static void InternalWrite(Color headerColor, string header, Color msgColor, string format, params object[] args)
		{
			try
			{
				RichTextBox logTextBox = MainWindow.Instance.LogTextBox;
				Color color = Color.FromArgb(headerColor.A, headerColor.R, headerColor.G, headerColor.B);
				Color color1 = Color.FromArgb(msgColor.A, msgColor.R, msgColor.G, msgColor.B);
				(new TextRange(logTextBox.Document.ContentEnd, logTextBox.Document.ContentEnd)
				{
					Text = string.Format("[{0:T}] {1}", DateTime.Now, header)
				}).ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
				TextRange textRange = new TextRange(logTextBox.Document.ContentEnd, logTextBox.Document.ContentEnd);
				string str = string.Format(format, args);
				textRange.Text = string.Concat(str, "\r");
				textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color1));
			}
			catch
			{
			}
		}

		public static void Write(string format, params object[] args)
		{
			Alex.WoWRelogger.Utility.Log.Write((HbRelogManager.Settings.UseDarkStyle ? Colors.White : Colors.Black), format, args);
		}

		public static void Write(Color color, string format, params object[] args)
		{
			if (MainWindow.Instance == null)
			{
				return;
			}
			if (Thread.CurrentThread != MainWindow.Instance.Dispatcher.Thread)
			{
				MainWindow.Instance.Dispatcher.Invoke(() => {
					Alex.WoWRelogger.Utility.Log.InternalWrite(color, string.Format(format, args));
					Alex.WoWRelogger.Utility.Log.WriteToLog(format, args);
				});
				return;
			}
			Alex.WoWRelogger.Utility.Log.InternalWrite(color, string.Format(format, args));
			Alex.WoWRelogger.Utility.Log.WriteToLog(format, args);
		}

		public static void Write(Color hColor, string header, Color mColor, string format, params object[] args)
		{
			if (MainWindow.Instance == null)
			{
				return;
			}
			if (Thread.CurrentThread != MainWindow.Instance.Dispatcher.Thread)
			{
				try
				{
					MainWindow.Instance.Dispatcher.Invoke(() => {
						Alex.WoWRelogger.Utility.Log.InternalWrite(hColor, header, mColor, string.Format(format, args), new object[0]);
						Alex.WoWRelogger.Utility.Log.WriteToLog(string.Concat(header, format), args);
					});
				}
				catch
				{
				}
				return;
			}
			Alex.WoWRelogger.Utility.Log.InternalWrite(hColor, header, mColor, string.Format(format, args), new object[0]);
			Alex.WoWRelogger.Utility.Log.WriteToLog(string.Concat(header, format), args);
		}

		public static void WriteToLog(string format, params object[] args)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(Alex.WoWRelogger.Utility.Log.LogPath, true))
				{
					DateTime now = DateTime.Now;
					streamWriter.WriteLine(string.Format(string.Concat("[", now.ToString(CultureInfo.InvariantCulture), "] ", format), args));
				}
			}
			catch
			{
			}
		}
	}
}