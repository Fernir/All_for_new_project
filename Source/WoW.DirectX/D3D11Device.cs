using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WoW.DirectX
{
	internal sealed class D3D11Device : D3DDevice
	{
		private const int DXGI_FORMAT_R8G8B8A8_UNORM = 28;

		private const int DXGI_USAGE_RENDER_TARGET_OUTPUT = 32;

		private const int D3D11_SDK_VERSION = 7;

		private const int D3D_DRIVER_TYPE_HARDWARE = 1;

		private IntPtr _swapChain;

		private IntPtr _device;

		private IntPtr _myDxgiDll;

		private IntPtr _theirDxgiDll;

		private D3DDevice.VTableFuncDelegate _deviceRelease;

		private D3DDevice.VTableFuncDelegate _deviceContextRelease;

		private D3DDevice.VTableFuncDelegate _swapchainRelease;

		public override int EndSceneVtableIndex
		{
			get
			{
				return 28;
			}
		}

		public override int PresentVtableIndex
		{
			get
			{
				return 8;
			}
		}

		public D3D11Device(Process targetProc) : base(targetProc, "d3d11.dll")
		{
		}

		protected override void CleanD3D()
		{
			if (this._swapChain != IntPtr.Zero)
			{
				this._swapchainRelease(this._swapChain);
			}
			if (this._device != IntPtr.Zero)
			{
				this._deviceRelease(this._device);
			}
			if (this.D3DDevicePtr != IntPtr.Zero)
			{
				this._deviceContextRelease(this.D3DDevicePtr);
			}
		}

		[DllImport("d3d11.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern unsafe int D3D11CreateDeviceAndSwapChain(void* pAdapter, int driverType, void* Software, int flags, void* pFeatureLevels, int FeatureLevels, int SDKVersion, void* pSwapChainDesc, void* ppSwapChain, void* ppDevice, void* pFeatureLevel, void* ppImmediateContext);

	    public IntPtr GetSwapVTableFuncAbsoluteAddress(int funcIndex)
	    {
	        unsafe
	        {
	            return IntPtr.Add(this._theirDxgiDll,
	                IntPtr.Subtract(*(IntPtr*) ((int) (*(IntPtr*) (void*) this._swapChain) + funcIndex*4),
	                    this._myDxgiDll.ToInt32()).ToInt32());
	        }


	    }

	    protected override void InitD3D(out IntPtr d3DDevicePtr)
		{
			unsafe
			{
				this.LoadDxgiDll();
				D3D11Device.SwapChainDescription swapChainDescription = new D3D11Device.SwapChainDescription()
				{
					BufferCount = 1,
					ModeDescription = new D3D11Device.ModeDescription()
					{
						Format = 28
					},
					Usage = 32,
					OutputHandle = base.Form.Handle,
					SampleDescription = new D3D11Device.SampleDescription()
					{
						Count = 1
					},
					IsWindowed = true
				};
				D3D11Device.SwapChainDescription swapChainDescription1 = swapChainDescription;
				IntPtr zero = IntPtr.Zero;
				IntPtr intPtr = IntPtr.Zero;
				IntPtr zero1 = IntPtr.Zero;
				this._swapChain = zero;
				this._device = intPtr;
				d3DDevicePtr = zero1;
			    if (
                    D3D11Device.D3D11CreateDeviceAndSwapChain((void*)IntPtr.Zero, 1, (void*)IntPtr.Zero, 0, (void*)IntPtr.Zero,
                        0, 7, (void*)&swapChainDescription1, (void*)&zero, (void*)&intPtr, (void*)IntPtr.Zero, (void*)&zero1) >= 0)
                {
			        this._swapchainRelease =
			            base.GetDelegate<D3DDevice.VTableFuncDelegate>(base.GetVTableFuncAddress(this._swapChain, 2));
			        this._deviceRelease =
			            base.GetDelegate<D3DDevice.VTableFuncDelegate>(base.GetVTableFuncAddress(this._device, 2));
			        this._deviceContextRelease =
			            base.GetDelegate<D3DDevice.VTableFuncDelegate>(base.GetVTableFuncAddress(d3DDevicePtr, 2));
			    }
			}
		}

		private void LoadDxgiDll()
		{
			this._myDxgiDll = base.LoadLibrary("dxgi.dll");
			if (this._myDxgiDll == IntPtr.Zero)
			{
				throw new Exception("Could not load dxgi.dll");
			}
			this._theirDxgiDll = this.TargetProcess.Modules.Cast<ProcessModule>().First<ProcessModule>((ProcessModule m) => m.ModuleName == "dxgi.dll").BaseAddress;
		}

		private struct ModeDescription
		{
			private readonly int Width;

			private readonly int Height;

			private readonly D3D11Device.Rational RefreshRate;

			public int Format;

			private readonly int ScanlineOrdering;

			private readonly int Scaling;
		}

		private struct Rational
		{
			private readonly int Numerator;

			private readonly int Denominator;
		}

		private struct SampleDescription
		{
			public int Count;

			private readonly int Quality;
		}

		private struct SwapChainDescription
		{
			public D3D11Device.ModeDescription ModeDescription;

			public D3D11Device.SampleDescription SampleDescription;

			public int Usage;

			public int BufferCount;

			public IntPtr OutputHandle;

			public bool IsWindowed;

			private readonly int SwapEffect;

			private readonly int Flags;
		}

		private struct VTableIndexes
		{
			public const int DXGISwapChainRelease = 2;

			public const int D3D11DeviceRelease = 2;

			public const int D3D11DeviceContextRelease = 2;

			public const int DXGISwapChainPresent = 8;

			public const int D3D11DeviceContextBegin = 27;

			public const int D3D11DeviceContextEnd = 28;
		}
	}
}