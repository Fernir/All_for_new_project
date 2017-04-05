namespace nManager.Wow.Helpers
{
    using nManager.Wow;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Battleground
    {
        private static readonly List<uint> PreparationId = new List<uint>();

        public static void AcceptBattlefieldPort(int index, bool accept)
        {
            Lua.LuaDoString(string.Concat(new object[] { "AcceptBattlefieldPort(", index, ",", accept ? 1 : 0, ")" }), false, true);
        }

        public static void AcceptBattlefieldPortAll()
        {
            uint num = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe03a2c);
            for (int i = 1; i <= num; i++)
            {
                AcceptBattlefieldPort(i, true);
                Thread.Sleep(500);
            }
        }

        public static bool BattlegroundIsStarted()
        {
            try
            {
                if (PreparationId.Count <= 0)
                {
                    PreparationId.Add(0xade9);
                    PreparationId.Add(0x7fd8);
                    PreparationId.Add(0x7fd7);
                }
                return !nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(PreparationId);
            }
            catch
            {
            }
            return false;
        }

        public static void ExitBattleground()
        {
            Lua.LuaDoString("LeaveBattlefield()", false, true);
        }

        public static BattlegroundId GetCurrentBattleground()
        {
            switch (((ContinentId) Usefuls.ContinentId))
            {
                case ContinentId.PVPZone04:
                    return BattlegroundId.ArathiBasin;

                case ContinentId.NetherstormBG:
                    return BattlegroundId.EyeoftheStorm;

                case ContinentId.NorthrendBG:
                    return BattlegroundId.StrandoftheAncients;

                case ContinentId.PVPZone01:
                    return BattlegroundId.AlteracValley;

                case ContinentId.PVPZone03:
                    return BattlegroundId.WarsongGulch;

                case ContinentId.CataclysmCTF:
                    return BattlegroundId.TwinPeaks;

                case ContinentId.STV_Mine_BG:
                    return BattlegroundId.SilvershardMines;

                case ContinentId.IsleofConquest:
                    return BattlegroundId.IsleofConquest;

                case ContinentId.Gilneas_BG_2:
                    return BattlegroundId.BattleforGilneas;

                case ContinentId.ValleyOfPower:
                    return BattlegroundId.TempleofKotmogu;

                case ContinentId.GoldRushBG:
                    return BattlegroundId.DeepwindGorge;
            }
            return BattlegroundId.None;
        }

        public static string GetCurrentBattlegroundNameLocalized()
        {
            WoWMap map = WoWMap.FromId(Usefuls.ContinentId);
            if (map.Record.InstanceType == WoWMap.InstanceType.Battleground)
            {
                return map.MapName;
            }
            return string.Empty;
        }

        public static bool IsFinishBattleground()
        {
            return (Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe03a60) > 0);
        }

        public static bool IsInBattleground()
        {
            return (GetCurrentBattlegroundNameLocalized() != string.Empty);
        }

        public static void JoinBattlefield(BattlegroundId type, bool asGroup = false)
        {
            if (type != BattlegroundId.None)
            {
                Lua.LuaDoString("for i = 1, GetNumBattlegroundTypes() do local _,_,_,_,id = GetBattlegroundInfo(i); if id == {0} then RequestBattlegroundInstanceInfo(i); end end", false, true);
                Lua.LuaDoString(string.Format("JoinBattlefield(1, {0})", asGroup ? "true" : "false"), false, true);
                Thread.Sleep(500);
            }
        }

        public static void JoinBattlegroundQueue(BattlegroundId id)
        {
            SetPVPRoles();
            Lua.LuaDoString("JoinBattlefield(" + ((uint) id) + ");", false, true);
        }

        public static int QueueingStatus()
        {
            uint num = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xbc0688);
            int num2 = Memory.WowMemory.Memory.ReadByte(Memory.WowProcess.WowModule + 0xbc0688) & 1;
            if ((num != 0) && (num2 <= 0))
            {
                return 1;
            }
            return 0;
        }

        public static void SetPVPRoles()
        {
            Lua.LuaDoString("SetPVPRoles(0, 0, 1);", false, true);
        }

        public string NonLocalizedName
        {
            get
            {
                switch (((ContinentId) Usefuls.ContinentId))
                {
                    case ContinentId.PVPZone04:
                        return "Arathi Basin";

                    case ContinentId.NetherstormBG:
                        return "Eye of the Storm";

                    case ContinentId.PVPZone01:
                        return "Alterac Valley";

                    case ContinentId.PVPZone03:
                        return "Warsong Gulch";

                    case ContinentId.NorthrendBG:
                        return "Strand of the Ancients";

                    case ContinentId.IsleofConquest:
                        return "Isle of Conquest";

                    case ContinentId.CataclysmCTF:
                        return "Twin Peaks";

                    case ContinentId.STV_Mine_BG:
                        return "Silvershard Mines";

                    case ContinentId.Gilneas_BG_2:
                        return "Battle For Gilneas";

                    case ContinentId.ValleyOfPower:
                        return "Temple of Kotmogu";
                }
                return "";
            }
        }
    }
}

