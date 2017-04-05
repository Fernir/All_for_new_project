namespace nManager.Helpful
{
    using SlimDX.Direct3D9;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public static class Direct3DCapture
    {
        private static Direct3D _direct3D9 = new Direct3D();
        private static Dictionary<IntPtr, Device> _direct3DDeviceCache = new Dictionary<IntPtr, Device>();

        public static Bitmap CaptureRegionDirect3D(IntPtr handle, Rectangle region)
        {
            Device device;
            IntPtr key = handle;
            AdapterInformation defaultAdapter = _direct3D9.Adapters.DefaultAdapter;
            if (_direct3DDeviceCache.ContainsKey(key))
            {
                device = _direct3DDeviceCache[key];
            }
            else
            {
                PresentParameters parameters = new PresentParameters {
                    BackBufferFormat = defaultAdapter.CurrentDisplayMode.Format
                };
                Rectangle absoluteClientRect = nManager.Helpful.NativeMethods.GetAbsoluteClientRect(key);
                parameters.BackBufferHeight = absoluteClientRect.Height;
                parameters.BackBufferWidth = absoluteClientRect.Width;
                parameters.Multisample = MultisampleType.None;
                parameters.SwapEffect = SwapEffect.Discard;
                parameters.DeviceWindowHandle = key;
                parameters.PresentationInterval = PresentInterval.Default;
                parameters.FullScreenRefreshRateInHertz = 0;
                device = new Device(_direct3D9, defaultAdapter.Adapter, DeviceType.Hardware, key, CreateFlags.SoftwareVertexProcessing, new PresentParameters[] { parameters });
                _direct3DDeviceCache.Add(key, device);
            }
            using (Surface surface = Surface.CreateOffscreenPlain(device, defaultAdapter.CurrentDisplayMode.Width, defaultAdapter.CurrentDisplayMode.Height, Format.A8R8G8B8, Pool.SystemMemory))
            {
                device.GetFrontBufferData(0, surface);
                return new Bitmap(Surface.ToStream(surface, ImageFileFormat.Bmp, new Rectangle(region.Left, region.Top, region.Width, region.Height)));
            }
        }

        public static Bitmap CaptureWindow(IntPtr hWnd, Size size)
        {
            return ResizeImage(CaptureRegionDirect3D(hWnd, nManager.Helpful.NativeMethods.GetAbsoluteClientRect(hWnd)), size);
        }

        public static Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {
            try
            {
                Bitmap image = new Bitmap(size.Width, size.Height);
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                }
                return image;
            }
            catch
            {
            }
            return null;
        }
    }
}

