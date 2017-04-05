namespace nManager.Wow.Helpers
{
    using Microsoft.Win32;
    using nManager;
    using nManager.Helpful;
    using nManager.Wow;
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
        private static int _continentId;
        private static int _lastContainerNumFreeSlots;
        private static int _lastHonorPoint;
        private static int _lastLatency;
        private static bool _lastResultPlayerUsingVehicle;
        private static nManager.Helpful.Timer _timePlayerUsingVehicle = new nManager.Helpful.Timer(0.0);
        private static nManager.Helpful.Timer _timerContainerNumFreeSlots = new nManager.Helpful.Timer(0.0);
        private static nManager.Helpful.Timer _timerLatency = new nManager.Helpful.Timer(0.0);
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
            }
            return WoWMap.FromId(cId).MapMPQName;
        }

        public static string ContinentNameMpqByContinentId(int cId)
        {
            return WoWMap.FromId(cId).MapMPQName;
        }

        public static void DisMount()
        {
            try
            {
                Lua.RunMacroText(nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()) ? "/cancelform" : "/dismount");
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
                uint num = Memory.WowMemory.Memory.ReadUInt((Memory.WowProcess.WowModule + 0xc60428) + 20);
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

        public static bool IsCompletedAchievement(int achievementId, bool meOnly = false)
        {
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

        public static bool MovementStatus(MovementFlags flags)
        {
            try
            {
                return Convert.ToBoolean((int) (((MovementFlags) Memory.WowMemory.Memory.ReadInt(Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + 300) + 0x40)) & flags));
            }
            catch (Exception exception)
            {
                Logging.WriteError("MovementStatus: " + exception, true);
                return false;
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

        public static void UpdateLastHardwareAction()
        {
            lock (ThisLock)
            {
                if (!Memory.WowMemory.IsGameFrameLocked)
                {
                    if (!InGame || IsLoadingOrConnecting)
                    {
                        Thread.Sleep(10);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(AfkKeyPress))
                        {
                            Thread.Sleep(10);
                            AfkKeyPress = nManager.Wow.Helpers.Keybindings.GetAFreeKey(true);
                            AfkTimer.Reset();
                        }
                        if (AfkTimer.IsReady)
                        {
                            Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, AfkKeyPress);
                            Thread.Sleep(10);
                            Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, AfkKeyPress);
                            AfkTimer.Reset();
                        }
                    }
                }
            }
        }

        public static uint WowVersion(string textBuild = "")
        {
            try
            {
                if (textBuild == "")
                {
                    textBuild = Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule + 0xc5db08);
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
                    return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xbb3f78);
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
                if (((_continentId == 0x45c) || (_continentId == 0)) || (((_continentId == 1) || (_continentId == 530)) || (_continentId == 870)))
                {
                    return _continentId;
                }
                if (!Garrison.GarrisonMapIdList.Contains(_continentId))
                {
                    return _continentId;
                }
                return 0x45c;
            }
            set
            {
                _continentId = value;
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
                return (MovementFlags) Memory.WowMemory.Memory.ReadInt(Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + 300) + 0x40);
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
                        if (!_timerContainerNumFreeSlots.IsReady)
                        {
                            return _lastContainerNumFreeSlots;
                        }
                        _timerContainerNumFreeSlots = new nManager.Helpful.Timer(1000.0);
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        string input = Lua.LuaDoString(randomString + " = 0; for i = 0, 4 do if GetContainerNumFreeSlots(i) ~= nil then " + randomString + " = " + randomString + " + GetContainerNumFreeSlots(i); end end  ", randomString, false);
                        if (Regex.IsMatch(input, "^[0-9]+$"))
                        {
                            _lastContainerNumFreeSlots = Others.ToInt32(input);
                        }
                        else
                        {
                            Logging.WriteError("GetContainerNumFreeSlots failed, \"" + input + "\" returned.", true);
                        }
                        num = _lastContainerNumFreeSlots;
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
                            return _lastHonorPoint;
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
                            _lastHonorPoint = num;
                        }
                        num2 = _lastHonorPoint;
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

        public static int GetMoneyCopper
        {
            get
            {
                int num;
                lock (typeof(Usefuls))
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = GetMoney()", false, true);
                        num = Others.ToInt32(Lua.GetLocalizedText(randomString));
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("GetMoneyCopper: " + exception, true);
                        num = 0;
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
                    return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xc05060);
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
                    return (Memory.WowMemory.Memory.ReadByte(Memory.WowProcess.WowModule + 0xda55b2) > 0);
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
                return MovementStatus(MovementFlags.Falling);
            }
        }

        public static bool IsFlyableArea
        {
            get
            {
                bool flag2;
                lock (typeof(Usefuls))
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = tostring(IsFlyableArea())", false, true);
                        flag2 = Lua.GetLocalizedText(randomString) == "true";
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("IsFlyableArea: " + exception, true);
                        flag2 = false;
                    }
                }
                return flag2;
            }
        }

        public static bool IsFlying
        {
            get
            {
                return MovementStatus(MovementFlags.Flying);
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

        public static bool IsLoadingOrConnecting
        {
            get
            {
                try
                {
                    return (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xbf8754) != 0);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("IsLoadingOrConnecting: " + exception, true);
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
                return MovementStatus(MovementFlags.None | MovementFlags.Swimming);
            }
        }

        public static int Latency
        {
            get
            {
                try
                {
                    if (!_timerLatency.IsReady)
                    {
                        return _lastLatency;
                    }
                    _timerLatency = new nManager.Helpful.Timer(30000.0);
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString("_,_,_,worldLag=GetNetStats() " + randomString + "=worldLag", false, true);
                    _lastLatency = Others.ToInt32(Lua.GetLocalizedText(randomString));
                    return ((_lastLatency == 0) ? 1 : _lastLatency);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Latency: " + exception, true);
                    return 0;
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
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xda55a8));
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
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InTransport)
                        {
                            return true;
                        }
                        if (!_timePlayerUsingVehicle.IsReady)
                        {
                            return _lastResultPlayerUsingVehicle;
                        }
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = tostring(UnitUsingVehicle(\"player\"));", false, true);
                        _lastResultPlayerUsingVehicle = Lua.GetLocalizedText(randomString) == "true";
                        _timePlayerUsingVehicle = new nManager.Helpful.Timer(500.0);
                        flag2 = _lastResultPlayerUsingVehicle;
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
                return _continentId;
            }
        }

        public static string RealmName
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule + 0xe981c6);
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
                    return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xbb3f70);
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
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xda55a4));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("SubMapZoneName: " + exception, true);
                    return "";
                }
            }
        }
    }
}

