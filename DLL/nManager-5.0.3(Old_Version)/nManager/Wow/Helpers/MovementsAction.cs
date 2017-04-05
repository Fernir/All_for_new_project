namespace nManager.Wow.Helpers
{
    using nManager;
    using nManager.Wow.Enums;
    using System;
    using System.Runtime.InteropServices;

    public class MovementsAction
    {
        public static bool UseLUAToMove;

        public static void Ascend(bool start, bool redo = false, bool forceLUA = false)
        {
            if (start && !UseLUAToMove)
            {
                CloseChatFrameEditBox();
            }
            if (UseLUAToMove || forceLUA)
            {
                Lua.LuaDoString(start ? "JumpOrAscendStart();" : "AscendStop();", false, true);
            }
            else if (start)
            {
                nManager.Wow.Helpers.Keybindings.DownKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            }
            else
            {
                nManager.Wow.Helpers.Keybindings.UpKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            }
            if (redo)
            {
                Ascend(!start, false, false);
            }
        }

        public static void CloseChatFrameEditBox()
        {
            if (nManagerSetting.CurrentSetting.AutoCloseChatFrame)
            {
                Lua.LuaDoString("ChatFrame1EditBox:Hide();", false, true);
            }
        }

        public static void Descend(bool start, bool redo = false, bool forceLUA = false)
        {
            if (start && !UseLUAToMove)
            {
                CloseChatFrameEditBox();
            }
            if (UseLUAToMove || forceLUA)
            {
                Lua.LuaDoString(start ? "SitStandOrDescendStart();" : "DescendStop();", false, true);
            }
            else if (start)
            {
                nManager.Wow.Helpers.Keybindings.DownKeybindings(nManager.Wow.Enums.Keybindings.SITORSTAND);
            }
            else
            {
                nManager.Wow.Helpers.Keybindings.UpKeybindings(nManager.Wow.Enums.Keybindings.SITORSTAND);
            }
            if (redo)
            {
                Descend(!start, false, false);
            }
        }

        public static void Jump()
        {
            Ascend(true, true, false);
        }

        public static void MoveBackward(bool start, bool redo = false)
        {
            if (start && !UseLUAToMove)
            {
                CloseChatFrameEditBox();
            }
            if (UseLUAToMove)
            {
                Lua.LuaDoString(start ? "MoveBackwardStart();" : "MoveBackwardStop();", false, true);
            }
            else if (start)
            {
                nManager.Wow.Helpers.Keybindings.DownKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
            }
            else
            {
                nManager.Wow.Helpers.Keybindings.UpKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
            }
            if (redo)
            {
                MoveBackward(!start, false);
            }
        }

        public static void MoveForward(bool start, bool redo = false)
        {
            if (start && !UseLUAToMove)
            {
                CloseChatFrameEditBox();
            }
            if (UseLUAToMove)
            {
                Lua.LuaDoString(start ? "MoveForwardStart();" : "MoveForwardStop();", false, true);
            }
            else if (start)
            {
                nManager.Wow.Helpers.Keybindings.DownKeybindings(nManager.Wow.Enums.Keybindings.MOVEFORWARD);
            }
            else
            {
                nManager.Wow.Helpers.Keybindings.UpKeybindings(nManager.Wow.Enums.Keybindings.MOVEFORWARD);
            }
            if (redo)
            {
                MoveForward(!start, false);
            }
        }

        public static void StrafeLeft(bool start, bool redo = false)
        {
            if (start && !UseLUAToMove)
            {
                CloseChatFrameEditBox();
            }
            if (UseLUAToMove)
            {
                Lua.LuaDoString(start ? "StrafeLeftStart();" : "StrafeLeftStop();", false, true);
            }
            else if (start)
            {
                nManager.Wow.Helpers.Keybindings.DownKeybindings(nManager.Wow.Enums.Keybindings.STRAFELEFT);
            }
            else
            {
                nManager.Wow.Helpers.Keybindings.UpKeybindings(nManager.Wow.Enums.Keybindings.STRAFELEFT);
            }
            if (redo)
            {
                StrafeLeft(!start, false);
            }
        }

        public static void StrafeRight(bool start, bool redo = false)
        {
            if (start && !UseLUAToMove)
            {
                CloseChatFrameEditBox();
            }
            if (UseLUAToMove)
            {
                Lua.LuaDoString(start ? "StrafeRightStart();" : "StrafeRightStop();", false, true);
            }
            else if (start)
            {
                nManager.Wow.Helpers.Keybindings.DownKeybindings(nManager.Wow.Enums.Keybindings.STRAFERIGHT);
            }
            else
            {
                nManager.Wow.Helpers.Keybindings.UpKeybindings(nManager.Wow.Enums.Keybindings.STRAFERIGHT);
            }
            if (redo)
            {
                StrafeRight(!start, false);
            }
        }
    }
}

