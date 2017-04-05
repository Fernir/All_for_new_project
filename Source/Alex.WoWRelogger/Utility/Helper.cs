using Alex.WoWRelogger;
using Alex.WoWRelogger.Settings;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Alex.WoWRelogger.Utility
{
	internal static class Helper
	{
		public const uint WmKeydown = 256;

		public const uint WmChar = 258;

		public const uint WmKeyup = 257;

		public readonly static Random Rand;

		public readonly static string AssemblyDirectory;

		private static ulong _lastNumber;

		private const string WebmoneyX8Request = "<w3s.request>\r\n                                        <reqn>{0}</reqn>\r\n                                        <wmid></wmid>\r\n                                        <sign></sign>\r\n                                        <getpurses>\r\n                                            <wmid>{1}</wmid>\r\n                                        </getpurses>\r\n                                     </w3s.request>";

		private const int SizeOfInput = 28;

		private static object _inputLock;

		private static IntPtr _originalForegroundWindow;

		private static Point _originalMousePos;

		public static bool HasInternetConnection
		{
			get
			{
				int num;
				return Alex.WoWRelogger.Utility.NativeMethods.InternetGetConnectedState(out num, 0);
			}
		}

		static Helper()
		{
			Helper.Rand = new Random();
			Helper.AssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			Helper._lastNumber = (ulong)0;
			Helper._inputLock = new object();
		}

		public static uint BaseOffset(this Process proc)
		{
			return (uint)proc.MainModule.BaseAddress.ToInt32();
		}

		public static string DecrptDpapi(string base64Data)
		{
			byte[] numArray = Convert.FromBase64String(base64Data);
			numArray = ProtectedData.Unprotect(numArray, null, DataProtectionScope.CurrentUser);
			return Encoding.Unicode.GetString(numArray);
		}

		public static string DecryptAes(string clearText, byte[] key, byte[] iv)
		{
			string str;
			byte[] numArray = Convert.FromBase64String(clearText);
			using (Aes ae = Aes.Create())
			{
				using (ICryptoTransform cryptoTransform = ae.CreateDecryptor(key, iv))
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
						{
							cryptoStream.Write(numArray, 0, (int)numArray.Length);
							cryptoStream.FlushFinalBlock();
						}
						byte[] array = memoryStream.ToArray();
						str = Encoding.Unicode.GetString(array);
					}
				}
			}
			return str;
		}

		public static string EncodeToUTF8(this string text)
		{
			StringBuilder stringBuilder = new StringBuilder(Encoding.UTF8.GetByteCount(text) * 2);
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			for (int i = 0; i < (int)bytes.Length; i++)
			{
				byte num = bytes[i];
				stringBuilder.Append(string.Format("\\{0:D3}", num));
			}
			return stringBuilder.ToString();
		}

		public static string EncrptDpapi(string clearData)
		{
			return Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(clearData), null, DataProtectionScope.CurrentUser));
		}

		public static string EncryptAes(string clearText, byte[] key, byte[] iv)
		{
			string base64String;
			byte[] bytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes ae = Aes.Create())
			{
				using (ICryptoTransform cryptoTransform = ae.CreateEncryptor(key, iv))
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
						{
							cryptoStream.Write(bytes, 0, (int)bytes.Length);
							cryptoStream.FlushFinalBlock();
						}
						base64String = Convert.ToBase64String(memoryStream.ToArray());
					}
				}
			}
			return base64String;
		}

		public static Process GetChildProcessByName(int parentPid, string processName)
		{
			return Process.GetProcessesByName(processName).FirstOrDefault<Process>((Process process) => Helper.IsChildProcessOf(parentPid, process));
		}

		private static ulong GetRequestNumber()
		{
			DateTime utcNow = DateTime.UtcNow;
			ulong num = ulong.Parse(utcNow.ToString("yyMMddHHmmssfff", CultureInfo.InvariantCulture.DateTimeFormat), NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat);
			if (num <= Helper._lastNumber)
			{
				num = Helper._lastNumber + (long)1;
			}
			Helper._lastNumber = num;
			return num;
		}

		public static double GetWebmoneyBalance()
		{
            //todo включить проверку сертификата
//			MyWebClient myWebClient = new MyWebClient(null);
//			X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
//			x509Store.Open(OpenFlags.ReadOnly);
//			X509Certificate2 x509Certificate2 = x509Store.Certificates.OfType<X509Certificate2>().FirstOrDefault<X509Certificate2>((X509Certificate2 a) => a.Subject.Contains("WM id:"));
//			string value = (new Regex("WM id: (\\d{12})")).Match(x509Certificate2.Subject).Groups[1].Value;
//			string str = myWebClient.UploadString("https://w3s.wmtransfer.com/asp/XMLPursesCert.asp", string.Format("<w3s.request>\r\n                                        <reqn>{0}</reqn>\r\n                                        <wmid></wmid>\r\n                                        <sign></sign>\r\n                                        <getpurses>\r\n                                            <wmid>{1}</wmid>\r\n                                        </getpurses>\r\n                                     </w3s.request>", Helper.GetRequestNumber(), value));
//			Alex.WoWRelogger.Utility.Log.Write(str, new object[0]);
//			return (
//				from x in XDocument.Parse(str).Root.Descendants("purse")
//				where (string)x.Element("pursename") == HbRelogManager.Settings.WmrPurse
//				select (double)((double)x.Element("amount"))).FirstOrDefault<double>();
		    return 50000;
		}

		public static Alex.WoWRelogger.Utility.NativeMethods.WindowInfo GetWindowInfo(IntPtr hWnd)
		{
			Alex.WoWRelogger.Utility.NativeMethods.WindowInfo windowInfo = new Alex.WoWRelogger.Utility.NativeMethods.WindowInfo(new bool?(true));
			Alex.WoWRelogger.Utility.NativeMethods.GetWindowInfo(hWnd, ref windowInfo);
			return windowInfo;
		}

		public static Alex.WoWRelogger.Utility.NativeMethods.Rect GetWindowRect(IntPtr hWnd)
		{
			Alex.WoWRelogger.Utility.NativeMethods.Rect rect = new Alex.WoWRelogger.Utility.NativeMethods.Rect();
			Alex.WoWRelogger.Utility.NativeMethods.GetWindowRect(hWnd, out rect);
			return rect;
		}

		public static bool Is64BitProcess(Process proc)
		{
			bool flag;
			if (!Environment.Is64BitOperatingSystem)
			{
				return false;
			}
			return !(Alex.WoWRelogger.Utility.NativeMethods.IsWow64Process(proc.Handle, out flag) & flag);
		}

		public static bool IsChildProcessOf(int parentPid, Process child)
		{
			bool flag;
			int id = child.Id;
			try
			{
				while (true)
				{
					int parentProcessId = Alex.WoWRelogger.Utility.NativeMethods.ParentProcessUtilities.GetParentProcessId(id);
					if (parentProcessId <= 0)
					{
						flag = false;
						return flag;
					}
					if (parentProcessId == parentPid)
					{
						break;
					}
					id = parentProcessId;
				}
				flag = true;
			}
			catch (Exception exception)
			{
				flag = false;
			}
			return flag;
		}

		public static bool IsEnglish(string inputstring)
		{
			if ((new Regex("[A-Za-z0-9 .,-=+(){}\\[\\]\\\\]")).Matches(inputstring).Count.Equals(inputstring.Length))
			{
				return true;
			}
			return false;
		}

		public static void LeftClickAtPos(IntPtr hWnd, int x, int y, bool doubleClick = false, bool restore = true, Func<bool> restoreCondition = null)
		{
			Alex.WoWRelogger.Utility.NativeMethods.Rect windowRect = Helper.GetWindowRect(hWnd);
			double systemMetrics = (double)(Alex.WoWRelogger.Utility.NativeMethods.GetSystemMetrics(Alex.WoWRelogger.Utility.NativeMethods.SystemMetric.SM_CXSCREEN) - 1);
			double num = (double)(Alex.WoWRelogger.Utility.NativeMethods.GetSystemMetrics(Alex.WoWRelogger.Utility.NativeMethods.SystemMetric.SM_CYSCREEN) - 1);
			double left = (double)(windowRect.Left + x) * (65535 / systemMetrics);
			double top = (double)(windowRect.Top + y) * (65535 / num);
			Alex.WoWRelogger.Utility.NativeMethods.Input input = new Alex.WoWRelogger.Utility.NativeMethods.Input()
			{
				type = Alex.WoWRelogger.Utility.NativeMethods.SendInputEventType.InputMouse
			};
			input.mkhi.mi.dwFlags = Alex.WoWRelogger.Utility.NativeMethods.MouseEventFlags.LEFTDOWN | Alex.WoWRelogger.Utility.NativeMethods.MouseEventFlags.LEFTUP | Alex.WoWRelogger.Utility.NativeMethods.MouseEventFlags.MOVE | Alex.WoWRelogger.Utility.NativeMethods.MouseEventFlags.ABSOLUTE;
			input.mkhi.mi.dx = (int)left;
			input.mkhi.mi.dy = (int)top;
			IntPtr foregroundWindow = Alex.WoWRelogger.Utility.NativeMethods.GetForegroundWindow();
			if (restore)
			{
				Helper.SaveForegroundWindowAndMouse();
			}
			try
			{
				for (int i = 0; foregroundWindow != hWnd && i < 1000; i++)
				{
					Alex.WoWRelogger.Utility.NativeMethods.SetForegroundWindow(hWnd);
					Thread.Sleep(1);
					foregroundWindow = Alex.WoWRelogger.Utility.NativeMethods.GetForegroundWindow();
				}
				Alex.WoWRelogger.Utility.NativeMethods.SendInput(1, ref input, 28);
				if (doubleClick)
				{
					Thread.Sleep(100);
					Alex.WoWRelogger.Utility.NativeMethods.SendInput(1, ref input, 28);
				}
			}
			finally
			{
				if (restore)
				{
					try
					{
						if (restoreCondition == null)
						{
							Thread.Sleep(100);
						}
						else
						{
							while (!restoreCondition())
							{
								Thread.Sleep(1);
							}
						}
						Helper.RestoreForegroundWindowAndMouse();
					}
					catch
					{
					}
				}
			}
		}

		public static int MakeLParam(int LoWord, int HiWord)
		{
			return HiWord << 16 | LoWord & 65535;
		}

		public static void PostBackgroundKey(IntPtr hWnd, char key, bool useVmChar = true)
		{
			uint num = Alex.WoWRelogger.Utility.NativeMethods.MapVirtualKey(key, 0);
			UIntPtr uIntPtr = (UIntPtr)(1 | num << 16);
			if (useVmChar)
			{
				Helper.PostMessage(hWnd, Alex.WoWRelogger.Utility.NativeMethods.Message.VM_CHAR, key, uIntPtr);
				return;
			}
			Helper.PostMessage(hWnd, Alex.WoWRelogger.Utility.NativeMethods.Message.KEY_DOWN, key, uIntPtr);
			Helper.PostMessage(hWnd, Alex.WoWRelogger.Utility.NativeMethods.Message.VM_CHAR, key, uIntPtr);
			Helper.PostMessage(hWnd, Alex.WoWRelogger.Utility.NativeMethods.Message.KEY_UP, key, uIntPtr);
		}

		public static void PostBackgroundString(IntPtr hWnd, string str, bool downUp = true)
		{
			string str1 = str;
			for (int i = 0; i < str1.Length; i++)
			{
				Helper.PostBackgroundKey(hWnd, str1[i], true);
			}
		}

		private static void PostMessage(IntPtr hWnd, Alex.WoWRelogger.Utility.NativeMethods.Message msg, char key, UIntPtr lParam)
		{
			Alex.WoWRelogger.Utility.NativeMethods.PostMessage(hWnd, (uint)msg, (IntPtr)key, lParam);
		}

		public static void ResizeAndMoveWindow(IntPtr hWnd, int x, int y, int width, int height)
		{
			Alex.WoWRelogger.Utility.NativeMethods.SetWindowPos(hWnd, new IntPtr(0), x, y, width, height, Alex.WoWRelogger.Utility.NativeMethods.SetWindowPosFlags.SWP_NOACTIVATE | Alex.WoWRelogger.Utility.NativeMethods.SetWindowPosFlags.SWP_NOZORDER);
		}

		public static void RestoreForegroundWindowAndMouse()
		{
			IntPtr foregroundWindow = Alex.WoWRelogger.Utility.NativeMethods.GetForegroundWindow();
			for (int i = 0; foregroundWindow != Helper._originalForegroundWindow && i < 1000 && Alex.WoWRelogger.Utility.NativeMethods.SetForegroundWindow(Helper._originalForegroundWindow); i++)
			{
				Thread.Sleep(1);
				foregroundWindow = Alex.WoWRelogger.Utility.NativeMethods.GetForegroundWindow();
			}
			Alex.WoWRelogger.Utility.NativeMethods.Input x = new Alex.WoWRelogger.Utility.NativeMethods.Input()
			{
				type = Alex.WoWRelogger.Utility.NativeMethods.SendInputEventType.InputMouse
			};
			double systemMetrics = (double)(Alex.WoWRelogger.Utility.NativeMethods.GetSystemMetrics(Alex.WoWRelogger.Utility.NativeMethods.SystemMetric.SM_CXSCREEN) - 1);
			double num = (double)(Alex.WoWRelogger.Utility.NativeMethods.GetSystemMetrics(Alex.WoWRelogger.Utility.NativeMethods.SystemMetric.SM_CYSCREEN) - 1);
			x.mkhi.mi.dwFlags = Alex.WoWRelogger.Utility.NativeMethods.MouseEventFlags.MOVE | Alex.WoWRelogger.Utility.NativeMethods.MouseEventFlags.ABSOLUTE;
			x.mkhi.mi.dx = (int)((double)Helper._originalMousePos.X * (65535 / systemMetrics));
			x.mkhi.mi.dy = (int)((double)Helper._originalMousePos.Y * (65535 / num));
			Alex.WoWRelogger.Utility.NativeMethods.SendInput(1, ref x, 28);
		}

		public static void SaveForegroundWindowAndMouse()
		{
			Helper._originalForegroundWindow = Alex.WoWRelogger.Utility.NativeMethods.GetForegroundWindow();
			Alex.WoWRelogger.Utility.NativeMethods.GetCursorPos(out Helper._originalMousePos);
		}

		public static bool SendBackgroundKey(IntPtr hWnd, char key, bool useVmChar = true)
		{
			bool flag = false;
			uint num = Alex.WoWRelogger.Utility.NativeMethods.MapVirtualKey(key, 0);
			UIntPtr uIntPtr = (UIntPtr)(1 | num << 16);
			if (!useVmChar)
			{
				flag = (!Helper.SendMessage(hWnd, Alex.WoWRelogger.Utility.NativeMethods.Message.KEY_DOWN, key, uIntPtr) ? false : Helper.SendMessage(hWnd, Alex.WoWRelogger.Utility.NativeMethods.Message.KEY_UP, key, uIntPtr));
			}
			else
			{
				flag = Helper.SendMessage(hWnd, Alex.WoWRelogger.Utility.NativeMethods.Message.VM_CHAR, key, uIntPtr);
			}
			return flag;
		}

		public static void SendBackgroundString(IntPtr hWnd, string str, bool downUp = true)
		{
			string str1 = str;
			for (int i = 0; i < str1.Length; i++)
			{
				char chr = str1[i];
				Helper.SendBackgroundKey(hWnd, chr, downUp);
			}
		}

		public static void SendEmail(string title, string text)
		{
			SmtpClient smtpClient = new SmtpClient()
			{
				Port = 587,
				Host = "smtp.mail.ru",
				EnableSsl = true,
				Timeout = 10000,
				Credentials = new NetworkCredential("mail-account5@mail.ru", "absolute123")
			};
			MailMessage mailMessage = new MailMessage("mail-account5@mail.ru", "god_915@mail.ru", title, text)
			{
				BodyEncoding = Encoding.UTF8,
				DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
			};
			Task.Run(() => smtpClient.SendAsync(mailMessage, null));
		}

		private static bool SendMessage(IntPtr hWnd, Alex.WoWRelogger.Utility.NativeMethods.Message msg, char key, UIntPtr lParam)
		{
			for (int i = 0; i < 4; i++)
			{
				if (Alex.WoWRelogger.Utility.NativeMethods.SendMessage(hWnd, (uint)msg, (IntPtr)key, lParam) == IntPtr.Zero)
				{
					return true;
				}
			}
			return false;
		}

		public static void SetWindowText(IntPtr hWnd, string text)
		{
			Alex.WoWRelogger.Utility.NativeMethods.SetWindowText(hWnd, text);
		}

		public static void ShowWindow(IntPtr hWnd, Alex.WoWRelogger.Utility.NativeMethods.ShowWindowCommands cmd)
		{
			Alex.WoWRelogger.Utility.NativeMethods.ShowWindow(hWnd, cmd);
		}

		public static bool SleepUntil(Func<bool> condition, TimeSpan maxSleepTime)
		{
			DateTime now = DateTime.Now;
			bool flag = false;
			while (!condition())
			{
				bool now1 = (DateTime.Now - now) >= maxSleepTime;
				flag = now1;
				if (now1)
				{
					break;
				}
				Thread.Sleep(10);
			}
			return !flag;
		}

		public static void UnblockFileIfZoneRestricted(string file)
		{
			if (!File.Exists(file))
			{
				throw new FileNotFoundException(file);
			}
			string str = string.Concat(file, ":Zone.Identifier");
			if (Alex.WoWRelogger.Utility.NativeMethods.GetFileAttributes(str) != -1)
			{
				Alex.WoWRelogger.Utility.Log.Write("Removing Zone restrictions from {0}", new object[] { file });
				Alex.WoWRelogger.Utility.NativeMethods.DeleteFile(str);
			}
		}

		public static string VersionString(this Process proc)
		{
			return proc.MainModule.FileVersionInfo.FileVersion;
		}
	}
}