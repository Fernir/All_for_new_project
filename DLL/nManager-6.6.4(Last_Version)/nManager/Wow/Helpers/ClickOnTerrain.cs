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
                Foamei(point);
            }
        }

        private static void Foamei(Point owetuacuciunEle)
        {
            try
            {
                if ((owetuacuciunEle != null) && owetuacuciunEle.IsValid)
                {
                    uint dwAddress = Memory.WowMemory.Memory.AllocateMemory(Marshal.SizeOf(typeof(StructClickOnTerrain)));
                    StructClickOnTerrain terrain = new StructClickOnTerrain {
                        X = owetuacuciunEle.X,
                        Y = owetuacuciunEle.Y,
                        Z = owetuacuciunEle.Z
                    };
                    Memory.WowMemory.Memory.WriteObject(dwAddress, terrain, typeof(StructClickOnTerrain));
                    string[] asm = new string[] { "call " + (Memory.WowProcess.WowModule + 0x8dd5a), "test eax, eax", "je @out", "push " + dwAddress, "mov ebx, " + (Memory.WowProcess.WowModule + 0x2dbe18), "call ebx", "add esp, 0x4", "@out:", "retn" };
                    Memory.WowMemory.InjectAndExecute(asm);
                    Memory.WowMemory.Memory.FreeMemory(dwAddress);
                }
            }
            catch
            {
            }
        }

        public static void Item(int Entry, Point point)
        {
            if (((Entry > 0) && (point != null)) && point.IsValid)
            {
                ItemsManager.UseItem(ItemsManager.GetItemNameById(Entry));
                Thread.Sleep((int) (Usefuls.Latency * 2));
                Foamei(point);
            }
        }

        public static void Spell(uint spellId, Point point)
        {
            if (((spellId > 0) && (point != null)) && ((point.X != 0f) || (point.Y != 0f)))
            {
                new nManager.Wow.Class.Spell(spellId).Launch();
                Thread.Sleep(Usefuls.Latency);
                Foamei(point);
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

