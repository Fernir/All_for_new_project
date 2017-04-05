namespace nManager.Wow.Helpers
{
    using nManager;
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class ConfigWowForThisBot
    {
        public static void ConfigWow()
        {
            try
            {
                Thread thread2 = new Thread(new ThreadStart(ConfigWowForThisBot.Lehoeja)) {
                    Name = "config wow Thread"
                };
                thread2.Start();
            }
            catch (Exception exception)
            {
                Logging.WriteError("ConfigWow(): " + exception, true);
            }
        }

        private static void Lehoeja()
        {
            try
            {
                while (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                {
                    Thread.Sleep(10);
                }
                Thread.Sleep(50);
                StartStopClickToMove(true);
                if (Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xf3dec4) + 0x34) != 1)
                {
                    Logging.WriteDebug("AutoDismount_Activate_Pointer was OFF, now activated.");
                    Memory.WowMemory.Memory.WriteUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xf3dec4) + 0x34, 1);
                }
                if (Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xf3dedc) + 0x34) != 1)
                {
                    Logging.WriteDebug("AutoLoot_Activate_Pointer was OFF, now activated.");
                    Memory.WowMemory.Memory.WriteUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xf3dedc) + 0x34, 1);
                }
                if (Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xf3dee8) + 0x34) != 1)
                {
                    Logging.WriteDebug("AutoSelfCast_Activate_Pointer was OFF, now activated.");
                    Memory.WowMemory.Memory.WriteUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xf3dee8) + 0x34, 1);
                }
                Lua.LuaDoString("SetCVar(\"ScriptErrors\", \"0\")", false, true);
                if (nManagerSetting.CurrentSetting.AllowTNBToSetYourMaxFps)
                {
                    Lua.LuaDoString("ConsoleExec(\"maxfpsbk 60\")", false, true);
                    Lua.LuaDoString("ConsoleExec(\"MaxFPS 60\")", false, true);
                }
                Lua.LuaDoString("ConsoleExec(\"AllowDangerousScripts 1\")", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ConfigWowThread(): " + exception, true);
            }
        }

        public static void StartStopClickToMove(bool startStop = true)
        {
            uint num = Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xf3debc) + 0x34);
            if (startStop && (num != 1))
            {
                Logging.WriteDebug("AutoInteract_Activate_Pointer was OFF, now activated.");
                Memory.WowMemory.Memory.WriteUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xf3debc) + 0x34, 1);
            }
            else if (!startStop && (num == 1))
            {
                Logging.WriteDebug("AutoInteract_Activate_Pointer was ON, now de-activated.");
                Memory.WowMemory.Memory.WriteUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xf3debc) + 0x34, 0);
            }
        }
    }
}

