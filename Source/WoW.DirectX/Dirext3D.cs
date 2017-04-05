using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WoW.DirectX
{
	internal class Dirext3D
	{
		public IntPtr HookPtr
		{
			get;
			private set;
		}

		private Process TargetProcess
		{
			get;
			set;
		}

		public bool UsingDirectX11
		{
			get;
			private set;
		}

		public Dirext3D(Process targetProc)
		{
			D3DDevice d3D11Device;
			this.TargetProcess = targetProc;
			this.UsingDirectX11 = this.TargetProcess.Modules.Cast<ProcessModule>().Any<ProcessModule>((ProcessModule m) => m.ModuleName == "d3d11.dll");
			if (this.UsingDirectX11)
			{
				d3D11Device = new D3D11Device(targetProc);
			}
			else
			{
				d3D11Device = new D3D9Device(targetProc);
			}
			using (D3DDevice d3DDevice = d3D11Device)
			{
				this.HookPtr = (this.UsingDirectX11 ? ((D3D11Device)d3DDevice).GetSwapVTableFuncAbsoluteAddress(d3DDevice.PresentVtableIndex) : d3DDevice.GetDeviceVTableFuncAbsoluteAddress(d3DDevice.EndSceneVtableIndex));
			}
		}
	}
}