namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using System;
    using System.Collections.Generic;

    internal class Umameireovoap
    {
        private static uint Agobimakafoco(uint nudeoxaowofVasabie, uint feakaEcearaug)
        {
            try
            {
                uint num;
                uint num2 = 0x88 * feakaEcearaug;
                if (Memory.WowMemory.Memory.ReadInt(nudeoxaowofVasabie + 0x880) == -1)
                {
                    num = Memory.WowMemory.Memory.ReadUInt(nudeoxaowofVasabie + 4) + num2;
                }
                else
                {
                    num = nudeoxaowofVasabie + num2;
                }
                return num;
            }
            catch (Exception exception)
            {
                Logging.WriteError("static uint GetAura(uint auraBase, uint currentIndex)" + exception, true);
                return 0;
            }
        }

        public static bool Erefaude(uint hoeqotaqum, List<uint> homekuatasiUpeloepa)
        {
            try
            {
                return (WacauGameNoiv(hoeqotaqum, homekuatasiUpeloepa) != -1);
            }
            catch (Exception exception)
            {
                Logging.WriteError("HaveBuff(uint baseAddress, List<UInt32> buffId)" + exception, true);
                return false;
            }
        }

        public static bool Erefaude(uint leidiupa, uint homekuatasiUpeloepa)
        {
            try
            {
                List<uint> list = new List<uint> {
                    homekuatasiUpeloepa
                };
                return Erefaude(leidiupa, list);
            }
            catch (Exception exception)
            {
                Logging.WriteError("HaveBuff(uint objBase, UInt32 buffId)" + exception, true);
                return false;
            }
        }

        public static Auras.UnitAuras WacauGameNoiv(uint hoeqotaqum)
        {
            try
            {
                Auras.UnitAuras auras = new Auras.UnitAuras(hoeqotaqum);
                uint dwAddress = hoeqotaqum + 0x1190;
                int num2 = Memory.WowMemory.Memory.ReadInt(dwAddress + 0x880);
                if (num2 == -1)
                {
                    num2 = Memory.WowMemory.Memory.ReadInt(dwAddress);
                }
                for (uint i = 0; i < num2; i++)
                {
                    uint num4 = Agobimakafoco(dwAddress, i);
                    if (num4 > 0)
                    {
                        uint num5 = Memory.WowMemory.Memory.ReadUInt(num4 + 0x58);
                        Auras.UnitAura item = new Auras.UnitAura {
                            BaseAddress = num4,
                            AuraSpellId = num5
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
            return new Auras.UnitAuras(hoeqotaqum);
        }

        public static int WacauGameNoiv(uint hoeqotaqum, List<uint> homekuatasiUpeloepa)
        {
            List<Auras.UnitAura> auras = WacauGameNoiv(hoeqotaqum).Auras;
            for (int i = 0; i < auras.Count; i++)
            {
                Auras.UnitAura aura = auras[i];
                if (homekuatasiUpeloepa.Contains(aura.AuraSpellId))
                {
                    if (!aura.IsActive)
                    {
                        if (!aura.Flags.HasFlag(UnitAuraFlags.None | UnitAuraFlags.Passive) || aura.Flags.HasFlag(UnitAuraFlags.Cancelable))
                        {
                            return aura.AuraCount;
                        }
                        if (!aura.Flags.HasFlag(UnitAuraFlags.Duration) && !aura.Flags.HasFlag(UnitAuraFlags.BasePoints))
                        {
                            return -1;
                        }
                    }
                    return aura.AuraCount;
                }
            }
            return -1;
        }
    }
}

