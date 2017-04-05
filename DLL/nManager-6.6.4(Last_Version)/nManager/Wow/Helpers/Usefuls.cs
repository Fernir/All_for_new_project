namespace nManager.Wow.Helpers
{
    using Microsoft.Win32;
    using nManager;
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    public class Usefuls
    {
        private static bool _eleboupijureimPohejilub;
        private static nManager.Helpful.Timer _haifuimoebieroEcelu = new nManager.Helpful.Timer(0.0);
        private static int _icouhe;
        private static int _jubaisoi;
        private static nManager.Helpful.Timer _koceilim = new nManager.Helpful.Timer(0.0);
        private static int _ojalapeo;
        private static nManager.Helpful.Timer _ojoejouq = new nManager.Helpful.Timer(0.0);
        private static int _riwohero;
        private static bool _uhavuhi = true;
        private static readonly List<int> AchievementsDoneCache = new List<int>();
        private static readonly List<int> AchievementsNotDoneCache = new List<int>();
        public static string AfkKeyPress;
        private static readonly nManager.Helpful.Timer AfkTimer = new nManager.Helpful.Timer(500.0);
        public static List<uint> CachedBattlegroundList;
        public static List<uint> CachedInstanceList;
        public static List<uint> CachedRaidList;
        private static readonly object ThisLock = new object();
        private static readonly nManager.Helpful.Timer TimerHonorPoint = new nManager.Helpful.Timer(1000.0);

        public static void CloseAllBags()
        {
            try
            {
                Lua.LuaDoString("CloseAllBags();", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CloseAllBags(): " + exception, true);
            }
        }

        public static int ContainerNumFreeSlots(BagType bagType)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            int num10 = 0;
            int num11 = 0;
            int num12 = 0;
            int num13 = 0;
            int num14 = 0;
            int num15 = 0;
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            string str2 = Others.GetRandomString(Others.Random(4, 10));
            string str3 = Others.GetRandomString(Others.Random(4, 10));
            string str4 = Others.GetRandomString(Others.Random(4, 10));
            string str5 = Others.GetRandomString(Others.Random(4, 10));
            string str6 = Others.GetRandomString(Others.Random(4, 10));
            string str7 = Others.GetRandomString(Others.Random(4, 10));
            string str8 = Others.GetRandomString(Others.Random(4, 10));
            string str9 = Others.GetRandomString(Others.Random(4, 10));
            string str10 = Others.GetRandomString(Others.Random(4, 10));
            string commandline = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + "," + str2 + " = GetContainerNumFreeSlots(0); " + str3 + "," + str4 + " = GetContainerNumFreeSlots(1); " + str5 + "," + str6 + " = GetContainerNumFreeSlots(2); " + str7 + "," + str8 + " = GetContainerNumFreeSlots(3); " + str9 + "," + str10 + " = GetContainerNumFreeSlots(4); if(" + str4 + " == nil) then " + str4 + " = 16777216 end if(" + str6 + " == nil) then " + str6 + " = 16777216 end if(" + str8 + " == nil) then " + str8 + " = 16777216 end if(" + str10 + " == nil) then " + str10 + " = 16777216 end " + commandline + " = " + randomString + " .. \",\" .. " + str2 + " .. \";\" .. " + str3 + " .. \",\" .. " + str4 + " .. \";\" .. " + str5 + " .. \",\" .. " + str6 + " .. \";\" .. " + str7 + " .. \",\" .. " + str8 + " .. \";\" .. " + str9 + " .. \",\" .. " + str10, false, true);
            string localizedText = Lua.GetLocalizedText(commandline);
            if (!string.IsNullOrEmpty(localizedText) && localizedText.Contains(";"))
            {
                foreach (string[] strArray2 in (from s in localizedText.Split(new char[] { ';' }) select s.Split(new char[] { ',' })).ToArray<string[]>())
                {
                    int num16;
                    if ((strArray2.Count<string>() > 1) && (strArray2[0] != "0"))
                    {
                        num16 = Others.ToInt32(strArray2[0]);
                        BagType type = (BagType) Others.ToInt32(strArray2[1]);
                        if (type == BagType.Unspecified)
                        {
                            num += num16;
                        }
                        else if (!type.HasFlag(BagType.None) && type.HasFlag(bagType))
                        {
                            switch (bagType)
                            {
                                case BagType.HerbBag:
                                    num7 += num16;
                                    break;

                                case BagType.EnchantingBag:
                                    num8 += num16;
                                    break;

                                case BagType.EngineeringBag:
                                    num9 += num16;
                                    break;

                                case BagType.Quiver:
                                    num2 += num16;
                                    break;

                                case BagType.AmmoPouch:
                                    num3 += num16;
                                    break;

                                case BagType.SoulBag:
                                    num4 += num16;
                                    break;

                                case BagType.LeatherworkingBag:
                                    goto Label_0467;

                                case BagType.InscriptionBag:
                                    goto Label_0482;

                                case BagType.Keyring:
                                    goto Label_04B4;

                                case BagType.GemBag:
                                    goto Label_0470;

                                case BagType.MiningBag:
                                    goto Label_0455;

                                case BagType.Unknown:
                                    num13 += num16;
                                    break;

                                case BagType.VanityPets:
                                    num14 += num16;
                                    break;

                                case BagType.LureBag:
                                    num15 += num16;
                                    break;
                            }
                        }
                    }
                    continue;
                Label_0455:
                    num12 += num16;
                    continue;
                Label_0467:
                    num5 += num16;
                    continue;
                Label_0470:
                    num11 += num16;
                    continue;
                Label_0482:
                    num6 += num16;
                    continue;
                Label_04B4:
                    num10 += num16;
                }
                switch (bagType)
                {
                    case BagType.HerbBag:
                        return (num7 + num);

                    case BagType.EnchantingBag:
                        return (num8 + num);

                    case BagType.EngineeringBag:
                        return (num9 + num);

                    case BagType.Unspecified:
                        return num;

                    case BagType.Quiver:
                        return (num2 + num);

                    case BagType.AmmoPouch:
                        return (num3 + num);

                    case BagType.SoulBag:
                        return (num4 + num);

                    case BagType.LeatherworkingBag:
                        return (num5 + num);

                    case BagType.InscriptionBag:
                        return (num6 + num);

                    case BagType.Keyring:
                        return (num10 + num);

                    case BagType.GemBag:
                        return (num11 + num);

                    case BagType.MiningBag:
                        return (num12 + num);

                    case BagType.Unknown:
                        return (num13 + num);

                    case BagType.VanityPets:
                        return (num14 + num);

                    case BagType.LureBag:
                        return (num15 + num);
                }
            }
            return 0;
        }

        public static int ContinentIdByContinentName(string name)
        {
            switch (name)
            {
                case "Outland":
                    return 530;

                case "Maelstrom":
                    return 0x286;

                case "Pandaria":
                    return 870;

                case "AllianceGunship":
                    return 0x1e240;
            }
            return (int) WoWMap.FromMPQName(name).Record.Id;
        }

        public static string ContinentNameByContinentId(int cId)
        {
            switch (cId)
            {
                case 530:
                    return "Outland";

                case 0x286:
                    return "Maelstrom";

                case 870:
                    return "Pandaria";

                case 0x1e240:
                    return "AllianceGunship";
            }
            return WoWMap.FromId(cId).MapMPQName;
        }

        public static string ContinentNameMpqByContinentId(int cId)
        {
            if (cId == 0x1e240)
            {
                return "AllianceGunship";
            }
            return WoWMap.FromId(cId).MapMPQName;
        }

        public static void DisableFIPS()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Lsa\FipsAlgorithmPolicy", true))
            {
                if ((key != null) && (((int) key.GetValue("Enabled", null, RegistryValueOptions.None)) != 0))
                {
                    Logging.Write("We had to disable FIPS for you, else you wouldn't be able to run the bot properly, learn more about FIPS here: https://en.wikipedia.org/wiki/FIPS_140-2");
                    key.SetValue("Enabled", 0, RegistryValueKind.DWord);
                }
            }
        }

        public static void DisMount()
        {
            try
            {
                MountTask.DismountTimer.Reset();
                Lua.RunMacroText(nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.DruidMountId()) ? "/cancelform" : "/dismount");
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisMount(): " + exception, true);
            }
        }

        public static void EjectVehicle()
        {
            try
            {
                Lua.LuaDoString("VehicleExit();", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("EjectVehicle(): " + exception, true);
            }
        }

        public static List<WoWMap.MapDbcRecord> GetBattlegroundsList()
        {
            List<WoWMap.MapDbcRecord> list = WoWMap.WoWMaps(WoWMap.InstanceType.Battleground, WoWMap.MapType.ADTType);
            list.AddRange(WoWMap.WoWMaps(WoWMap.InstanceType.Battleground, WoWMap.MapType.WDTOnlyType));
            return list;
        }

        public static string GetClientLanguage()
        {
            string randomString = Others.GetRandomString(Others.Random(5, 10));
            return Lua.LuaDoString(randomString + " =  GetLocale();", randomString, false);
        }

        public static WoWCurrency GetCurrencyInfo(int entry)
        {
            string randomString = Others.GetRandomString(Others.Random(5, 10));
            string str2 = Lua.LuaDoString(string.Concat(new object[] { randomString, " = \"\"; local name, currentAmount, texture, earnedThisWeek, weeklyMax, totalMax, isDiscovered = GetCurrencyInfo(", entry, "); ", randomString, " = tostring(name) .. \"##\" .. tostring(currentAmount) .. \"##\" .. tostring(texture) .. \"##\" .. tostring(earnedThisWeek)  .. \"##\" .. tostring(weeklyMax)  .. \"##\" .. tostring(totalMax)  .. \"##\" .. tostring(isDiscovered);" }), randomString, false);
            if (!string.IsNullOrWhiteSpace(str2))
            {
                string[] separator = new string[] { "##" };
                string[] strArray2 = str2.Split(separator, StringSplitOptions.None);
                if (strArray2.Length == 6)
                {
                    return new WoWCurrency { Entry = entry, Name = strArray2[0], CurrentAmount = Others.ToInt32(strArray2[1]), FileId = Others.ToInt32(strArray2[2]), EarnedThisWeek = Others.ToInt32(strArray2[3]), WeeklyMax = Others.ToInt32(strArray2[4]), TotalMax = Others.ToInt32(strArray2[5]), IsDiscovered = Others.ToBoolean(strArray2[6]) };
                }
            }
            return new WoWCurrency();
        }

        public static List<WoWMap.MapDbcRecord> GetInstancesList()
        {
            List<WoWMap.MapDbcRecord> list = WoWMap.WoWMaps(WoWMap.InstanceType.Party, WoWMap.MapType.ADTType);
            list.AddRange(WoWMap.WoWMaps(WoWMap.InstanceType.Party, WoWMap.MapType.WDTOnlyType));
            return list;
        }

        public static string GetPlayerName(UInt128 guid)
        {
            try
            {
                uint num = Memory.WowMemory.Memory.ReadUInt((Memory.WowProcess.WowModule + 0xdd5de0) + 20);
                uint dwAddress = num;
                do
                {
                    uint num3 = dwAddress + 0x10;
                    if (Memory.WowMemory.Memory.ReadUInt128(num3) == guid)
                    {
                        return Memory.WowMemory.Memory.ReadUTF8String(num3 + 0x11);
                    }
                    dwAddress = Memory.WowMemory.Memory.ReadUInt(dwAddress);
                }
                while ((dwAddress != 0) && (dwAddress != num));
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetPlayerName(UInt128 guid): " + exception, true);
            }
            return "Unknow";
        }

        public static List<WoWMap.MapDbcRecord> GetRaidsList()
        {
            List<WoWMap.MapDbcRecord> list = WoWMap.WoWMaps(WoWMap.InstanceType.Raid, WoWMap.MapType.ADTType);
            list.AddRange(WoWMap.WoWMaps(WoWMap.InstanceType.Raid, WoWMap.MapType.WDTOnlyType));
            return list;
        }

        public static Reaction GetReputationReaction(int reputationId)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            return (Reaction) Others.ToInt32(Lua.LuaDoString(string.Concat(new object[] { "_, _,", randomString, ", _, _, _, _, _, _, _, _, _, _, _, _, _= GetFactionInfoByID(", reputationId, ")" }), randomString, false));
        }

        public static Point GetSafeResPoint()
        {
            float degrees = 0f;
            Point positionCorpse = nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse;
            Point point = new Point();
            Point other = new Point(0f, 0f, 0f, "None");
            while (degrees < 360f)
            {
                float x = positionCorpse.X + ((float) (30.0 * System.Math.Cos((double) nManager.Helpful.Math.DegreeToRadian(degrees))));
                float y = positionCorpse.Y + ((float) (30.0 * System.Math.Sin((double) nManager.Helpful.Math.DegreeToRadian(degrees))));
                other = new Point(x, y, PathFinder.GetZPosition(x, y, false), "None");
                if ((nManager.Wow.ObjectManager.ObjectManager.Me.PositionCorpse.DistanceTo(other) < 36f) && !nManagerSetting.IsBlackListedZone(other))
                {
                    bool flag;
                    List<Point> listPoints = PathFinder.FindPath(other, out flag, true, false);
                    if (flag && (nManager.Helpful.Math.DistanceListPoint(listPoints) <= 40f))
                    {
                        WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitHostile(), other, false, false, false);
                        WoWUnit unit2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitHostile(), point, false, false, false);
                        if (!point.IsValid)
                        {
                            point = other;
                        }
                        if (point.DistanceTo(unit2.Position) < other.DistanceTo(unit.Position))
                        {
                            point = other;
                        }
                    }
                }
                degrees += 15f;
            }
            return point;
        }

        public static bool IsCompletedAchievement(int achievementId, bool meOnly = false)
        {
            if (achievementId <= 0)
            {
                return true;
            }
            if (AchievementsDoneCache.Contains(achievementId))
            {
                return true;
            }
            if (!AchievementsNotDoneCache.Contains(achievementId))
            {
                string str4;
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                string str2 = Others.GetRandomString(Others.Random(4, 10));
                string str3 = Others.GetRandomString(Others.Random(4, 10));
                if (meOnly)
                {
                    str4 = string.Concat(new object[] { "_, _, _, ", str2, ", _, _, _, _, _, _, _, _, ", str3, " = GetAchievementInfo(", achievementId, "); if ", str2, " and ", str3, " then ", randomString, "=\"1\" else ", randomString, "=\"0\" end;" });
                }
                else
                {
                    str4 = string.Concat(new object[] { "_, _, _, ", str2, " = GetAchievementInfo(", achievementId, "); if ", str2, " then ", randomString, "=\"1\" else ", randomString, "=\"0\" end;" });
                }
                Lua.LuaDoString(str4, false, true);
                if (Lua.GetLocalizedText(randomString) == "1")
                {
                    AchievementsDoneCache.Add(achievementId);
                    return true;
                }
                AchievementsNotDoneCache.Add(achievementId);
            }
            return false;
        }

        public static int LaunchWow(string param = "")
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Blizzard Entertainment\World of Warcraft");
                if (key == null)
                {
                    MakeWowRegistry();
                    LaunchWow("");
                    return 0;
                }
                object obj2 = key.GetValue("InstallPath");
                if (obj2 == null)
                {
                    MakeWowRegistry();
                    LaunchWow("");
                    return 0;
                }
                if (!File.Exists(obj2 + "Wow.exe"))
                {
                    MakeWowRegistry();
                    LaunchWow("");
                    return 0;
                }
                Process process = new Process {
                    StartInfo = { FileName = obj2 + "Wow.exe", Arguments = param }
                };
                process.Start();
                return process.Id;
            }
            catch (Exception exception)
            {
                Logging.WriteError("LaunchWow(): " + exception, true);
            }
            return 0;
        }

        public static void MakeWowRegistry()
        {
            try
            {
                MessageBox.Show(string.Format("{0}.", Translate.Get(Translate.Id.Please_select_exe_in_the_install_folder_of_the_game)));
                string str = Others.DialogBoxOpenFile("", "Profile files (Wow.exe)|Wow.exe");
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Blizzard Entertainment\World of Warcraft") ?? Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Blizzard Entertainment\World of Warcraft");
                if (key != null)
                {
                    key.SetValue("InstallPath", str.Replace("Wow.exe", ""), RegistryValueKind.String);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("MakeWowRegistry(): " + exception, true);
            }
        }

        public static void OpenAllBags()
        {
            try
            {
                Lua.LuaDoString("OpenAllBags();", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("OpenAllBags(): " + exception, true);
            }
        }

        public static void ResetAllInstances()
        {
            Lua.LuaDoString("ResetInstances()", false, true);
        }

        public static void SleepGlobalCooldown()
        {
            for (int i = 1; SpellManager.IsOnGlobalCooldown; i++)
            {
                if (i < 100)
                {
                    Thread.Sleep((int) (300 / i));
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        public static void TagMonstersArround(Spell spellToUses, float range, List<int> entry = null)
        {
            SleepGlobalCooldown();
            foreach (WoWUnit unit in nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWUnit60Yards())
            {
                if (((unit.IsAlive && !unit.InCombat) && ((entry == null) || entry.Contains(unit.Entry))) && (((UnitRelation.GetReaction(nManager.Wow.ObjectManager.ObjectManager.Me.Faction, unit.Faction) > Reaction.Hostile) && unit.IsInRange(range)) && (nManager.Wow.ObjectManager.ObjectManager.Me.Target != unit.Guid)))
                {
                    Interact.InteractWith(unit.GetBaseAddress, true);
                    Thread.Sleep(100);
                    spellToUses.Cast(true, true, false, null);
                    break;
                }
            }
        }

        public static void TagMonstersArround(Spell spellToUses, float range, int entry)
        {
            TagMonstersArround(spellToUses, range, new List<int> { entry });
        }

        public static void UpdateLastHardwareAction()
        {
            if ((!Memory.WowMemory.IsGameFrameLocked && _uhavuhi) && (InGame && !IsLoading))
            {
                _uhavuhi = false;
                if (string.IsNullOrEmpty(AfkKeyPress) || (AfkKeyPress == "B"))
                {
                    Thread.Sleep(10);
                    AfkKeyPress = nManager.Wow.Helpers.Keybindings.GetAFreeKey(true);
                    AfkTimer.Reset();
                }
                if (!AfkTimer.IsReady)
                {
                    _uhavuhi = true;
                }
                else
                {
                    Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, AfkKeyPress);
                    Thread.Sleep(10);
                    Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, AfkKeyPress);
                    AfkTimer.Reset();
                    _uhavuhi = true;
                }
            }
        }

        public static uint WowVersion(string textBuild = "")
        {
            try
            {
                if (textBuild == "")
                {
                    textBuild = Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule + 0xdcaac0);
                }
                if (!textBuild.Contains<char>(' '))
                {
                    return 0;
                }
                string[] strArray = textBuild.Split(new char[] { ' ' });
                return Others.ToUInt32(strArray[strArray.Length - 1].Remove(5));
            }
            catch (Exception exception)
            {
                Logging.WriteError("WowVersion: " + exception, true);
                return 0;
            }
        }

        public static int AreaId
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xcf6fe8);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaId: " + exception, true);
                    return 0;
                }
            }
        }

        public static int ContinentId
        {
            get
            {
                if (((_ojalapeo == 0x45c) || (_ojalapeo == 0)) || (((_ojalapeo == 1) || (_ojalapeo == 530)) || (_ojalapeo == 870)))
                {
                    return _ojalapeo;
                }
                if (_ojalapeo == 0x286)
                {
                    WoWGameObject objectByGuid = nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(nManager.Wow.ObjectManager.ObjectManager.Me.TransportGuid) as WoWGameObject;
                    if (((objectByGuid != null) && objectByGuid.IsValid) && (objectByGuid.Entry == 0x31cf2))
                    {
                        return 0x1e240;
                    }
                    return 0x286;
                }
                if (!Garrison.GarrisonMapIdList.Contains(_ojalapeo))
                {
                    return _ojalapeo;
                }
                return 0x45c;
            }
            set
            {
                _ojalapeo = value;
            }
        }

        public static string ContinentNameMpq
        {
            get
            {
                try
                {
                    return ContinentNameMpqByContinentId(ContinentId);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("ContinentNameMpq: " + exception, true);
                    return "Azeroth";
                }
            }
        }

        public static MovementFlags GetAllMovementStatus
        {
            get
            {
                return (MovementFlags) Memory.WowMemory.Memory.ReadInt(Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + 0x124) + 0x40);
            }
        }

        public static int GetContainerNumFreeSlots
        {
            get
            {
                int num;
                try
                {
                    lock (typeof(Usefuls))
                    {
                        if (!_koceilim.IsReady)
                        {
                            return _icouhe;
                        }
                        _koceilim = new nManager.Helpful.Timer(1000.0);
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        string input = Lua.LuaDoString(randomString + " = 0; for i = 0, 4 do if GetContainerNumFreeSlots(i) ~= nil then " + randomString + " = " + randomString + " + GetContainerNumFreeSlots(i); end end  ", randomString, false);
                        if (Regex.IsMatch(input, "^[0-9]+$"))
                        {
                            _icouhe = Others.ToInt32(input);
                        }
                        else
                        {
                            Logging.WriteError("GetContainerNumFreeSlots failed, \"" + input + "\" returned.", true);
                        }
                        num = _icouhe;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetContainerNumFreeSlots: " + exception, true);
                    num = 50;
                }
                return num;
            }
        }

        public static int GetHonorPoint
        {
            get
            {
                int num2;
                lock (typeof(Usefuls))
                {
                    try
                    {
                        int num;
                        if (!TimerHonorPoint.IsReady)
                        {
                            return _riwohero;
                        }
                        TimerHonorPoint.Reset();
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString("_, " + randomString + " = GetCurrencyInfo(392);", false, true);
                        try
                        {
                            string localizedText = Lua.GetLocalizedText(randomString);
                            if (localizedText != null)
                            {
                                num = Others.ToInt32(localizedText);
                            }
                            else
                            {
                                num = -1;
                            }
                        }
                        catch
                        {
                            num = -1;
                        }
                        if ((num >= -1) || (num <= 0xfa0))
                        {
                            _riwohero = num;
                        }
                        num2 = _riwohero;
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("GetHonorPoint: " + exception, true);
                        num2 = 0;
                    }
                }
                return num2;
            }
        }

        public static ulong GetMoneyCopper
        {
            get
            {
                ulong num;
                lock (typeof(Usefuls))
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = GetMoney()", false, true);
                        num = Others.ToUInt64(Lua.GetLocalizedText(randomString));
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("GetMoneyCopper: " + exception, true);
                        num = 0L;
                    }
                }
                return num;
            }
        }

        public static int GetWoWTime
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xd75cc0);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetWoWTime: " + exception, true);
                    return 0;
                }
            }
        }

        public static bool InGame
        {
            get
            {
                try
                {
                    return (Memory.WowMemory.Memory.ReadByte(Memory.WowProcess.WowModule + 0xf3de96) > 0);
                }
                catch (Exception exception)
                {
                    if (exception.ToString() == "Process is not open for read/write.")
                    {
                        Thread.Sleep(500);
                    }
                    Logging.WriteError("InGame: " + exception, true);
                    return false;
                }
            }
        }

        public static bool IsFalling
        {
            get
            {
                return nManager.Wow.ObjectManager.ObjectManager.Me.IsFalling;
            }
        }

        public static bool IsFlyableArea
        {
            get
            {
                lock (typeof(Usefuls))
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = tostring(IsFlyableArea())", false, true);
                        if (Lua.GetLocalizedText(randomString) == "true")
                        {
                            Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                            position.Z += 30f;
                            return !TraceLine.TraceLineGo(position);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("IsFlyableArea: " + exception, true);
                    }
                    return false;
                }
            }
        }

        public static bool IsFlying
        {
            get
            {
                return nManager.Wow.ObjectManager.ObjectManager.Me.IsFlying;
            }
        }

        public static bool IsInBattleground
        {
            get
            {
                if (CachedBattlegroundList == null)
                {
                    CachedBattlegroundList = new List<uint>();
                    List<WoWMap.MapDbcRecord> battlegroundsList = GetBattlegroundsList();
                    for (int i = 0; i < battlegroundsList.Count; i++)
                    {
                        CachedBattlegroundList.Add(battlegroundsList[i].Id);
                    }
                }
                return CachedBattlegroundList.Contains((uint) ContinentId);
            }
        }

        public static bool IsInDungeon
        {
            get
            {
                if (CachedInstanceList == null)
                {
                    CachedInstanceList = new List<uint>();
                    List<WoWMap.MapDbcRecord> instancesList = GetInstancesList();
                    for (int i = 0; i < instancesList.Count; i++)
                    {
                        CachedInstanceList.Add(instancesList[i].Id);
                    }
                }
                return CachedInstanceList.Contains((uint) ContinentId);
            }
        }

        public static bool IsInPetBattle
        {
            get
            {
                return (Memory.WowMemory.Memory.ReadByte(Memory.WowProcess.WowModule + 0xcfe718) == 1);
            }
        }

        public static bool IsInRaid
        {
            get
            {
                if (CachedRaidList == null)
                {
                    CachedRaidList = new List<uint>();
                    List<WoWMap.MapDbcRecord> raidsList = GetRaidsList();
                    for (int i = 0; i < raidsList.Count; i++)
                    {
                        CachedRaidList.Add(raidsList[i].Id);
                    }
                }
                return CachedRaidList.Contains((uint) ContinentId);
            }
        }

        public static bool IsLoading
        {
            get
            {
                try
                {
                    return (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xd690f0) != 0);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("IsLoading: " + exception, true);
                    return false;
                }
            }
        }

        public static bool IsOutdoors
        {
            get
            {
                bool flag2;
                lock (typeof(Usefuls))
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = tostring(IsOutdoors())", false, true);
                        flag2 = Lua.GetLocalizedText(randomString) == "true";
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("IsOutdoors: " + exception, true);
                        flag2 = false;
                    }
                }
                return flag2;
            }
        }

        public static bool IsSwimming
        {
            get
            {
                return nManager.Wow.ObjectManager.ObjectManager.Me.IsSwimming;
            }
        }

        public static int Latency
        {
            get
            {
                try
                {
                    if (!_haifuimoebieroEcelu.IsReady)
                    {
                        return _jubaisoi;
                    }
                    _haifuimoebieroEcelu = new nManager.Helpful.Timer(30000.0);
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString("_,_,_,worldLag=GetNetStats() " + randomString + "=worldLag", false, true);
                    _jubaisoi = Others.ToInt32(Lua.GetLocalizedText(randomString));
                    return ((_jubaisoi == 0) ? 1 : _jubaisoi);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Latency: " + exception, true);
                    return 1;
                }
            }
        }

        public static string MapName
        {
            get
            {
                try
                {
                    return ContinentNameByContinentId(ContinentId);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("MapName: " + exception, true);
                    return "Azeroth";
                }
            }
        }

        public static string MapZoneName
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xf3e0b8));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("MapZoneName: " + exception, true);
                    return "";
                }
            }
        }

        public static bool PlayerUsingVehicle
        {
            get
            {
                bool flag2;
                lock (typeof(Usefuls))
                {
                    try
                    {
                        if (!_ojoejouq.IsReady)
                        {
                            return _eleboupijureimPohejilub;
                        }
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = tostring(UnitUsingVehicle(\"player\"));", false, true);
                        _eleboupijureimPohejilub = Lua.GetLocalizedText(randomString) == "true";
                        _ojoejouq = new nManager.Helpful.Timer(500.0);
                        flag2 = _eleboupijureimPohejilub;
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("PlayerUsingVehicle: " + exception, true);
                        flag2 = false;
                    }
                }
                return flag2;
            }
        }

        public static int RealContinentId
        {
            get
            {
                return _ojalapeo;
            }
        }

        public static string RealmName
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0x101fd3c) + 0x394);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("RealmName: " + exception, true);
                    return "";
                }
            }
        }

        public static int SubAreaId
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xcf6fec);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("SubAreaId: " + exception, true);
                    return 0;
                }
            }
        }

        public static string SubMapZoneName
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xf3e0c0));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("SubMapZoneName: " + exception, true);
                    return "";
                }
            }
        }

        public class WoWCurrency
        {
            public int CurrentAmount;
            public int EarnedThisWeek;
            public int Entry;
            public int FileId;
            public bool IsDiscovered;
            public string Name;
            public int TotalMax;
            public int WeeklyMax;

            public bool IsValid
            {
                get
                {
                    return (this.Name != "");
                }
            }
        }
    }
}

