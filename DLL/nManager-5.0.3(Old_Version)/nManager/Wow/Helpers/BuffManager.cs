namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using System;
    using System.Collections.Generic;

    internal class BuffManager
    {
        public static Auras.UnitAuras AuraStack(uint baseAddress)
        {
            try
            {
                Auras.UnitAuras auras = new Auras.UnitAuras(baseAddress);
                uint dwAddress = baseAddress + 0x1108;
                int num2 = Memory.WowMemory.Memory.ReadInt(dwAddress + 0x480);
                if (num2 == -1)
                {
                    num2 = Memory.WowMemory.Memory.ReadInt(dwAddress);
                }
                for (uint i = 0; i < num2; i++)
                {
                    uint auraPtrByIndex = GetAuraPtrByIndex(dwAddress, i);
                    if (auraPtrByIndex > 0)
                    {
                        UInt128 num5 = Memory.WowMemory.Memory.ReadUInt128(auraPtrByIndex + 0x20);
                        uint num6 = Memory.WowMemory.Memory.ReadUInt(auraPtrByIndex + 0x30);
                        byte num7 = Memory.WowMemory.Memory.ReadByte(auraPtrByIndex + 0x34);
                        byte num8 = Memory.WowMemory.Memory.ReadByte(auraPtrByIndex + 0x39);
                        byte num9 = Memory.WowMemory.Memory.ReadByte(auraPtrByIndex + 0x3a);
                        byte num10 = Memory.WowMemory.Memory.ReadByte(auraPtrByIndex + 0x3b);
                        int num11 = Memory.WowMemory.Memory.ReadInt(auraPtrByIndex + 60);
                        int num12 = Memory.WowMemory.Memory.ReadInt(auraPtrByIndex + 0x40);
                        uint num13 = Memory.WowMemory.Memory.ReadUInt(auraPtrByIndex + 0x44);
                        uint num14 = Memory.WowMemory.Memory.ReadUInt(auraPtrByIndex + 0x35);
                        Auras.UnitAura item = new Auras.UnitAura {
                            AuraCreatorGUID = num5,
                            AuraSpellId = num6,
                            AuraFlag = num7,
                            AuraCount = num8,
                            AuraCasterLevel = num9,
                            AuraMask = num14,
                            AuraUnk1 = num10,
                            AuraDuration = num11,
                            AuraSpellEndTime = num12,
                            AuraUnk2 = num13
                        };
                        auras.Auras.Add(item);
                    }
                }
                return auras;
            }
            catch (Exception exception)
            {
                Logging.WriteError("AuraStack(uint baseAddress, List<UInt32> buffId)" + exception, true);
            }
            return new Auras.UnitAuras(baseAddress);
        }

        public static int AuraStack(uint baseAddress, List<uint> buffId)
        {
            foreach (Auras.UnitAura aura in AuraStack(baseAddress).Auras)
            {
                if (buffId.Contains(aura.AuraSpellId))
                {
                    if (aura.IsActive)
                    {
                        return aura.AuraCount;
                    }
                    if (!aura.Flags.HasFlag(UnitAuraFlags.None | UnitAuraFlags.Passive) || aura.Flags.HasFlag(UnitAuraFlags.Cancelable))
                    {
                        return aura.AuraCount;
                    }
                    if (aura.Flags.HasFlag(UnitAuraFlags.Duration) || aura.Flags.HasFlag(UnitAuraFlags.BasePoints))
                    {
                        return aura.AuraCount;
                    }
                    return -1;
                }
            }
            return -1;
        }

        private static uint GetAuraPtrByIndex(uint auraBase, uint currentIndex)
        {
            try
            {
                uint num;
                uint num2 = 0x48 * currentIndex;
                if (Memory.WowMemory.Memory.ReadInt(auraBase + 0x480) == -1)
                {
                    num = Memory.WowMemory.Memory.ReadUInt(auraBase + 4) + num2;
                }
                else
                {
                    num = auraBase + num2;
                }
                return num;
            }
            catch (Exception exception)
            {
                Logging.WriteError("static uint GetAura(uint auraBase, uint currentIndex)" + exception, true);
                return 0;
            }
        }

        public static bool HaveBuff(uint baseAddress, List<uint> buffId)
        {
            try
            {
                return (AuraStack(baseAddress, buffId) != -1);
            }
            catch (Exception exception)
            {
                Logging.WriteError("HaveBuff(uint baseAddress, List<UInt32> buffId)" + exception, true);
                return false;
            }
        }

        public static bool HaveBuff(uint objBase, uint buffId)
        {
            try
            {
                List<uint> list = new List<uint> {
                    buffId
                };
                return HaveBuff(objBase, list);
            }
            catch (Exception exception)
            {
                Logging.WriteError("HaveBuff(uint objBase, UInt32 buffId)" + exception, true);
                return false;
            }
        }
    }
}

