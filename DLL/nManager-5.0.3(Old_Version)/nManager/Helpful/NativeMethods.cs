namespace nManager.Helpful
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    internal sealed class NativeMethods
    {
        internal static Rectangle GetAbsoluteClientRect(IntPtr hWnd)
        {
            Rectangle windowRect = GetWindowRect(hWnd);
            Rectangle clientRect = GetClientRect(hWnd);
            int num = (windowRect.Width - clientRect.Width) / 2;
            return new Rectangle(new Point(windowRect.X + num, windowRect.Y + ((windowRect.Height - clientRect.Height) - num)), clientRect.Size);
        }

        internal static Rectangle GetClientRect(IntPtr hwnd)
        {
            nManager.Helpful.RECT lpRect = new nManager.Helpful.RECT();
            GetClientRect(hwnd, out lpRect);
            return lpRect.AsRectangle;
        }

        [DllImport("user32.dll")]
        internal static extern bool GetClientRect(IntPtr hWnd, out nManager.Helpful.RECT lpRect);
        internal static Rectangle GetWindowRect(IntPtr hwnd)
        {
            nManager.Helpful.RECT lpRect = new nManager.Helpful.RECT();
            GetWindowRect(hwnd, out lpRect);
            return lpRect.AsRectangle;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hWnd, out nManager.Helpful.RECT lpRect);
    }
}

