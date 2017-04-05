namespace nManager.Helpful
{
    using SlimDX.Direct3D9;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public static class Direct3DCapture
    {
        private static Dictionary<IntPtr, Device> _ejiripOsa = new Dictionary<IntPtr, Device>();
        private static Direct3D _juqeilaolWuiko = new Direct3D();

        public static Bitmap CaptureRegionDirect3D(IntPtr handle, Rectangle region)
        {
            Device device;
            IntPtr key = handle;
            AdapterInformation defaultAdapter = _juqeilaolWuiko.Adapters.DefaultAdapter;
            if (_ejiripOsa.ContainsKey(key))
            {
                device = _ejiripOsa[key];
            }
            else
            {
                PresentParameters parameters = new PresentParameters {
                    BackBufferFormat = defaultAdapter.CurrentDisplayMode.Format
                };
                Rectangle rectangle = Mibuwiugeawi.UvuwixoparoIjihuo(key);
                parameters.BackBufferHeight = rectangle.Height;
                parameters.BackBufferWidth = rectangle.Width;
                parameters.Multisample = MultisampleType.None;
                parameters.SwapEffect = SwapEffect.Discard;
                parameters.DeviceWindowHandle = key;
                parameters.PresentationInterval = PresentInterval.Default;
                parameters.FullScreenRefreshRateInHertz = 0;
                device = new Device(_juqeilaolWuiko, defaultAdapter.Adapter, DeviceType.Hardware, key, CreateFlags.SoftwareVertexProcessing, new PresentParameters[] { parameters });
                _ejiripOsa.Add(key, device);
            }
            using (Surface surface = Surface.CreateOffscreenPlain(device, defaultAdapter.CurrentDisplayMode.Width, defaultAdapter.CurrentDisplayMode.Height, Format.A8R8G8B8, Pool.SystemMemory))
            {
                device.GetFrontBufferData(0, surface);
                return new Bitmap(Surface.ToStream(surface, ImageFileFormat.Bmp, new Rectangle(region.Left, region.Top, region.Width, region.Height)));
            }
        }

        public static Bitmap CaptureWindow(IntPtr hWnd, Size size)
        {
            return ResizeImage(CaptureRegionDirect3D(hWnd, Mibuwiugeawi.UvuwixoparoIjihuo(hWnd)), size);
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

