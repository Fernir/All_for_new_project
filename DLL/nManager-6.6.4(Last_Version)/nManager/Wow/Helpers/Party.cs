namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class Party
    {
        public static bool CurrentPlayerIsLeader()
        {
            try
            {
                return ((nManager.Wow.ObjectManager.ObjectManager.Me.GetCurrentPartyType == PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME) ? nManager.Wow.ObjectManager.ObjectManager.Me.IsHomePartyLeader : nManager.Wow.ObjectManager.ObjectManager.Me.IsInstancePartyLeader);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Party > CurrentPlayerIsLeader(): " + exception, true);
            }
            return false;
        }

        public static UInt128 GetPartyLeaderGUID()
        {
            try
            {
                uint partyPointer = GetPartyPointer(nManager.Wow.ObjectManager.ObjectManager.Me.GetCurrentPartyType);
                if (partyPointer > 0)
                {
                    uint partyNumberPlayers = GetPartyNumberPlayers();
                    for (uint i = 0; i < partyNumberPlayers; i++)
                    {
                        uint num4 = Memory.WowMemory.Memory.ReadUInt(partyPointer + (4 * i));
                        if ((num4 > 0) && (Memory.WowMemory.Memory.ReadUInt(num4 + 4) == 2))
                        {
                            return Memory.WowMemory.Memory.ReadUInt128(num4 + 0x10);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Party > GetPartyLeaderGUID(): " + exception, true);
            }
            return 0;
        }

        public static uint GetPartyNumberPlayers()
        {
            try
            {
                uint partyPointer = GetPartyPointer(nManager.Wow.ObjectManager.ObjectManager.Me.GetCurrentPartyType);
                if (partyPointer > 0)
                {
                    uint num2 = Memory.WowMemory.Memory.ReadUInt(partyPointer + 200);
                    if ((num2 > 0) && (num2 <= 40))
                    {
                        return num2;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Party > GetPartyNumberPlayers(): " + exception, true);
            }
            return 0;
        }

        public static uint GetPartyNumberPlayersLUA(PartyEnums.PartyType partyType = 1)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { randomString, " = GetNumGroupMembers(\"", partyType, "\");" }), false, true);
                return Others.ToUInt32(Lua.GetLocalizedText(randomString));
            }
            catch (Exception exception)
            {
                Logging.WriteError("Party > GetPartyNumberPlayers(): " + exception, true);
            }
            return 0;
        }

        public static List<UInt128> GetPartyPlayersGUID()
        {
            List<UInt128> list = new List<UInt128>();
            try
            {
                uint partyPointer = GetPartyPointer(nManager.Wow.ObjectManager.ObjectManager.Me.GetCurrentPartyType);
                if (partyPointer <= 0)
                {
                    return list;
                }
                for (uint i = 0; i < 40; i++)
                {
                    uint num3 = Memory.WowMemory.Memory.ReadUInt(partyPointer + (4 * i));
                    if (num3 > 0)
                    {
                        UInt128 item = Memory.WowMemory.Memory.ReadUInt128(num3 + 0x10);
                        if (item > 0)
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Party > GetPartyGUID(): " + exception, true);
                list = new List<UInt128>();
            }
            return list;
        }

        public static uint GetPartyPointer(PartyEnums.PartyType partyType = 1)
        {
            try
            {
                if (partyType == PartyEnums.PartyType.None)
                {
                    return 0;
                }
                return Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + ((uint) (0xf9c84c + ((partyType - 1) * ((PartyEnums.PartyType) 4)))));
            }
            catch (Exception exception)
            {
                Logging.WriteError("Party > GetPartyPointer(): " + exception, true);
            }
            return 0;
        }

        public static bool IsInGroup()
        {
            try
            {
                return (nManager.Wow.ObjectManager.ObjectManager.Me.GetCurrentPartyType != PartyEnums.PartyType.None);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Party > IsInGroup(): " + exception, true);
            }
            return false;
        }

        public static bool IsInGroupLUA(PartyEnums.PartyType partyType = 1)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                string str2 = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { str2, " = IsInGroup(\"", partyType, "\"); if ", str2, " then ", randomString, " = \"1\" else ", randomString, " = \"0\" end" }), false, true);
                return (Lua.GetLocalizedText(randomString) == "1");
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSpellInfoLUA(string spellNameInGame): " + exception, true);
            }
            return false;
        }
    }
}

