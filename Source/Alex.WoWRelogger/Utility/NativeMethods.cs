using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alex.WoWRelogger.Utility
{
	internal class NativeMethods
	{
		public const int MaxPath = 260;

		public const int MaxAlternate = 14;

		public NativeMethods()
		{
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool BlockInput(bool fBlockIt);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, Alex.WoWRelogger.Utility.NativeMethods.ProcessCreationFlags dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref Alex.WoWRelogger.Utility.NativeMethods.STARTUPINFO lpStartupInfo, out Alex.WoWRelogger.Utility.NativeMethods.PROCESS_INFORMATION lpProcessInformation);

		[DllImport("kernel32", CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
		public static extern bool DeleteFile(string name);

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool EnumChildWindows(IntPtr window, Alex.WoWRelogger.Utility.NativeMethods.EnumWindowProc callback, IntPtr i);

		public static List<IntPtr> EnumerateProcessWindowHandles(int processId)
		{
			Alex.WoWRelogger.Utility.NativeMethods.EnumWindowProc enumWindowProc = null;
			List<IntPtr> intPtrs = new List<IntPtr>();
			foreach (object thread in Process.GetProcessById(processId).Threads)
			{
				int id = ((ProcessThread)thread).Id;
				Alex.WoWRelogger.Utility.NativeMethods.EnumWindowProc enumWindowProc1 = enumWindowProc;
				if (enumWindowProc1 == null)
				{
					Alex.WoWRelogger.Utility.NativeMethods.EnumWindowProc enumWindowProc2 = (IntPtr hWnd, IntPtr lParam) => {
						intPtrs.Add(hWnd);
						return true;
					};
					Alex.WoWRelogger.Utility.NativeMethods.EnumWindowProc enumWindowProc3 = enumWindowProc2;
					enumWindowProc = enumWindowProc2;
					enumWindowProc1 = enumWindowProc3;
				}
				Alex.WoWRelogger.Utility.NativeMethods.EnumThreadWindows(id, enumWindowProc1, IntPtr.Zero);
			}
			return intPtrs;
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool EnumThreadWindows(int dwThreadId, Alex.WoWRelogger.Utility.NativeMethods.EnumWindowProc lpfn, IntPtr lParam);

		private static bool EnumWindow(IntPtr handle, IntPtr pointer)
		{
			List<IntPtr> target = GCHandle.FromIntPtr(pointer).Target as List<IntPtr>;
			if (target == null)
			{
				throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
			}
			target.Add(handle);
			return true;
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern bool FindClose(IntPtr hFindFile);

		[DllImport("kernel32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		public static extern IntPtr FindFirstFile(string lpFileName, out Alex.WoWRelogger.Utility.NativeMethods.Win32FindData lpFindFileData);

		[DllImport("kernel32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		public static extern bool FindNextFile(IntPtr hFindFile, out Alex.WoWRelogger.Utility.NativeMethods.Win32FindData lpFindFileData);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern bool FreeLibrary(IntPtr hModule);

		public static List<IntPtr> GetChildWindows(IntPtr parent)
		{
			List<IntPtr> intPtrs = new List<IntPtr>();
			GCHandle gCHandle = GCHandle.Alloc(intPtrs);
			try
			{
				Alex.WoWRelogger.Utility.NativeMethods.EnumChildWindows(parent, new Alex.WoWRelogger.Utility.NativeMethods.EnumWindowProc(Alex.WoWRelogger.Utility.NativeMethods.EnumWindow), GCHandle.ToIntPtr(gCHandle));
			}
			finally
			{
				if (gCHandle.IsAllocated)
				{
					gCHandle.Free();
				}
			}
			return intPtrs;
		}

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		public static string GetClassName(IntPtr hWnd)
		{
			StringBuilder stringBuilder = new StringBuilder(100);
			Alex.WoWRelogger.Utility.NativeMethods.GetClassName(hWnd, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool GetCursorPos(out Point lpPoint);

		[DllImport("kernel32.dll", CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
		public static extern int GetFileAttributes(string lpFileName);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("kernel32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int GetSystemMetrics(Alex.WoWRelogger.Utility.NativeMethods.SystemMetric smIndex);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern IntPtr GetWindow(IntPtr hWnd, Alex.WoWRelogger.Utility.NativeMethods.GetWindow_Cmd uCmd);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern bool GetWindowInfo(IntPtr hwnd, ref Alex.WoWRelogger.Utility.NativeMethods.WindowInfo pwi);

		public static uint GetWindowProcessId(IntPtr hWnd)
		{
			uint num;
			Alex.WoWRelogger.Utility.NativeMethods.GetWindowThreadProcessId(hWnd, out num);
			return num;
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool GetWindowRect(IntPtr hWnd, out Alex.WoWRelogger.Utility.NativeMethods.Rect lpRect);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		public static string GetWindowText(IntPtr hWnd)
		{
			StringBuilder stringBuilder = new StringBuilder(Alex.WoWRelogger.Utility.NativeMethods.GetWindowTextLength(hWnd) + 1);
			Alex.WoWRelogger.Utility.NativeMethods.GetWindowText(hWnd, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		private static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("wininet.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool IsWow64Process([In] IntPtr process, out bool wow64Process);

		[DllImport("kernel32", CharSet=CharSet.None, ExactSpelling=false)]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr LoadLibrary(string libraryName);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern uint MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern IntPtr OpenThread(Alex.WoWRelogger.Utility.NativeMethods.ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		public static extern IntPtr PostMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, UIntPtr lParam);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

		public static void ResumeProcess(Process process)
		{
			foreach (ProcessThread thread in process.Threads)
			{
				IntPtr intPtr = Alex.WoWRelogger.Utility.NativeMethods.OpenThread(Alex.WoWRelogger.Utility.NativeMethods.ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
				if (intPtr != IntPtr.Zero)
				{
					Alex.WoWRelogger.Utility.NativeMethods.ResumeThread(intPtr);
				}
				else
				{
					return;
				}
			}
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int ResumeThread(IntPtr hThread);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern uint SendInput(uint nInputs, ref Alex.WoWRelogger.Utility.NativeMethods.Input pInputs, int cbSize);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, UIntPtr lParam);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, Alex.WoWRelogger.Utility.NativeMethods.SetWindowPosFlags uFlags);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int SetWindowText(IntPtr hWnd, string text);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool ShowWindow(IntPtr hWnd, Alex.WoWRelogger.Utility.NativeMethods.ShowWindowCommands nCmdShow);

		public static Process StartSuspended(string path, string args)
		{
			string str = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last<string>();
			Alex.WoWRelogger.Utility.NativeMethods.STARTUPINFO sTARTUPINFO = new Alex.WoWRelogger.Utility.NativeMethods.STARTUPINFO();
			Alex.WoWRelogger.Utility.NativeMethods.PROCESS_INFORMATION pROCESSINFORMATION = new Alex.WoWRelogger.Utility.NativeMethods.PROCESS_INFORMATION();
			Alex.WoWRelogger.Utility.NativeMethods.CreateProcess(path, string.Concat(str, " ", args), IntPtr.Zero, IntPtr.Zero, false, Alex.WoWRelogger.Utility.NativeMethods.ProcessCreationFlags.CREATE_SUSPENDED, IntPtr.Zero, null, ref sTARTUPINFO, out pROCESSINFORMATION);
			return Process.GetProcessById((int)pROCESSINFORMATION.dwProcessId);
		}

		public static void SuspendProcess(Process process)
		{
			foreach (ProcessThread thread in process.Threads)
			{
				IntPtr intPtr = Alex.WoWRelogger.Utility.NativeMethods.OpenThread(Alex.WoWRelogger.Utility.NativeMethods.ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
				if (intPtr != IntPtr.Zero)
				{
					Alex.WoWRelogger.Utility.NativeMethods.SuspendThread(intPtr);
				}
				else
				{
					return;
				}
			}
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern uint SuspendThread(IntPtr hThread);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern bool VirtualProtect(uint lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

		[Flags]
		public enum ConnectionStates
		{
			Modem = 1,
			Lan = 2,
			Proxy = 4,
			RasInstalled = 16,
			Offline = 32,
			Configured = 64
		}

		public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

		public struct Filetime
		{
			public uint dwLowDateTime;

			public uint dwHighDateTime;
		}

		public enum GetWindow_Cmd : uint
		{
			GW_HWNDFIRST,
			GW_HWNDLAST,
			GW_HWNDNEXT,
			GW_HWNDPREV,
			GW_OWNER,
			GW_CHILD,
			GW_ENABLEDPOPUP
		}

		public struct Hardwareinput
		{
			public int uMsg;

			public short wParamL;

			public short wParamH;
		}

		public struct Input
		{
			public Alex.WoWRelogger.Utility.NativeMethods.SendInputEventType type;

			public Alex.WoWRelogger.Utility.NativeMethods.MouseKeybdhardwareInputUnion mkhi;
		}

		public struct Keybdinput
		{
			public ushort wVk;

			public ushort wScan;

			public uint dwFlags;

			public uint time;

			public IntPtr dwExtraInfo;
		}

		public enum Message : uint
		{
			NCHITTEST = 132,
			KEY_DOWN = 256,
			KEY_UP = 257,
			VM_CHAR = 258,
			SYSKEYDOWN = 260,
			SYSKEYUP = 261,
			SYSCHAR = 262,
			LBUTTONDOWN = 513,
			LBUTTONUP = 514,
			LBUTTONDBLCLK = 515,
			RBUTTONDOWN = 516,
			RBUTTONUP = 517,
			RBUTTONDBLCLK = 518,
			MBUTTONDOWN = 519,
			MBUTTONUP = 520
		}

		[Flags]
		public enum MouseEventFlags
		{
			MOVE = 1,
			LEFTDOWN = 2,
			LEFTUP = 4,
			RIGHTDOWN = 8,
			RIGHTUP = 16,
			MIDDLEDOWN = 32,
			MIDDLEUP = 64,
			ABSOLUTE = 32768
		}

		public struct MouseInputData
		{
			public int dx;

			public int dy;

			public uint mouseData;

			public Alex.WoWRelogger.Utility.NativeMethods.MouseEventFlags dwFlags;

			public uint time;

			public IntPtr dwExtraInfo;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct MouseKeybdhardwareInputUnion
		{
			[FieldOffset(0)]
			public Alex.WoWRelogger.Utility.NativeMethods.MouseInputData mi;

			[FieldOffset(0)]
			public Alex.WoWRelogger.Utility.NativeMethods.Keybdinput ki;

			[FieldOffset(0)]
			public Alex.WoWRelogger.Utility.NativeMethods.Hardwareinput hi;
		}

		public struct ParentProcessUtilities
		{
			internal IntPtr Reserved1;

			internal IntPtr PebBaseAddress;

			internal IntPtr Reserved2_0;

			internal IntPtr Reserved2_1;

			internal IntPtr UniqueProcessId;

			internal IntPtr InheritedFromUniqueProcessId;

			public static Process GetParentProcess()
			{
				return Alex.WoWRelogger.Utility.NativeMethods.ParentProcessUtilities.GetParentProcess(Process.GetCurrentProcess().Handle);
			}

			public static Process GetParentProcess(int id)
			{
				return Alex.WoWRelogger.Utility.NativeMethods.ParentProcessUtilities.GetParentProcess(Process.GetProcessById(id).Handle);
			}

			public static Process GetParentProcess(IntPtr handle)
			{
				int parentProcessId = Alex.WoWRelogger.Utility.NativeMethods.ParentProcessUtilities.GetParentProcessId(handle);
				if (parentProcessId == -1)
				{
					return null;
				}
				return Process.GetProcessById(parentProcessId);
			}

			public static int GetParentProcessId(int id)
			{
				return Alex.WoWRelogger.Utility.NativeMethods.ParentProcessUtilities.GetParentProcessId(Process.GetProcessById(id).Handle);
			}

			public static int GetParentProcessId(IntPtr handle)
			{
				int num;
				int num1;
				Alex.WoWRelogger.Utility.NativeMethods.ParentProcessUtilities parentProcessUtility = new Alex.WoWRelogger.Utility.NativeMethods.ParentProcessUtilities();
				if (Alex.WoWRelogger.Utility.NativeMethods.ParentProcessUtilities.NtQueryInformationProcess(handle, 0, ref parentProcessUtility, Marshal.SizeOf<Alex.WoWRelogger.Utility.NativeMethods.ParentProcessUtilities>(parentProcessUtility), out num) != 0)
				{
					return -1;
				}
				try
				{
					num1 = parentProcessUtility.InheritedFromUniqueProcessId.ToInt32();
				}
				catch (ArgumentException argumentException)
				{
					num1 = -1;
				}
				return num1;
			}

			[DllImport("ntdll.dll", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref Alex.WoWRelogger.Utility.NativeMethods.ParentProcessUtilities processInformation, int processInformationLength, out int returnLength);
		}

		public struct PROCESS_INFORMATION
		{
			public IntPtr hProcess;

			public IntPtr hThread;

			public uint dwProcessId;

			public uint dwThreadId;
		}

		[Flags]
		public enum ProcessCreationFlags : uint
		{
			ZERO_FLAG = 0,
			DEBUG_PROCESS = 1,
			DEBUG_ONLY_THIS_PROCESS = 2,
			CREATE_SUSPENDED = 4,
			DETACHED_PROCESS = 8,
			CREATE_NEW_CONSOLE = 16,
			CREATE_NEW_PROCESS_GROUP = 512,
			CREATE_UNICODE_ENVIRONMENT = 1024,
			CREATE_SEPARATE_WOW_VDM = 4096,
			CREATE_SHARED_WOW_VDM = 4096,
			INHERIT_PARENT_AFFINITY = 65536,
			CREATE_PROTECTED_PROCESS = 262144,
			EXTENDED_STARTUPINFO_PRESENT = 524288,
			CREATE_BREAKAWAY_FROM_JOB = 16777216,
			CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 33554432,
			CREATE_DEFAULT_ERROR_MODE = 67108864,
			CREATE_NO_WINDOW = 134217728
		}

		public enum Protection
		{
			PageNoaccess = 1,
			PageReadonly = 2,
			PageReadwrite = 4,
			PageWritecopy = 8,
			PageExecute = 16,
			PageExecuteRead = 32,
			PageExecuteReadwrite = 64,
			PageExecuteWritecopy = 128,
			PageGuard = 256,
			PageNocache = 512,
			PageWritecombine = 1024
		}

		public struct Rect
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		public enum SendInputEventType
		{
			InputMouse,
			InputKeyboard,
			InputHardware
		}

		[Flags]
		public enum SetWindowPosFlags : uint
		{
			SWP_NOSIZE = 1,
			SWP_NOMOVE = 2,
			SWP_NOZORDER = 4,
			SWP_NOREDRAW = 8,
			SWP_NOACTIVATE = 16,
			SWP_DRAWFRAME = 32,
			SWP_FRAMECHANGED = 32,
			SWP_SHOWWINDOW = 64,
			SWP_HIDEWINDOW = 128,
			SWP_NOCOPYBITS = 256,
			SWP_NOOWNERZORDER = 512,
			SWP_NOREPOSITION = 512,
			SWP_NOSENDCHANGING = 1024,
			SWP_DEFERERASE = 8192,
			SWP_ASYNCWINDOWPOS = 16384
		}

		public enum ShowWindowCommands
		{
			Hide = 0,
			Normal = 1,
			ShowMinimized = 2,
			Maximize = 3,
			ShowMaximized = 3,
			ShowNoActivate = 4,
			Show = 5,
			Minimize = 6,
			ShowMinNoActive = 7,
			ShowNA = 8,
			Restore = 9,
			ShowDefault = 10,
			ForceMinimize = 11
		}

		public struct STARTUPINFO
		{
			public uint cb;

			public string lpReserved;

			public string lpDesktop;

			public string lpTitle;

			public uint dwX;

			public uint dwY;

			public uint dwXSize;

			public uint dwYSize;

			public uint dwXCountChars;

			public uint dwYCountChars;

			public uint dwFillAttribute;

			public uint dwFlags;

			public short wShowWindow;

			public short cbReserved2;

			public IntPtr lpReserved2;

			public IntPtr hStdInput;

			public IntPtr hStdOutput;

			public IntPtr hStdError;
		}

		public enum SystemMetric
		{
			SM_CXSCREEN = 0,
			SM_CYSCREEN = 1,
			SM_CXVSCROLL = 2,
			SM_CYHSCROLL = 3,
			SM_CYCAPTION = 4,
			SM_CXBORDER = 5,
			SM_CYBORDER = 6,
			SM_CXDLGFRAME = 7,
			SM_CXFIXEDFRAME = 7,
			SM_CYDLGFRAME = 8,
			SM_CYFIXEDFRAME = 8,
			SM_CYVTHUMB = 9,
			SM_CXHTHUMB = 10,
			SM_CXICON = 11,
			SM_CYICON = 12,
			SM_CXCURSOR = 13,
			SM_CYCURSOR = 14,
			SM_CYMENU = 15,
			SM_CXFULLSCREEN = 16,
			SM_CYFULLSCREEN = 17,
			SM_CYKANJIWINDOW = 18,
			SM_MOUSEPRESENT = 19,
			SM_CYVSCROLL = 20,
			SM_CXHSCROLL = 21,
			SM_DEBUG = 22,
			SM_SWAPBUTTON = 23,
			SM_CXMIN = 28,
			SM_CYMIN = 29,
			SM_CXSIZE = 30,
			SM_CYSIZE = 31,
			SM_CXFRAME = 32,
			SM_CXSIZEFRAME = 32,
			SM_CYFRAME = 33,
			SM_CYSIZEFRAME = 33,
			SM_CXMINTRACK = 34,
			SM_CYMINTRACK = 35,
			SM_CXDOUBLECLK = 36,
			SM_CYDOUBLECLK = 37,
			SM_CXICONSPACING = 38,
			SM_CYICONSPACING = 39,
			SM_MENUDROPALIGNMENT = 40,
			SM_PENWINDOWS = 41,
			SM_DBCSENABLED = 42,
			SM_CMOUSEBUTTONS = 43,
			SM_SECURE = 44,
			SM_CXEDGE = 45,
			SM_CYEDGE = 46,
			SM_CXMINSPACING = 47,
			SM_CYMINSPACING = 48,
			SM_CXSMICON = 49,
			SM_CYSMICON = 50,
			SM_CYSMCAPTION = 51,
			SM_CXSMSIZE = 52,
			SM_CYSMSIZE = 53,
			SM_CXMENUSIZE = 54,
			SM_CYMENUSIZE = 55,
			SM_ARRANGE = 56,
			SM_CXMINIMIZED = 57,
			SM_CYMINIMIZED = 58,
			SM_CXMAXTRACK = 59,
			SM_CYMAXTRACK = 60,
			SM_CXMAXIMIZED = 61,
			SM_CYMAXIMIZED = 62,
			SM_NETWORK = 63,
			SM_CLEANBOOT = 67,
			SM_CXDRAG = 68,
			SM_CYDRAG = 69,
			SM_SHOWSOUNDS = 70,
			SM_CXMENUCHECK = 71,
			SM_CYMENUCHECK = 72,
			SM_SLOWMACHINE = 73,
			SM_MIDEASTENABLED = 74,
			SM_MOUSEWHEELPRESENT = 75,
			SM_XVIRTUALSCREEN = 76,
			SM_YVIRTUALSCREEN = 77,
			SM_CXVIRTUALSCREEN = 78,
			SM_CYVIRTUALSCREEN = 79,
			SM_CMONITORS = 80,
			SM_SAMEDISPLAYFORMAT = 81,
			SM_IMMENABLED = 82,
			SM_CXFOCUSBORDER = 83,
			SM_CYFOCUSBORDER = 84,
			SM_TABLETPC = 86,
			SM_MEDIACENTER = 87,
			SM_STARTER = 88,
			SM_SERVERR2 = 89,
			SM_MOUSEHORIZONTALWHEELPRESENT = 91,
			SM_CXPADDEDBORDER = 92,
			SM_DIGITIZER = 94,
			SM_MAXIMUMTOUCHES = 95,
			SM_REMOTESESSION = 4096,
			SM_SHUTTINGDOWN = 8192,
			SM_REMOTECONTROL = 8193
		}

		[Flags]
		public enum ThreadAccess
		{
			TERMINATE = 1,
			SUSPEND_RESUME = 2,
			GET_CONTEXT = 8,
			SET_CONTEXT = 16,
			SET_INFORMATION = 32,
			QUERY_INFORMATION = 64,
			SET_THREAD_TOKEN = 128,
			IMPERSONATE = 256,
			DIRECT_IMPERSONATION = 512
		}

		public struct Win32FindData
		{
			public FileAttributes dwFileAttributes;

			public Alex.WoWRelogger.Utility.NativeMethods.Filetime ftCreationTime;

			public Alex.WoWRelogger.Utility.NativeMethods.Filetime ftLastAccessTime;

			public Alex.WoWRelogger.Utility.NativeMethods.Filetime ftLastWriteTime;

			public uint nFileSizeHigh;

			public uint nFileSizeLow;

			public uint dwReserved0;

			public uint dwReserved1;

			public string cFileName;

			public string cAlternate;
		}

		public struct WindowInfo
		{
			public uint cbSize;

			public Alex.WoWRelogger.Utility.NativeMethods.Rect rcWindow;

			public Alex.WoWRelogger.Utility.NativeMethods.Rect rcClient;

			public uint dwStyle;

			public uint dwExStyle;

			public uint dwWindowStatus;

			public uint cxWindowBorders;

			public uint cyWindowBorders;

			public ushort atomWindowType;

			public ushort wCreatorVersion;

			public WindowInfo(bool? filler)
			{
				this = new Alex.WoWRelogger.Utility.NativeMethods.WindowInfo()
				{
					cbSize = (uint)Marshal.SizeOf(typeof(Alex.WoWRelogger.Utility.NativeMethods.WindowInfo))
				};
			}
		}
	}
}