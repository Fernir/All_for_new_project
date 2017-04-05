namespace nManager.Helpful
{
    using nManager.Helpful.Win32;
    using System;

    public class Mouse
    {
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const int MOUSEEVENTF_LEFTDOWN = 2;
        private const int MOUSEEVENTF_LEFTUP = 4;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;
        private const int MOUSEEVENTF_RIGHTDOWN = 8;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void ClickLeft()
        {
            try
            {
                Native.mouse_event(0x8002, 0, 0, 0, 0);
                Native.mouse_event(0x8004, 0, 0, 0, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ClickLeft(): " + exception, true);
            }
        }

        public static void ClickRight()
        {
            try
            {
                Native.mouse_event(0x8008, 0, 0, 0, 0);
                Native.mouse_event(0x8010, 0, 0, 0, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ClickRight(): " + exception, true);
            }
        }

        public static void ClickRoller()
        {
            try
            {
                Native.mouse_event(0x8020, 0, 0, 0, 0);
                Native.mouse_event(0x8040, 0, 0, 0, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ClickRoller(): " + exception, true);
            }
        }

        public static void CurseurPosition(int posX, int posY)
        {
            try
            {
                Native.SetCursorPos(posX, posY);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CurseurPosition(int posX, int posY): " + exception, true);
            }
        }

        public static void CurseurWindowPercentagePosition(IntPtr mainWindowHandle, int percentageX, int percentageY)
        {
            try
            {
                Native.SetCursorPos(Display.GetWindowPosX(mainWindowHandle) + ((percentageX * Display.GetWindowWidth(mainWindowHandle)) / 100), Display.GetWindowPosY(mainWindowHandle) + ((percentageY * Display.GetWindowHeight(mainWindowHandle)) / 100));
            }
            catch (Exception exception)
            {
                Logging.WriteError("CurseurWindowPercentagePosition(IntPtr mainWindowHandle, int percentageX, int percentageY): " + exception, true);
            }
        }

        public static void CurseurWindowPosition(IntPtr mainWindowHandle, int posX, int posY)
        {
            try
            {
                Native.SetCursorPos(Display.GetWindowPosX(mainWindowHandle) + posX, Display.GetWindowPosY(mainWindowHandle) + posY);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CurseurWindowPosition(IntPtr mainWindowHandle, int posX, int posY): " + exception, true);
            }
        }
    }
}

