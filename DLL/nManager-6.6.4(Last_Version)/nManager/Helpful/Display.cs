namespace nManager.Helpful
{
    using nManager.Helpful.Win32;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    public static class Display
    {
        public static int GetWindowHeight(IntPtr mainWindowHandle)
        {
            try
            {
                Native.RECT rect = new Native.RECT();
                Native.GetWindowRect(mainWindowHandle, ref rect);
                return (rect.bottom - rect.top);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWindowHeight(IntPtr mainWindowHandle): " + exception, true);
                return 0;
            }
        }

        public static int GetWindowPosX(IntPtr mainWindowHandle)
        {
            try
            {
                Native.RECT rect = new Native.RECT();
                Native.GetWindowRect(mainWindowHandle, ref rect);
                return rect.left;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWindowPosX(IntPtr mainWindowHandle): " + exception, true);
                return 0;
            }
        }

        public static int GetWindowPosY(IntPtr mainWindowHandle)
        {
            try
            {
                Native.RECT rect = new Native.RECT();
                Native.GetWindowRect(mainWindowHandle, ref rect);
                return rect.top;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWindowPosY(IntPtr mainWindowHandle): " + exception, true);
                return 0;
            }
        }

        public static int GetWindowWidth(IntPtr mainWindowHandle)
        {
            try
            {
                Native.RECT rect = new Native.RECT();
                Native.GetWindowRect(mainWindowHandle, ref rect);
                return (rect.right - rect.left);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWindowWidth(IntPtr mainWindowHandle): " + exception, true);
                return 0;
            }
        }

        public static Image ScreenshotWindow(IntPtr mainWindowHandle)
        {
            try
            {
                IntPtr hWnd = mainWindowHandle;
                IntPtr windowDC = Native.GetWindowDC(hWnd);
                Native.RECT rect = new Native.RECT();
                Native.GetWindowRect(hWnd, ref rect);
                int nWidth = rect.right - rect.left;
                int nHeight = rect.bottom - rect.top;
                IntPtr hDC = Native.CreateCompatibleDC(windowDC);
                IntPtr hObject = Native.CreateCompatibleBitmap(windowDC, nWidth, nHeight);
                IntPtr ptr5 = Native.SelectObject(hDC, hObject);
                Native.BitBlt(hDC, 0, 0, nWidth, nHeight, windowDC, 0, 0, 0xcc0020);
                Native.SelectObject(hDC, ptr5);
                Native.DeleteDC(hDC);
                Native.ReleaseDC(hWnd, windowDC);
                Image image = Image.FromHbitmap(hObject);
                Native.DeleteObject(hObject);
                return image;
            }
            catch (Exception exception)
            {
                Logging.WriteError("ScreenshotWindow(IntPtr mainWindowHandle): " + exception, true);
            }
            return new Bitmap(1, 1);
        }

        public static void ScreenshotWindow(IntPtr mainWindowHandle, string filename, ImageFormat format)
        {
            try
            {
                ScreenshotWindow(mainWindowHandle).Save(filename, format);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ScreenshotWindow(IntPtr mainWindowHandle, string filename, ImageFormat format): " + exception, true);
            }
        }

        public static void SetWindowPositionSize(IntPtr mainWindowHandle, int x, int y, int width, int height)
        {
            try
            {
                Native.SetWindowPos(mainWindowHandle, IntPtr.Zero, x, y, width, height, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("SetWindowPositionSize(IntPtr mainWindowHandle, int x, int y, int width, int height): " + exception, true);
            }
        }

        public static void ShowWindow(IntPtr mainWindowHandle)
        {
            try
            {
                Native.ShowWindow(mainWindowHandle, 9);
                Native.SetForegroundWindow(mainWindowHandle);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowWindow(IntPtr mainWindowHandle): " + exception, true);
            }
        }

        public static bool WindowInTaskBarre(IntPtr mainWindowHandle)
        {
            try
            {
                return ((GetWindowPosY(mainWindowHandle) < -100) && (GetWindowPosX(mainWindowHandle) < -100));
            }
            catch (Exception exception)
            {
                Logging.WriteError("WindowInTaskBarre(IntPtr mainWindowHandle): " + exception, true);
                return false;
            }
        }
    }
}

