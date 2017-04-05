namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Runtime.InteropServices;

    public class TraceLine
    {
        private static Point _fromLast = new Point();
        private static bool _lastResult = true;
        private static Point _toLast = new Point();

        public static bool TraceLineGo(Point to)
        {
            try
            {
                return TraceLineGo(nManager.Wow.ObjectManager.ObjectManager.Me.Position, to, CGWorldFrameHitFlags.HitTestAll);
            }
            catch (Exception exception)
            {
                Logging.WriteError("TraceLineGo(Point to): " + exception, true);
                return true;
            }
        }

        public static bool TraceLineGo(Point from, Point to, CGWorldFrameHitFlags hitFlags = 0x10151)
        {
            try
            {
                if (((from.X != 0f) && (from.Y != 0f)) && ((to.X != 0f) && (to.Y != 0f)))
                {
                    if ((_toLast.DistanceTo(to) < 1.5f) && (_fromLast.DistanceTo(from) < 1.5f))
                    {
                        return _lastResult;
                    }
                    _toLast = to;
                    _fromLast = from;
                    uint dwAddress = Memory.WowMemory.Memory.AllocateMemory(12);
                    uint num2 = Memory.WowMemory.Memory.AllocateMemory(12);
                    uint num3 = Memory.WowMemory.Memory.AllocateMemory(12);
                    uint num4 = Memory.WowMemory.Memory.AllocateMemory(4);
                    uint num5 = Memory.WowMemory.Memory.AllocateMemory(12);
                    uint num6 = Memory.WowMemory.Memory.AllocateMemory(4);
                    if (((dwAddress <= 0) || (num2 <= 0)) || (((num3 <= 0) || (num4 <= 0)) || (num5 <= 0)))
                    {
                        return false;
                    }
                    Memory.WowMemory.Memory.WriteFloat(num5, 0f);
                    Memory.WowMemory.Memory.WriteFloat(num5 + 4, 0f);
                    Memory.WowMemory.Memory.WriteFloat(num5 + 8, 0f);
                    Memory.WowMemory.Memory.WriteFloat(num4, 0.9f);
                    Memory.WowMemory.Memory.WriteFloat(num3, 0f);
                    Memory.WowMemory.Memory.WriteFloat(num3 + 4, 0f);
                    Memory.WowMemory.Memory.WriteFloat(num3 + 8, 0f);
                    Memory.WowMemory.Memory.WriteFloat(num2, from.X);
                    Memory.WowMemory.Memory.WriteFloat(num2 + 4, from.Y);
                    Memory.WowMemory.Memory.WriteFloat(num2 + 8, from.Z + 1.5f);
                    Memory.WowMemory.Memory.WriteFloat(dwAddress, to.X);
                    Memory.WowMemory.Memory.WriteFloat(dwAddress + 4, to.Y);
                    Memory.WowMemory.Memory.WriteFloat(dwAddress + 8, to.Z + 1.5f);
                    Memory.WowMemory.Memory.WriteInt(num6, 0);
                    string[] asm = new string[] { "call " + (Memory.WowProcess.WowModule + 0x3c47), "test eax, eax", "je @out", "push " + 0, "push " + ((uint) hitFlags), "push " + num4, "push " + num3, "push " + num2, "push " + dwAddress, "call " + (Memory.WowProcess.WowModule + 0x5676d0), "mov [" + num6 + "], al", "add esp, " + 0x18, "@out:", "retn" };
                    Memory.WowMemory.InjectAndExecute(asm);
                    bool flag = Memory.WowMemory.Memory.ReadInt(num6) > 0;
                    Memory.WowMemory.Memory.FreeMemory(num6);
                    Memory.WowMemory.Memory.FreeMemory(dwAddress);
                    Memory.WowMemory.Memory.FreeMemory(num2);
                    Memory.WowMemory.Memory.FreeMemory(num3);
                    Memory.WowMemory.Memory.FreeMemory(num4);
                    Memory.WowMemory.Memory.FreeMemory(num5);
                    _lastResult = flag;
                    return flag;
                }
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("TraceLineGo(Point from, Point to, Enums.CGWorldFrameHitFlags hitFlags = Enums.CGWorldFrameHitFlags.HitTestAll): " + exception, true);
                return true;
            }
        }
    }
}

