﻿namespace nManager.Wow.Helpers
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
        private static bool _ereovokacaesIpiruo = true;
        private static Point _mewuogosairum = new Point();
        private static Point _xakacioxemoUliu = new Point();

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
                if (Usefuls.ContinentId == 0x1e240)
                {
                    WoWObject objectByGuid = nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(nManager.Wow.ObjectManager.ObjectManager.Me.TransportGuid);
                    if (objectByGuid is WoWGameObject)
                    {
                        WoWGameObject o = objectByGuid as WoWGameObject;
                        if (from.DistanceTo(new Point()) < 100f)
                        {
                            from = new Point(from.TransformInvert(o));
                        }
                        if (to.DistanceTo(new Point()) < 100f)
                        {
                            to = new Point(to.TransformInvert(o));
                        }
                    }
                }
                if (((from.X != 0f) && (from.Y != 0f)) && ((to.X != 0f) && (to.Y != 0f)))
                {
                    if ((_mewuogosairum.DistanceTo(to) < 1.5f) && (_xakacioxemoUliu.DistanceTo(from) < 1.5f))
                    {
                        return _ereovokacaesIpiruo;
                    }
                    _mewuogosairum = to;
                    _xakacioxemoUliu = from;
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
                    string[] asm = new string[] { "call " + (Memory.WowProcess.WowModule + 0x8dd5a), "test eax, eax", "je @out", "push " + 0, "push " + ((uint) hitFlags), "push " + num4, "push " + num3, "push " + num2, "push " + dwAddress, "call " + (Memory.WowProcess.WowModule + 0x6434bf), "mov [" + num6 + "], al", "add esp, " + 0x18, "@out:", "retn" };
                    Memory.WowMemory.InjectAndExecute(asm);
                    bool flag = Memory.WowMemory.Memory.ReadInt(num6) > 0;
                    Memory.WowMemory.Memory.FreeMemory(num6);
                    Memory.WowMemory.Memory.FreeMemory(dwAddress);
                    Memory.WowMemory.Memory.FreeMemory(num2);
                    Memory.WowMemory.Memory.FreeMemory(num3);
                    Memory.WowMemory.Memory.FreeMemory(num4);
                    Memory.WowMemory.Memory.FreeMemory(num5);
                    _ereovokacaesIpiruo = flag;
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

