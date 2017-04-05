namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    public static class Lua
    {
        public static string GetLocalizedText(string commandline)
        {
            try
            {
                while (!Usefuls.InGame && Usefuls.IsLoading)
                {
                    Memory.WowMemory.GameFrameUnLock();
                    Thread.Sleep(200);
                }
                if (!Usefuls.InGame && !Usefuls.IsLoading)
                {
                    return "NOT_CONNECTED";
                }
                string s = commandline;
                if (s.Replace(" ", "").Length <= 0)
                {
                    return "";
                }
                uint dwAddress = Memory.WowMemory.Memory.AllocateMemory((Encoding.UTF8.GetBytes(s).Length + 1) + Others.Random(1, 0x19));
                if (dwAddress <= 0)
                {
                    return "";
                }
                Memory.WowMemory.Memory.WriteBytes(dwAddress, Encoding.UTF8.GetBytes(s));
                uint num2 = Memory.WowMemory.Memory.AllocateMemory(4);
                string[] asm = new string[] { "call " + (Memory.WowProcess.WowModule + 0x8dd5a), "test eax, eax", "je @out", "mov ecx, eax", "push -1", "mov edx, " + dwAddress, "push edx", "call " + (Memory.WowProcess.WowModule + 0x32a5c0), "mov [" + num2 + "], eax", "test eax, eax", "je @out", "push eax", "call " + (Memory.WowProcess.WowModule + 0x7ba7c0), "add esp, 4", "@out:", "retn" };
                uint num3 = Memory.WowMemory.InjectAndExecute(asm);
                uint num4 = Memory.WowMemory.Memory.ReadUInt(num2);
                string str2 = "";
                if (num4 > 0)
                {
                    str2 = Memory.WowMemory.Memory.ReadUTF8String(num4, (int) num3);
                }
                Memory.WowMemory.Memory.FreeMemory(dwAddress);
                Memory.WowMemory.Memory.FreeMemory(num2);
                return str2;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetLocalizedText(string commandline): " + exception, true);
                return "";
            }
        }

        public static void LuaDoString(List<string> command, bool notInGameMode = false)
        {
            try
            {
                LuaDoString(command.Aggregate<string, string>("", (current, line) => current + " " + line), notInGameMode, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LuaDoString(List<string> command, bool notInGameMode = false): " + exception, true);
            }
        }

        public static void LuaDoString(string command, bool notInGameMode = false, bool doAntiAfk = true)
        {
            try
            {
                if ((notInGameMode || Usefuls.InGame) || Usefuls.IsLoading)
                {
                    goto Label_0034;
                }
                Memory.WowMemory.GameFrameUnLock();
                return;
            Label_0020:
                Memory.WowMemory.GameFrameUnLock();
                Thread.Sleep(200);
            Label_0034:
                if ((!notInGameMode && !Usefuls.InGame) || Usefuls.IsLoading)
                {
                    goto Label_0020;
                }
                if (!notInGameMode && doAntiAfk)
                {
                    Usefuls.UpdateLastHardwareAction();
                }
                if (command.Replace(" ", "").Length > 0)
                {
                    uint dwAddress = Memory.WowMemory.Memory.AllocateMemory((Encoding.UTF8.GetBytes(command).Length + 1) + Others.Random(1, 0x19));
                    if (dwAddress > 0)
                    {
                        Memory.WowMemory.Memory.WriteBytes(dwAddress, Encoding.UTF8.GetBytes(command));
                        string[] collection = new string[] { "mov eax, " + dwAddress, "push 0", "push eax", "push eax", "mov eax, " + (Memory.WowProcess.WowModule + 0xb2e28), "call eax", "add esp, 0xC", "@out:", "retn" };
                        if (!notInGameMode)
                        {
                            List<string> list = new List<string> {
                                "call " + (Memory.WowProcess.WowModule + ((uint) 0x8dd5a)),
                                "test eax, eax",
                                "je @out"
                            };
                            list.AddRange(collection);
                            collection = list.ToArray();
                        }
                        else
                        {
                            List<string> list3 = new List<string>();
                            list3.AddRange(collection);
                            collection = list3.ToArray();
                        }
                        Memory.WowMemory.InjectAndExecute(collection);
                        Memory.WowMemory.Memory.FreeMemory(dwAddress);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LuaDoString(string command, bool notInGameMode = false): " + exception, true);
            }
        }

        public static string LuaDoString(string command, string returnArgument, bool notInGameMode = false)
        {
            string localizedText;
            try
            {
                lock ("LuaWithReturnValue")
                {
                    LuaDoString(command, notInGameMode, true);
                    localizedText = GetLocalizedText(returnArgument);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LuaDoString(string command, string returnArgument, bool notInGameMode = false): " + exception, true);
                localizedText = "";
            }
            return localizedText;
        }

        public static void RunMacroText(string macro)
        {
            try
            {
                LuaDoString("RunMacroText(\"" + macro + "\")", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("RunMacroText(string macro): " + exception, true);
            }
        }
    }
}

