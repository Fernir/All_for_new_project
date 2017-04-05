namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Interact
    {
        public static void InteractWith(uint baseAddress, bool stopMove = false)
        {
            try
            {
                Usefuls.UpdateLastHardwareAction();
                if (baseAddress > 0)
                {
                    WoWObject obj2 = new WoWObject(baseAddress);
                    if (obj2.IsValid && (obj2.Guid > 0))
                    {
                        uint dwAddress = Memory.WowMemory.Memory.AllocateMemory(0x10);
                        Memory.WowMemory.Memory.WriteBytes(dwAddress, obj2.Guid.ToByteArray());
                        string[] asm = new string[] { "call " + (Memory.WowProcess.WowModule + 0x8dd5a), "test eax, eax", "je @out", "push " + dwAddress, "mov ecx, eax", "call " + (Memory.WowProcess.WowModule + 0x5341d), "add esp, 4", "@out:", "retn" };
                        Memory.WowMemory.InjectAndExecute(asm);
                        Memory.WowMemory.Memory.FreeMemory(dwAddress);
                        if (stopMove)
                        {
                            MovementManager.StopMove();
                        }
                        Thread.Sleep(Usefuls.Latency);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("InteractGameObject(uint baseAddress): " + exception, true);
            }
        }

        public static void InteractWithBeta(uint baseAddress)
        {
            if (baseAddress > 0)
            {
                WoWObject obj2 = new WoWObject(baseAddress);
                if (obj2.IsValid && (obj2.Guid > 0))
                {
                    if (obj2.Type == WoWObjectType.Unit)
                    {
                        ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, obj2.Guid, 6, 0.5f);
                    }
                    else
                    {
                        InteractWith(baseAddress, false);
                    }
                }
            }
        }

        public static void Repop()
        {
            try
            {
                Lua.LuaDoString("RepopMe()", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Repop(): " + exception, true);
            }
        }

        public static void RetrieveCorpse()
        {
            try
            {
                Lua.LuaDoString("RetrieveCorpse()", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("RetrieveCorpse(): " + exception, true);
            }
        }

        public static void SpiritHealerAccept()
        {
            try
            {
                Lua.LuaDoString("AcceptXPLoss() local f = StaticPopup_Visible local s = f(\"XP_LOSS\") if s then _G[s]:Hide() end s = f(\"XP_LOSS_NO_SICKNESS\") if s then _G[s]:Hide() end", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpiritHealerAccept(): " + exception, true);
            }
        }

        public static void TeleportToSpiritHealer()
        {
            try
            {
                Lua.RunMacroText("/click GhostFrame");
            }
            catch (Exception exception)
            {
                Logging.WriteError("TeleportToSpiritHealer(): " + exception, true);
            }
        }
    }
}

