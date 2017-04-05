using Alex.WoWRelogger;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using Fasm;
using GreyMagic;
using GreyMagic.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using WoW.DirectX;

namespace iRobot
{
	internal class Hook
	{
		private readonly static string[] RegisterNames;

		private readonly static string[] Register32BitNames;

		private readonly Dirext3D _dx3D;

		private readonly object _executeLockObject = new object();

		private readonly System.Diagnostics.Process _wowProcess;

		private IntPtr _addresseInjection;

		private byte[] _endSceneOriginalBytes;

		private IntPtr _fixHBStub;

		private IntPtr _injectedCode;

		private byte[] _myEndSceneOriginalBytes = new byte[] { 144, 144, 144, 144, 144, 139, 255 };

		private IntPtr _retnInjectionAsm;

		public Alex.WoWRelogger.WoW.WowManager WowManager;

		public bool Installed
		{
			get;
			private set;
		}

		public ExternalProcessReader Memory
		{
			get; private set;
        }

		public System.Diagnostics.Process Process
		{
			get; private set;
        }

		private static bool UsingWin8
		{
			get
			{
				if (Environment.OSVersion.Version.Major != 6)
				{
					return false;
				}
				return Environment.OSVersion.Version.Minor == 2;
			}
		}

		static Hook()
		{
			Hook.RegisterNames = new string[] { "AH", "AL", "BH", "BL", "CH", "CL", "DH", "DL", "EAX", "EBX", "ECX", "EDX" };
			Hook.Register32BitNames = new string[] { "EAX", "EBX", "ECX", "EDX" };
		}

		public Hook(System.Diagnostics.Process wowProc, Alex.WoWRelogger.WoW.WowManager wowManager)
		{
			this.Process = wowProc;
			this.WowManager = wowManager;
			this.Memory = new ExternalProcessReader(wowProc);
			this._wowProcess = wowProc;
			this.Installed = false;
		}

		private void AddAsmAndRandomOPs(string asm)
		{
			this.InsertRandomOpCodes();
			this.Memory.Asm.AddLine(asm);
			this.InsertRandomOpCodes();
		}

		public void DisposeHooking()
		{
			try
			{
				Alex.WoWRelogger.Utility.Log.Debug("Dispose hooking", new object[0]);
				if (this.Memory.Read<byte>(this._dx3D.HookPtr, false) == 235)
				{
					if (this._endSceneOriginalBytes != null)
					{
						this.Memory.WriteBytes(this._dx3D.HookPtr - 5, this._endSceneOriginalBytes, false);
					}
					else if (!Hook.UsingWin8 || this._dx3D.UsingDirectX11)
					{
						this.Memory.WriteBytes(this._dx3D.HookPtr - 5, this._myEndSceneOriginalBytes, false);
					}
					else
					{
						this._myEndSceneOriginalBytes = new byte[] { 144, 144, 144, 144, 144, 144, 144 };
						this.Memory.WriteBytes(this._dx3D.HookPtr - 5, this._myEndSceneOriginalBytes, false);
					}
				}
				this.Memory.FreeMemory(this._injectedCode);
				this.Memory.FreeMemory(this._addresseInjection);
				this.Memory.FreeMemory(this._retnInjectionAsm);
				this.Installed = false;
			}
			catch (Exception exception)
			{
				Alex.WoWRelogger.Utility.Log.Write(exception.ToString(), new object[0]);
			}
		}

		private void FixEndSceneForHB(IntPtr pEndScene)
		{
			this.Memory.Asm.Clear();
			this._fixHBStub = this.Memory.AllocateMemory(512, MemoryAllocationType.MEM_COMMIT, MemoryProtectionType.PAGE_EXECUTE_READWRITE);
			this.AddAsmAndRandomOPs("push ebx");
			this.AddAsmAndRandomOPs(string.Concat("mov bl, [", pEndScene.ToString(), "]"));
			this.AddAsmAndRandomOPs("cmp bl, 0xE9");
			this.AddAsmAndRandomOPs("jnz @HbIsNotHooked");
			this.AddAsmAndRandomOPs("pop ebx");
			this.AddAsmAndRandomOPs("pop ebp");
			this.AddAsmAndRandomOPs("jmp @original");
			this.Memory.Asm.AddLine("@HbIsNotHooked:");
			this.AddAsmAndRandomOPs("pop ebx");
			this.Memory.Asm.AddLine("@original:");
			this.AddAsmAndRandomOPs("Push 0x14");
			this.AddAsmAndRandomOPs(string.Concat("Mov Eax, ", this.Memory.Read<uint>(pEndScene + 3, false)));
			IntPtr intPtr = (pEndScene + 12) + this.Memory.Read<int>(pEndScene + 8, false);
			this.AddAsmAndRandomOPs(string.Concat("Call ", (uint)((int)intPtr - (int)this._fixHBStub)));
			this.AddAsmAndRandomOPs(string.Concat("Jmp ", (uint)((int)pEndScene + 12 - (int)this._fixHBStub)));
			this.Memory.WriteBytes(this._fixHBStub, this.Memory.Asm.Assemble(), false);
			this.Memory.Asm.Clear();
			int num = Helper.Rand.Next(0, 2);
			if (num == 0)
			{
				if (Helper.Rand.Next(2) != 1)
				{
					this.Memory.Asm.Add("Nop\nNop\n");
				}
				else
				{
					this.InsertRandomMov();
				}
				if (Helper.Rand.Next(2) != 1)
				{
					this.Memory.Asm.Add("Nop\nNop\n");
				}
				else
				{
					this.InsertRandomMov();
				}
				this.Memory.Asm.AddLine("Nop");
				if (Helper.Rand.Next(2) != 1)
				{
					this.Memory.Asm.Add("Nop\nNop\n");
				}
				else
				{
					this.InsertRandomMov();
				}
				this.Memory.Asm.AddLine(string.Concat("Jmp ", (uint)((int)this._fixHBStub - (int)pEndScene)));
			}
			else if (num == 1)
			{
				if (Helper.Rand.Next(2) != 1)
				{
					this.Memory.Asm.Add("Nop\nNop\n");
				}
				else
				{
					this.InsertRandomMov();
				}
				if (Helper.Rand.Next(2) != 1)
				{
					this.Memory.Asm.Add("Nop\nNop\n");
				}
				else
				{
					this.InsertRandomMov();
				}
				this.Memory.Asm.AddLine("Nop");
				this.Memory.Asm.AddLine(string.Concat("Jmp ", (uint)((int)this._fixHBStub - (int)pEndScene)));
				if (Helper.Rand.Next(2) != 1)
				{
					this.Memory.Asm.Add("Nop\nNop\n");
				}
				else
				{
					this.InsertRandomMov();
				}
			}
			this.Memory.WriteBytes(pEndScene, this.Memory.Asm.Assemble(), false);
			this.Memory.Asm.Clear();
		}

		public byte[] InjectAndExecute(IEnumerable<string> asm, int returnLength = 0)
		{
			byte[] numArray;
			lock (this._executeLockObject)
			{
				byte[] numArray1 = new byte[returnLength];
				try
				{
					if (this._wowProcess == null || this._wowProcess.HasExitedSafe())
					{
						throw new Exception();
					}
					if (this._retnInjectionAsm == IntPtr.Zero || this._addresseInjection == IntPtr.Zero)
					{
						throw new Exception();
					}
					this.Memory.Write<int>(this._retnInjectionAsm, 0, false);
					if (!this.Memory.IsProcessOpen || !this.Installed)
					{
						throw new Exception();
					}
					this.Memory.Asm.Clear();
					foreach (string str in asm)
					{
						this.Memory.Asm.AddLine(str);
					}
					IntPtr intPtr = this.Memory.AllocateMemory(32840, MemoryAllocationType.MEM_COMMIT, MemoryProtectionType.PAGE_EXECUTE_READWRITE);
					if (intPtr == IntPtr.Zero)
					{
						throw new Exception();
					}
					this.Memory.Asm.Inject((uint)(int)intPtr);
					if (!this.Memory.Write<int>(this._addresseInjection, (int)intPtr, false))
					{
						throw new Exception();
					}
					int num2 = 0;
					while (this.Memory.Read<int>(this._addresseInjection, false) > 0)
					{
						Helper.SleepUntil(() => {
							int num = this.Memory.Read<int>(this._addresseInjection, false);
							int num1 = num;
							num2 = num;
							return num1 == 0;
						}, new TimeSpan(0, 0, 5));
					}
					if (num2 != 0)
					{
						throw new Exception();
					}
					if (returnLength <= 0)
					{
						numArray = null;
						return numArray;
					}
					else
					{
						numArray1 = this.Memory.ReadBytes(this._retnInjectionAsm, returnLength, false);
						Timer timer = new Timer((object state) => this.Memory.FreeMemory((IntPtr)state), (object)intPtr, 5500, 0);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					Alex.WoWRelogger.Utility.Log.Err(exception.Source, new object[0]);
					throw exception;
				}
				numArray = numArray1;
			}
			return numArray;
		}

		private void InsertRandomMov()
		{
			int num = Helper.Rand.Next(0, (int)Hook.RegisterNames.Length);
			this.Memory.Asm.AddLine("mov {0}, {1}", new object[] { Hook.RegisterNames[num], Hook.RegisterNames[num] });
		}

		private void InsertRandomOpCodes()
		{
			for (int i = Helper.Rand.Next(1); i >= 0; i--)
			{
				int num = Helper.Rand.Next(0, 6);
				if (num == 0)
				{
					this.Memory.Asm.AddLine("nop");
					if (Helper.Rand.Next(2) == 1)
					{
						this.Memory.Asm.AddLine("nop");
					}
				}
				else if (num <= 5)
				{
					this.InsertRandomMov();
				}
			}
		}

		private void InsertRandomPushPop()
		{
			int num = Helper.Rand.Next(0, (int)Hook.Register32BitNames.Length);
			this.Memory.Asm.AddLine("push {0}", new object[] { Hook.Register32BitNames[num] });
			this.Memory.Asm.AddLine("pop {0}", new object[] { Hook.Register32BitNames[num] });
		}

		public void InstallHook()
		{
			try
			{
				if (this.Memory.IsProcessOpen)
				{
					this.WowManager.Profile.Log("Hooking started", new object[0]);
					if (this.Memory.Read<byte>(this._dx3D.HookPtr, false) == 235 && (this._injectedCode == IntPtr.Zero || this._addresseInjection == IntPtr.Zero))
					{
						this.DisposeHooking();
					}
					this.Installed = false;
					this._injectedCode = this.Memory.AllocateMemory(8264, MemoryAllocationType.MEM_COMMIT, MemoryProtectionType.PAGE_EXECUTE_READWRITE);
					this._addresseInjection = this.Memory.AllocateMemory(4, MemoryAllocationType.MEM_COMMIT, MemoryProtectionType.PAGE_EXECUTE_READWRITE);
					this.Memory.Write<int>(this._addresseInjection, 0, false);
					this._retnInjectionAsm = this.Memory.AllocateMemory(4, MemoryAllocationType.MEM_COMMIT, MemoryProtectionType.PAGE_EXECUTE_READWRITE);
					this.Memory.Write<int>(this._retnInjectionAsm, 0, false);
					this.Memory.Asm.Clear();
					this.AddAsmAndRandomOPs("pushad");
					this.AddAsmAndRandomOPs("pushfd");
					this.AddAsmAndRandomOPs(string.Concat("mov eax, [", this._addresseInjection.ToString(), "]"));
					this.AddAsmAndRandomOPs("test eax, eax");
					this.AddAsmAndRandomOPs("je @out");
					this.AddAsmAndRandomOPs(string.Concat("mov eax, [", this._addresseInjection.ToString(), "]"));
					this.AddAsmAndRandomOPs("call eax");
					this.AddAsmAndRandomOPs(string.Concat("mov [", this._retnInjectionAsm.ToString(), "], eax"));
					this.AddAsmAndRandomOPs(string.Concat("mov edx, ", this._addresseInjection.ToString()));
					this.AddAsmAndRandomOPs("mov ecx, 0");
					this.AddAsmAndRandomOPs("mov [edx], ecx");
					this.AddAsmAndRandomOPs("@out:");
					this.AddAsmAndRandomOPs("popfd");
					this.AddAsmAndRandomOPs("popad");
					uint length = (uint)this.Memory.Asm.Assemble().Length;

                    this.Memory.Asm.Inject((uint)(int)this._injectedCode);
					this._endSceneOriginalBytes = this.Memory.ReadBytes(this._dx3D.HookPtr - 5, 7, false);
					string str = "";
					byte[] numArray = this._endSceneOriginalBytes;
					for (int i = 0; i < (int)numArray.Length; i++)
					{
						byte num = numArray[i];
						str = string.Concat(str, num, ", ");
					}
					this.WowManager.Profile.Log("Original EndSceneBytes = ({0})", new object[] { str });
					this.Memory.WriteBytes(IntPtr.Add(this._injectedCode, (int)length), new byte[] { this._endSceneOriginalBytes[5], this._endSceneOriginalBytes[6] }, false);
					this.Memory.Asm.Clear();
					this.Memory.Asm.AddLine(string.Concat("jmp ", (uint)((int)this._dx3D.HookPtr + 2)));
					this.Memory.Asm.Inject((uint)(int)this._injectedCode + length + 2);
					this.Memory.Asm.Clear();
					this.Memory.Asm.AddLine("@top:");
					this.Memory.Asm.AddLine(string.Concat("jmp ", this._injectedCode.ToString()));
					this.Memory.Asm.AddLine("jmp @top");
					this.Memory.Asm.Inject((uint)(int)this._dx3D.HookPtr - 5);
					this.Installed = true;
					this.WowManager.Profile.Log("Hook installed", new object[0]);
				}
			}
			catch (Exception exception)
			{
				Alex.WoWRelogger.Utility.Log.Write(exception.ToString(), new object[0]);
			}
		}
	}
}