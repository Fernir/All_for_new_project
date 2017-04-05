namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using System;

    public class CGUnit_C__GetFacing
    {
        public static float GetFacing(uint baseAddress)
        {
            try
            {
                if (baseAddress > 0)
                {
                    uint num = Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(baseAddress) + 0xb0);
                    if (num <= 0)
                    {
                        return 0f;
                    }
                    uint dwAddress = Memory.WowMemory.Memory.AllocateMemory(5 + Others.Random(1, 0x19));
                    if (dwAddress <= 0)
                    {
                        return 0f;
                    }
                    Memory.WowMemory.Memory.WriteFloat(dwAddress, 0f);
                    string[] asm = new string[] { "call " + (Memory.WowProcess.WowModule + 0x3c47), "test eax, eax", "je @out", "mov ecx, " + baseAddress, "call " + num, "mov eax, " + dwAddress, "fstp dword [eax]", "@out:", "retn" };
                    Memory.WowMemory.InjectAndExecute(asm);
                    float num3 = Memory.WowMemory.Memory.ReadFloat(dwAddress);
                    Memory.WowMemory.Memory.FreeMemory(dwAddress);
                    if (num3 < 0f)
                    {
                        num3 += 6.283185f;
                    }
                    if (num3 > 6.283185f)
                    {
                        num3 = 0f;
                    }
                    if (num3 < 0f)
                    {
                        num3 = 0f;
                    }
                    return num3;
                }
            }
            catch
            {
            }
            return 0f;
        }
    }
}

