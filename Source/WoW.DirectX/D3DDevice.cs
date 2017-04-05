using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WoW.DirectX
{
	internal abstract class D3DDevice : IDisposable
	{
		private readonly string _d3DDllName;

		protected readonly IntPtr D3DDevicePtr;

		protected readonly Process TargetProcess;

		private readonly List<IntPtr> _loadedLibraries = new List<IntPtr>();

		private IntPtr _myD3DDll;

		private IntPtr _theirD3DDll;

		private bool _disposed;

		public abstract int EndSceneVtableIndex
		{
			get;
		}

        protected System.Windows.Forms.Form Form
        {
            get;
            private set;
        }

		public abstract int PresentVtableIndex
		{
			get;
		}

		protected D3DDevice(Process targetProcess, string d3DDllName)
		{
			this.TargetProcess = targetProcess;
			this._d3DDllName = d3DDllName;
			this.Form = new System.Windows.Forms.Form();
			this.LoadDll();
			this.InitD3D(out this.D3DDevicePtr);
		}

		protected abstract void CleanD3D();

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					this.CleanD3D();
					if (this.Form != null)
					{
						this.Form.Dispose();
					}
					foreach (IntPtr _loadedLibrary in this._loadedLibraries)
					{
						D3DDevice.NativeMethods.FreeLibrary(_loadedLibrary);
					}
				}
				this._disposed = true;
			}
		}

		~D3DDevice()
		{
			this.Dispose(false);
		}

		protected T GetDelegate<T>(IntPtr address)
		where T : class
		{
			return (T)(Marshal.GetDelegateForFunctionPointer(address, typeof(T)) as T);
		}

		public IntPtr GetDeviceVTableFuncAbsoluteAddress(int funcIndex)
		{
			unsafe
			{
                return IntPtr.Add(this._theirD3DDll, IntPtr.Subtract(*(IntPtr*)((int)(*(IntPtr*)(void*)this.D3DDevicePtr) + funcIndex * 4), this._myD3DDll.ToInt32()).ToInt32());
            }
        }

		protected IntPtr GetVTableFuncAddress(IntPtr obj, int funcIndex)
		{
			unsafe
            {
                return *(IntPtr*)((int)(*(IntPtr*)(void*)obj) + funcIndex * 4);
            }
		}

		protected abstract void InitD3D(out IntPtr d3DDevicePtr);

		private void LoadDll()
		{
			this._myD3DDll = this.LoadLibrary(this._d3DDllName);
			if (this._myD3DDll == IntPtr.Zero)
			{
				throw new Exception(string.Format("Could not load {0}", this._d3DDllName));
			}
			this._theirD3DDll = this.TargetProcess.Modules.Cast<ProcessModule>().First<ProcessModule>((ProcessModule m) => m.ModuleName == this._d3DDllName).BaseAddress;
		}

		protected IntPtr LoadLibrary(string library)
		{
			IntPtr moduleHandle = D3DDevice.NativeMethods.GetModuleHandle(library);
			if (moduleHandle == IntPtr.Zero)
			{
				moduleHandle = D3DDevice.NativeMethods.LoadLibrary(library);
				this._loadedLibraries.Add(moduleHandle);
			}
			return moduleHandle;
		}

		public static class NativeMethods
		{
			[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
			public static extern bool FreeLibrary(IntPtr hModule);

			[DllImport("kernel32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
			public static extern IntPtr GetModuleHandle(string lpModuleName);

			[DllImport("kernel32", CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
			public static extern IntPtr LoadLibrary(string lpFileName);
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		protected delegate void VTableFuncDelegate(IntPtr instance);
	}
}