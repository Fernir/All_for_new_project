namespace nManager.Helpful
{
    using nManager.Wow.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class Garrison
    {
        private static List<int> _garrisonMapIdList;

        public static void DumpAllOwnedBuildingsRanks()
        {
            foreach (BuildingID gid in GetBuildingOwnedList())
            {
                int buildingRank = GetBuildingRank(gid);
                Logging.Write(string.Concat(new object[] { "GBuilding = ", gid, ", rank = ", buildingRank }));
            }
        }

        public static List<BuildingID> GetBuildingOwnedList()
        {
            try
            {
                List<BuildingID> list = new List<BuildingID>();
                Lua.LuaDoString("mygb = \"\"; local buildings = C_Garrison.GetBuildings()  for i = 1, #buildings do   mygb =  mygb.. tostring(buildings[i].buildingID) .. \"|\" end ", false, true);
                foreach (string str in Lua.GetLocalizedText("mygb").Split(new char[] { '|' }))
                {
                    int num = Others.ToInt32(str);
                    if (num > 0)
                    {
                        list.Add((BuildingID) num);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("public static List<BuildingID> GetBuildingOwnedList(): " + exception, true);
            }
            return new List<BuildingID>();
        }

        public static int GetBuildingRank(BuildingID buildingId)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { "_, _, _, _, _, ", randomString, " = C_Garrison.GetBuildingInfo(", (int) buildingId, ");" }), false, true);
                return Others.ToInt32(Lua.GetLocalizedText(randomString));
            }
            catch (Exception exception)
            {
                Logging.WriteError("public int GetBuildingRank(int buildingId): " + exception, true);
            }
            return 0;
        }

        public static int GetGarrisonLevel()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = C_Garrison.GetGarrisonInfo()", false, true);
            return Others.ToInt32(Lua.GetLocalizedText(randomString));
        }

        public static List<int> GarrisonMapIdList
        {
            get
            {
                try
                {
                    if (_garrisonMapIdList == null)
                    {
                        _garrisonMapIdList = new List<int>();
                        foreach (string str in Others.ReadFileAllLines(Application.StartupPath + @"\Data\garrisonMapIdList.txt"))
                        {
                            if (!string.IsNullOrWhiteSpace(str))
                            {
                                _garrisonMapIdList.Add(Others.ToInt32(str));
                            }
                        }
                    }
                    return _garrisonMapIdList;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Usefuls.GarrisonMapIdList : " + exception, true);
                    return new List<int>();
                }
            }
        }

        public static int GetGarrisonGardenLevel
        {
            get
            {
                List<BuildingID> buildingOwnedList = GetBuildingOwnedList();
                if (buildingOwnedList.Contains(BuildingID.HerbGarden1))
                {
                    return 1;
                }
                if (buildingOwnedList.Contains(BuildingID.HerbGarden2))
                {
                    return 2;
                }
                if (buildingOwnedList.Contains(BuildingID.HerbGarden3))
                {
                    return 3;
                }
                return 0;
            }
        }

        public static int GetGarrisonMineLevel
        {
            get
            {
                List<BuildingID> buildingOwnedList = GetBuildingOwnedList();
                if (buildingOwnedList.Contains(BuildingID.LunarfallExcavationFrostwallMines1))
                {
                    return 1;
                }
                if (buildingOwnedList.Contains(BuildingID.LunarfallExcavationFrostwallMines2))
                {
                    return 2;
                }
                if (buildingOwnedList.Contains(BuildingID.LunarfallExcavationFrostwallMines3))
                {
                    return 3;
                }
                return 0;
            }
        }

        public enum BuildingID
        {
            AlchemyLab1 = 0x4c,
            AlchemyLab2 = 0x77,
            AlchemyLab3 = 120,
            Barn1 = 0x18,
            Barn2 = 0x19,
            Barn3 = 0x85,
            Barracks1 = 0x1a,
            Barracks2 = 0x1b,
            Barracks3 = 0x1c,
            DwarvenBunkerWarMill1 = 8,
            DwarvenBunkerWarMill2 = 9,
            DwarvenBunkerWarMill3 = 10,
            EnchantersStudy1 = 0x5d,
            EnchantersStudy2 = 0x7d,
            EnchantersStudy3 = 0x7e,
            EngineeringWorks1 = 0x5b,
            EngineeringWorks2 = 0x7b,
            EngineeringWorks3 = 0x7c,
            FishingShack1 = 0x40,
            FishingShack2 = 0x86,
            FishingShack3 = 0x87,
            GemBoutique1 = 0x60,
            GemBoutique2 = 0x83,
            GemBoutique3 = 0x84,
            GladiatorsSanctum1 = 0x9f,
            GladiatorsSanctum2 = 160,
            GladiatorsSanctum3 = 0xa1,
            GnomishGearworksGoblinWorkshop1 = 0xa2,
            GnomishGearworksGoblinWorkshop2 = 0xa3,
            GnomishGearworksGoblinWorkshop3 = 0xa4,
            HerbGarden1 = 0x1d,
            HerbGarden2 = 0x88,
            HerbGarden3 = 0x89,
            LumberMill1 = 40,
            LumberMill2 = 0x29,
            LumberMill3 = 0x8a,
            LunarfallExcavationFrostwallMines1 = 0x3d,
            LunarfallExcavationFrostwallMines2 = 0x3e,
            LunarfallExcavationFrostwallMines3 = 0x3f,
            LunarfallInnFrostwallTavern1 = 0x22,
            LunarfallInnFrostwallTavern2 = 0x23,
            LunarfallInnFrostwallTavern3 = 0x24,
            MageTowerSpiritLodge1 = 0x25,
            MageTowerSpiritLodge2 = 0x26,
            MageTowerSpiritLodge3 = 0x27,
            Menagerie1 = 0x2a,
            Menagerie2 = 0xa7,
            Menagerie3 = 0xa8,
            SalvageYard1 = 0x34,
            SalvageYard2 = 140,
            SalvageYard3 = 0x8d,
            ScribesQuarters1 = 0x5f,
            ScribesQuarters2 = 0x81,
            ScribesQuarters3 = 130,
            Stables1 = 0x41,
            Stables2 = 0x42,
            Stables3 = 0x43,
            Storehouse1 = 0x33,
            Storehouse2 = 0x8e,
            Storehouse3 = 0x8f,
            TailoringEmporium1 = 0x5e,
            TailoringEmporium2 = 0x7f,
            TailoringEmporium3 = 0x80,
            TheForge1 = 60,
            TheForge2 = 0x75,
            TheForge3 = 0x76,
            TheTannery1 = 90,
            TheTannery2 = 0x79,
            TheTannery3 = 0x7a,
            TradingPost1 = 0x6f,
            TradingPost2 = 0x90,
            TradingPost3 = 0x91
        }
    }
}

