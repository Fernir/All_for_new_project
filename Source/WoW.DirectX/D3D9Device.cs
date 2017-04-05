using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WoW.DirectX
{
	internal sealed class D3D9Device : D3DDevice
	{
		private const int D3D9SdkVersion = 32;

		private const int D3DCREATE_SOFTWARE_VERTEXPROCESSING = 32;

		private D3DDevice.VTableFuncDelegate _d3DDeviceRelease;

		private D3DDevice.VTableFuncDelegate _d3DRelease;

		private IntPtr _pD3D;

		public override int EndSceneVtableIndex
		{
			get
			{
				return 42;
			}
		}

		public override int PresentVtableIndex
		{
			get
			{
				return 17;
			}
		}

		public D3D9Device(Process targetProc) : base(targetProc, "d3d9.dll")
		{
		}

		protected override void CleanD3D()
		{
			if (this.D3DDevicePtr != IntPtr.Zero)
			{
				this._d3DDeviceRelease(this.D3DDevicePtr);
			}
			if (this._pD3D != IntPtr.Zero)
			{
				this._d3DRelease(this._pD3D);
			}
		}

		[DllImport("d3d9.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern IntPtr Direct3DCreate9(uint sdkVersion);

		protected override void InitD3D(out IntPtr d3DDevicePtr)
		{
			this._pD3D = D3D9Device.Direct3DCreate9(32);
			if (this._pD3D == IntPtr.Zero)
			{
				throw new Exception("Failed to create D3D.");
			}
			D3D9Device.D3DPresentParameters d3DPresentParameter = new D3D9Device.D3DPresentParameters()
			{
				Windowed = true,
				SwapEffect = 1,
				BackBufferFormat = 0
			};
			D3D9Device.D3DPresentParameters d3DPresentParameter1 = d3DPresentParameter;
			if (base.GetDelegate<D3D9Device.CreateDeviceDelegate>(base.GetVTableFuncAddress(this._pD3D, 16))(this._pD3D, 0, 1, base.Form.Handle, 32, ref d3DPresentParameter1, out d3DDevicePtr) < 0)
			{
				throw new Exception("Failed to create device.");
			}
			this._d3DDeviceRelease = base.GetDelegate<D3DDevice.VTableFuncDelegate>(base.GetVTableFuncAddress(this.D3DDevicePtr, 2));
			this._d3DRelease = base.GetDelegate<D3DDevice.VTableFuncDelegate>(base.GetVTableFuncAddress(this._pD3D, 2));
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int CreateDeviceDelegate(IntPtr instance, uint adapter, uint deviceType, IntPtr focusWindow, uint behaviorFlags, [In] ref D3D9Device.D3DPresentParameters presentationParameters, out IntPtr returnedDeviceInterface);

		private struct D3DPresentParameters
		{
			private readonly uint BackBufferWidth;

			private readonly uint BackBufferHeight;

			public uint BackBufferFormat;

			private readonly uint BackBufferCount;

			private readonly uint MultiSampleType;

			private readonly uint MultiSampleQuality;

			public uint SwapEffect;

			private readonly IntPtr hDeviceWindow;

			public bool Windowed;

			private readonly bool EnableAutoDepthStencil;

			private readonly uint AutoDepthStencilFormat;

			private readonly uint Flags;

			private readonly uint FullScreen_RefreshRateInHz;

			private readonly uint PresentationInterval;
		}

		private struct VTableIndexes
		{
			public const int Direct3D9Release = 2;

			public const int Direct3D9CreateDevice = 16;

			public const int Direct3DDevice9Release = 2;

			public const int Direct3DDevice9Present = 17;

			public const int Direct3DDevice9BeginScene = 41;

			public const int Direct3DDevice9EndScene = 42;
		}
	}
}