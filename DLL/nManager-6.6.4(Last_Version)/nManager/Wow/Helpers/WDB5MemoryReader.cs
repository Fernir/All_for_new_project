namespace nManager.Wow.Helpers
{
    using nManager.Wow;
    using System;

    public class WDB5MemoryReader
    {
        private static bool Afialaob(uint owemoenimufulaOfajuximu, int igiofe)
        {
            return ((igiofe > 0) && (igiofe != Memory.WowMemory.Memory.ReadByte(owemoenimufulaOfajuximu + 220)));
        }

        private static int Akivewijeroi(uint udiuvalox, int wiogeigLagu)
        {
            if (udiuvalox <= 0)
            {
                return 0;
            }
            int num = Memory.WowMemory.Memory.ReadInt(udiuvalox) + (8 * (Memory.WowMemory.Memory.ReadInt(udiuvalox) + 1));
            int num2 = Ojauwio(Memory.WowMemory.Memory.ReadInt(udiuvalox), num, wiogeigLagu);
            if ((num2 == num) || (Memory.WowMemory.Memory.ReadInt((uint) num2) != wiogeigLagu))
            {
                return 0;
            }
            return Memory.WowMemory.Memory.ReadInt((uint) (num2 + 4));
        }

        private static int Ojauwio(int tavoanuAcuke, int wiogeigLagu, int aluqaromerelea)
        {
            int num = tavoanuAcuke;
            int num2 = wiogeigLagu;
            int num3 = 0;
            while (num != num2)
            {
                if (num3 > 0x186a0)
                {
                    return num;
                }
                num3++;
                int num4 = num + ((8 * ((num2 - num) >> 3)) / 2);
                if (Memory.WowMemory.Memory.ReadInt((uint) num4) >= aluqaromerelea)
                {
                    num2 = num + ((8 * ((num2 - num) >> 3)) / 2);
                }
                else
                {
                    num = num4 + 8;
                }
            }
            return num;
        }

        private static int Ovajiwaobio(int tavoanuAcuke, int wiogeigLagu, ref short aluqaromerelea)
        {
            int num = tavoanuAcuke;
            int num2 = wiogeigLagu;
            int num3 = 0;
            while (num != num2)
            {
                if (num3 > 0x186a0)
                {
                    return num;
                }
                num3++;
                int num4 = num + ((4 * ((num2 - num) >> 2)) / 2);
                if (Memory.WowMemory.Memory.ReadShort((uint) num4) >= aluqaromerelea)
                {
                    num2 = num + ((4 * ((num2 - num) >> 2)) / 2);
                }
                else
                {
                    num = num4 + 4;
                }
            }
            return num;
        }

        private static int Purexuxus(uint udiuvalox, int wiogeigLagu)
        {
            uint num2 = udiuvalox;
            int num3 = 0;
            short num4 = 0;
            bool flag = false;
            if ((wiogeigLagu < Memory.WowMemory.Memory.ReadInt(num2 + 0x18)) || (wiogeigLagu > Memory.WowMemory.Memory.ReadInt(num2 + 0x1c)))
            {
                flag = true;
            }
            else
            {
                uint num5 = Memory.WowMemory.Memory.ReadUInt(num2 + 0x4c);
                int num6 = wiogeigLagu - Memory.WowMemory.Memory.ReadShort(num2 + 0x18);
                short aluqaromerelea = (short) (wiogeigLagu - Memory.WowMemory.Memory.ReadShort(num2 + 0x18));
                num3 = Ovajiwaobio(Memory.WowMemory.Memory.ReadInt(num2 + 0x48), Memory.WowMemory.Memory.ReadInt(num2 + 0x48) + ((int) (4 * num5)), ref aluqaromerelea);
                if ((num3 == (Memory.WowMemory.Memory.ReadInt(num2 + 0x48) + (4 * Memory.WowMemory.Memory.ReadInt(num2 + 0x4c)))) || (Memory.WowMemory.Memory.ReadShort((uint) num3) != num6))
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                num4 = Memory.WowMemory.Memory.ReadShort((uint) (num3 + 2));
                if (num4 == -2)
                {
                    return 0;
                }
            }
            if ((num4 == -1) || flag)
            {
                return Akivewijeroi(Memory.WowMemory.Memory.ReadUInt(udiuvalox + 0x54), wiogeigLagu);
            }
            short num8 = Memory.WowMemory.Memory.ReadShort(num2 + 0x20);
            if (num4 >= num8)
            {
                short num9 = (short) (num4 - num8);
                if (((ushort) num9) >= Memory.WowMemory.Memory.ReadInt(num2 + 0x2c))
                {
                    return 0;
                }
                return (Memory.WowMemory.Memory.ReadInt(num2 + 8) + (num9 * Memory.WowMemory.Memory.ReadInt(num2 + 20)));
            }
            return (Memory.WowMemory.Memory.ReadInt(num2 + 4) + (Memory.WowMemory.Memory.ReadInt(num2 + 0x10) * num4));
        }

        public static uint WowClientDB2__GetRowPointer(uint db2Offset, int reqId)
        {
            uint dwAddress = Memory.WowProcess.WowModule + db2Offset;
            if (((reqId >= 0) || ((Memory.WowMemory.Memory.ReadByte(Memory.WowMemory.Memory.ReadUInt(dwAddress + 0x98) + 0x58) == 1) && (reqId != -1))) && ((reqId >= Memory.WowMemory.Memory.ReadUInt(dwAddress + 200)) && (reqId <= Memory.WowMemory.Memory.ReadUInt(dwAddress + 0xc4))))
            {
                int igiofe = Purexuxus(Memory.WowMemory.Memory.ReadUInt(dwAddress + 0xa8), reqId);
                if ((igiofe > 0) && Afialaob(Memory.WowMemory.Memory.ReadUInt(dwAddress), igiofe))
                {
                    return (uint) igiofe;
                }
            }
            return 0;
        }
    }
}

