namespace nManager.Wow.Helpers
{
    using nManager.Wow;
    using nManager.Wow.Class;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class ClickOnTerrain
    {
        public static void ClickOnly(Point point)
        {
            if ((point != null) && point.IsValid)
            {
                Thread.Sleep((int) (Usefuls.Latency + 50));
                Pulse(point);
            }
        }

        public static void Item(int Entry, Point point)
        {
            if (((Entry > 0) && (point != null)) && point.IsValid)
            {
                ItemsManager.UseItem(ItemsManager.GetItemNameById(Entry));
                Thread.Sleep((int) (Usefuls.Latency * 2));
                Pulse(point);
            }
        }

        private static void Pulse(Point point)
        {
            try
            {
                if ((point != null) && point.IsValid)
                {
                    uint dwAddress = Memory.WowMemory.Memory.AllocateMemory(Marshal.SizeOf(typeof(StructClickOnTerrain)));
                    StructClickOnTerrain terrain = new StructClickOnTerrain {
                        X = point.X,
                        Y = point.Y,
                        Z = point.Z
                    };
                    Memory.WowMemory.Memory.WriteObject(dwAddress, terrain, typeof(StructClickOnTerrain));
                    string[] asm = new string[] { "call " + (Memory.WowProcess.WowModule + 0x3c47), "test eax, eax", "je @out", "push " + dwAddress, "mov ebx, " + (Memory.WowProcess.WowModule + 0x2859b2), "call ebx", "add esp, 0x4", "@out:", "retn" };
                    Memory.WowMemory.InjectAndExecute(asm);
                    Memory.WowMemory.Memory.FreeMemory(dwAddress);
                }
            }
            catch
            {
            }
        }

        public static void Spell(uint spellId, Point point)
        {
            if (((spellId > 0) && (point != null)) && ((point.X != 0f) || (point.Y != 0f)))
            {
                new nManager.Wow.Class.Spell(spellId).Launch();
                Thread.Sleep((int) (Usefuls.Latency * 2));
                Pulse(point);
            }
        }

        [StructLayout(LayoutKind.Explicit, Size=0x20)]
        public struct StructClickOnTerrain
        {
            [FieldOffset(0)]
            public UInt128 Guid;
            [FieldOffset(0x10)]
            public float X;
            [FieldOffset(20)]
            public float Y;
            [FieldOffset(0x18)]
            public float Z;
        }
    }
}

