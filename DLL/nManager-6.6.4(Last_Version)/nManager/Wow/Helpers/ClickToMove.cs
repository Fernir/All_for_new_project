namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Runtime.InteropServices;

    public class ClickToMove
    {
        private static Vector3 _baveubIkovu = new Vector3();

        public static void CGPlayer_C__ClickToMove(float x, float y, float z, UInt128 guid, int action, float precision)
        {
            try
            {
                Usefuls.UpdateLastHardwareAction();
                if (((x != 0f) || (y != 0f)) || ((z != 0f) || (guid != 0)))
                {
                    uint dwAddress = Memory.WowMemory.Memory.AllocateMemory(12);
                    uint num2 = Memory.WowMemory.Memory.AllocateMemory(0x20);
                    uint num3 = Memory.WowMemory.Memory.AllocateMemory(4);
                    if (((dwAddress > 0) && (num2 > 0)) && (num3 > 0))
                    {
                        Memory.WowMemory.Memory.WriteInt128(num2, guid);
                        Memory.WowMemory.Memory.WriteInt128(num2 + ((uint) Marshal.SizeOf(guid)), nManager.Wow.ObjectManager.ObjectManager.Me.TransportGuid);
                        Memory.WowMemory.Memory.WriteFloat(num3, precision);
                        Memory.WowMemory.Memory.WriteFloat(dwAddress, x);
                        Memory.WowMemory.Memory.WriteFloat(dwAddress + 4, y);
                        Memory.WowMemory.Memory.WriteFloat(dwAddress + 8, z);
                        string[] asm = new string[] { "call " + (Memory.WowProcess.WowModule + 0x8dd5a), "test eax, eax", "je @out", "mov edx, [" + num3 + "]", "push edx", "push " + dwAddress, "push " + num2, "push " + action, "mov ecx, eax", "call " + (Memory.WowProcess.WowModule + 0x32fe34), "@out:", "retn" };
                        Memory.WowMemory.InjectAndExecute(asm);
                        Memory.WowMemory.Memory.FreeMemory(dwAddress);
                        Memory.WowMemory.Memory.FreeMemory(num2);
                        Memory.WowMemory.Memory.FreeMemory(num3);
                        if (_baveubIkovu != new Vector3(x, y, z))
                        {
                            Logging.WriteNavigator(string.Concat(new object[] { "MoveTo(", x, ", ", y, ", ", z, ", ", guid, ", ", action, ", ", precision, ")" }));
                            _baveubIkovu = new Vector3(x, y, z);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("CGPlayer_C__ClickToMove(Single x, Single y, Single z, UInt64 guid, Int32 action, Single precision): " + exception, true);
            }
        }

        public static Point GetClickToMovePosition()
        {
            try
            {
                return new Point(Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + 0xec43f0), Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + 0xec43f4), Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + 0xec43f8), "None");
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetClickToMovePosition(): " + exception, true);
                return new Point(0f, 0f, 0f, "None");
            }
        }

        public static ClickToMoveType GetClickToMoveTypePush()
        {
            try
            {
                return (ClickToMoveType) Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xec43c8);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetClickToMoveTypePush(): " + exception, true);
                return ClickToMoveType.None;
            }
        }
    }
}

