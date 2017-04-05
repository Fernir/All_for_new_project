namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Plugins;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.MemoryClass.Magic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class SpellManager
    {
        private static List<uint> _spellBookID = new List<uint>();
        private static List<Spell> _spellBookSpell = new List<Spell>();
        public static Dictionary<uint, SpellInfoLua> _spellInfos = new Dictionary<uint, SpellInfoLua>();
        private static bool _usedSbid;
        private static readonly Dictionary<string, List<uint>> CacheSpellIdByName = new Dictionary<string, List<uint>>();
        private static readonly List<uint> FlightFormsIdsList = new List<uint>();
        private static readonly List<uint> KnownGetCooldownIssues = new List<uint> { 0xc931 };
        private static readonly List<uint> MountDruidIdList = new List<uint>();
        public static bool SpellBookLoaded;
        public static object SpellManagerLocker = new object();

        public static void CastSpellByIDAndPosition(uint spellId, Point postion)
        {
            try
            {
                ClickOnTerrain.Spell(spellId, postion);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CastSpellByIDAndPosition(UInt32 spellId, Point postion): " + exception, true);
            }
        }

        public static void CastSpellByIdLUA(uint spellId)
        {
            try
            {
                Spell spell = new Spell(spellId);
                CastSpellByNameLUA(spell.NameInGame);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CastSpellByIdLUA(uint spellId): " + exception, true);
            }
        }

        public static void CastSpellByNameLUA(string spellName)
        {
            try
            {
                Lua.LuaDoString("CastSpellByName(\"" + spellName + "\");", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CastSpellByNameLUA(string spellName): " + exception, true);
            }
        }

        public static void CastSpellByNameLUA(string spellName, string target)
        {
            try
            {
                Lua.LuaDoString("CastSpellByName(\"" + spellName + "\", \"" + target + "\");", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CastSpellByNameLUA(string spellName, string target): " + exception, true);
            }
        }

        public static bool ExistMountLUA(string spellName)
        {
            try
            {
                return (Lua.LuaDoString("ret = \"\"; nameclient = \"" + spellName + "\"; for i=1,GetNumCompanions(\"MOUNT\"),1 do local _, name = GetCompanionInfo(\"MOUNT\", i)  if name == nameclient then ret = \"true\"  return end  end", "ret", false) == "true");
            }
            catch (Exception exception)
            {
                Logging.WriteError("ExistSpellLUA(string spellName): " + exception, true);
                return false;
            }
        }

        public static bool ExistSpellBookLUA(string spellName)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                string str2 = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = \"\"; " + str2 + " = \"" + spellName + "\"; if (GetSpellBookItemInfo(" + str2 + ")) then " + randomString + " = \"true\" else " + randomString + " = \"false\" end", false, true);
                return (Lua.GetLocalizedText(randomString) == "true");
            }
            catch (Exception exception)
            {
                Logging.WriteError("ExistSpellBookLUA(string spellName): " + exception, true);
                return false;
            }
        }

        public static List<uint> FlightFormsIds()
        {
            try
            {
                if (FlightFormsIdsList.Count <= 0)
                {
                    FlightFormsIdsList.AddRange(SpellListManager.SpellIdByName("Swift Flight Form"));
                    FlightFormsIdsList.AddRange(SpellListManager.SpellIdByName("Flight Form"));
                }
                return FlightFormsIdsList;
            }
            catch (Exception exception)
            {
                Logging.WriteError("MountDruidId(): " + exception, true);
            }
            return new List<uint>();
        }

        public static string GetAquaticMountName()
        {
            try
            {
                List<string> spellList = new List<string>(Others.ReadFileAllLines(Application.StartupPath + @"\Data\aquaticmountList.txt"));
                string clientNameBySpellName = GetClientNameBySpellName(spellList);
                if (clientNameBySpellName != "")
                {
                    Logging.Write("Found aquatic mount: " + clientNameBySpellName);
                }
                return clientNameBySpellName;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetAquaticMountName(): " + exception, true);
            }
            return "";
        }

        public static string GetClientNameBySpellName(List<string> spellList)
        {
            try
            {
                foreach (uint num in SpellBookID())
                {
                    string item = SpellListManager.SpellNameById(num);
                    if (spellList.Contains(item))
                    {
                        return item;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetClientNameBySpellName(List<string> spellList): " + exception, true);
            }
            return "";
        }

        public static string GetFlyMountName()
        {
            try
            {
                List<string> spellList = new List<string>(Others.ReadFileAllLines(Application.StartupPath + @"\Data\flymountList.txt"));
                string clientNameBySpellName = GetClientNameBySpellName(spellList);
                if (clientNameBySpellName != "")
                {
                    Logging.Write("Found flying mount: " + clientNameBySpellName);
                }
                return clientNameBySpellName;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetFlyMountName(): " + exception, true);
            }
            return "";
        }

        public static string GetMountName()
        {
            try
            {
                List<string> spellList = new List<string>(Others.ReadFileAllLines(Application.StartupPath + @"\Data\mountList.txt"));
                string clientNameBySpellName = GetClientNameBySpellName(spellList);
                if (clientNameBySpellName != "")
                {
                    Logging.Write("Found mount: " + clientNameBySpellName);
                }
                return clientNameBySpellName;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetMountName(): " + exception, true);
            }
            return "";
        }

        public static uint GetSpellIdBySpellNameInGame(string spellName)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("_, " + randomString + " = GetSpellBookItemInfo(\"" + spellName + "\")", false, true);
                return uint.Parse(Lua.GetLocalizedText(randomString));
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSpellInfoLUA(string spellNameInGame): " + exception, true);
            }
            return 0;
        }

        public static SpellInfoLua GetSpellInfo(uint id)
        {
            try
            {
                lock (SpellManagerLocker)
                {
                    if (_spellInfos.ContainsKey(id))
                    {
                        return _spellInfos[id];
                    }
                    string randomString = Others.GetRandomString(Others.Random(5, 10));
                    string str2 = Lua.LuaDoString(string.Concat(new object[] { randomString, " = \"\"; local name, rank, icon, castTime, minRange, maxRange, spellId = GetSpellInfo(", id, "); ", randomString, " = tostring(name) .. \"##\" .. tostring(icon) .. \"##\" .. tostring(castTime) .. \"##\" .. tostring(minRange)  .. \"##\" .. tostring(maxRange)  .. \"##\" .. tostring(spellId);" }), randomString, false);
                    if (!string.IsNullOrWhiteSpace(str2))
                    {
                        string[] separator = new string[] { "##" };
                        string[] strArray2 = str2.Split(separator, StringSplitOptions.None);
                        if (strArray2.Length == 6)
                        {
                            int num;
                            float num2;
                            SpellInfoLua lua = new SpellInfoLua {
                                ID = id
                            };
                            if (!string.IsNullOrWhiteSpace(strArray2[0]) && (strArray2[0] != "nil"))
                            {
                                lua.Name = strArray2[0];
                            }
                            if (!string.IsNullOrWhiteSpace(strArray2[2]))
                            {
                                lua.Icon = strArray2[2];
                            }
                            if (!string.IsNullOrWhiteSpace(strArray2[3]) && int.TryParse(strArray2[3].Replace(".", ","), out num))
                            {
                                lua.CastTime = num;
                            }
                            if (!string.IsNullOrWhiteSpace(strArray2[4]) && float.TryParse(strArray2[4].Replace(".", ","), out num2))
                            {
                                lua.MinRange = num2;
                            }
                            if (!string.IsNullOrWhiteSpace(strArray2[5]) && float.TryParse(strArray2[5].Replace(".", ","), out num2))
                            {
                                lua.MaxRange = num2;
                            }
                            _spellInfos.Add(id, lua);
                            return lua;
                        }
                        Logging.WriteDebug("Return as bad format: public static SpellInfo GetSpellInfo(" + id + ")");
                    }
                    else
                    {
                        Logging.WriteDebug("Cannot find spell: public static SpellInfo GetSpellInfo(" + id + ")");
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellInfo GetSpellInfo(uint id): " + exception, true);
            }
            return new SpellInfoLua();
        }

        public static Spell GetSpellInfoLUA(string spellNameInGame)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("_, " + randomString + " = GetSpellBookItemInfo(\"" + spellNameInGame + "\")", false, true);
                return new Spell(Lua.GetLocalizedText(randomString));
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSpellInfoLUA(string spellNameInGame): " + exception, true);
            }
            return new Spell("");
        }

        public static bool HasSpell(int spellId)
        {
            if (spellId > 0x30d40)
            {
                return false;
            }
            return ((Memory.WowMemory.Memory.ReadInt(Memory.WowMemory.Memory.ReadUInt((Memory.WowProcess.WowModule + 0xe02ee0) + ((uint) ((spellId >> 5) * 4)))) & ~(((int) 1) << spellId)) != 0);
        }

        public static bool HaveBuffLua(string spellNameInGame)
        {
            try
            {
                lock (typeof(SpellManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + " = \"false\" for i=1,40 do local n,_,_,_,_,_,_,_,id=UnitBuff(\"player\",i); if n == \"" + spellNameInGame + "\" then " + randomString + " = \"true\" end end", false, true);
                    return (Lua.GetLocalizedText(randomString) == "true");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("HaveBuffLua(string spellNameInGame): " + exception, true);
            }
            return false;
        }

        public static bool IsSpellOnCooldown(uint spellId, uint categoryId = 0, uint startRecoveryCategoryId = 0)
        {
            if (KnownGetCooldownIssues.Contains(spellId))
            {
                return (TimeLeftOnSpellCooldownLUA(spellId) > 0.0);
            }
            foreach (SpellCooldownEntry entry in GetAllSpellsOnCooldown)
            {
                if ((entry.GCDDuration <= 0) && ((entry.SpellId == spellId) || ((entry.SpellCategoryId == categoryId) && (categoryId != 0))))
                {
                    int num2 = Usefuls.GetWoWTime - entry.StartTime;
                    if (((entry.SpellOrItemCooldownDuration > 0) && (entry.SpellId == spellId)) && (entry.SpellOrItemCooldownDuration >= num2))
                    {
                        return true;
                    }
                    if ((entry.CategoryCooldownDuration > 0) && (entry.CategoryCooldownDuration >= num2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsSpellOnCooldown(List<uint> spellIds, uint categoryId = 0, uint startRecoveryCategoryId = 0)
        {
            for (int i = 0; i < spellIds.Count; i++)
            {
                uint spellId = spellIds[i];
                if (IsSpellOnCooldown(spellId, categoryId, startRecoveryCategoryId))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSpellUsableLUA(Spell spell)
        {
            try
            {
                if (IsSpellOnCooldown(spell.Id, spell.CategoryId, spell.StartRecoveryCategoryId))
                {
                    return false;
                }
                if (IsSpellOnCooldown(spell.Ids, spell.CategoryId, spell.StartRecoveryCategoryId))
                {
                    return false;
                }
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                string str2 = Others.GetRandomString(Others.Random(4, 10));
                string commandline = Others.GetRandomString(Others.Random(4, 10));
                string str6 = randomString + "," + str2 + "=IsUsableSpell(\"" + spell.NameInGame + "\"); ";
                Lua.LuaDoString(((str6 + "if " + randomString + " and not " + str2 + " then ") + commandline + "=\"1\" ") + "end ", false, false);
                return (Lua.GetLocalizedText(commandline) == "1");
            }
            catch (Exception exception)
            {
                Logging.WriteError("public static bool IsSpellUsableLUA(Spell spell): " + exception, true);
                return false;
            }
        }

        public static bool KnownSpell(uint spellId)
        {
            return SpellBookID().Contains(spellId);
        }

        public static List<uint> MountDruidId()
        {
            try
            {
                if (MountDruidIdList.Count <= 0)
                {
                    MountDruidIdList.AddRange(SpellListManager.SpellIdByName("Travel Form"));
                    MountDruidIdList.AddRange(SpellListManager.SpellIdByName("Sky Golem"));
                }
                return MountDruidIdList;
            }
            catch (Exception exception)
            {
                Logging.WriteError("MountDruidId(): " + exception, true);
            }
            return new List<uint>();
        }

        public static List<Spell> SpellBook()
        {
            try
            {
                lock (SpellManagerLocker)
                {
                    if (_spellBookSpell.Count <= 0)
                    {
                        Logging.Write("Initializing Character's SpellBook.");
                        SpellInfoCreateCache(SpellBookID());
                        SpellListManager.SpellIdByNameCreateCache();
                        List<Spell> list = new List<Spell>();
                        Logging.Write("May take few seconds...");
                        Memory.WowMemory.GameFrameLock();
                        foreach (uint num in SpellBookID())
                        {
                            list.Add(new Spell(num));
                        }
                        Memory.WowMemory.GameFrameUnLock();
                        Logging.Write("Character's SpellBook fully loaded. Found " + _spellBookID.Count + " spells, mounts and professions.");
                        Logging.Status = "Character SpellBook loaded.";
                        _spellBookSpell = list;
                    }
                }
                return _spellBookSpell;
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellBook(): " + exception, true);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
            return new List<Spell>();
        }

        public static List<uint> SpellBookID()
        {
            lock (SpellManagerLocker)
            {
                try
                {
                    while (_usedSbid)
                    {
                        Thread.Sleep(10);
                    }
                    if (_spellBookID.Count <= 0)
                    {
                        _usedSbid = true;
                        List<uint> list = new List<uint>();
                        uint num = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe02ee4);
                        uint num2 = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe02ee8);
                        for (uint i = 0; i < num; i++)
                        {
                            uint dwAddress = Memory.WowMemory.Memory.ReadUInt(num2 + (i * 4));
                            SpellInfo info = (SpellInfo) Memory.WowMemory.Memory.ReadObject(dwAddress, typeof(SpellInfo));
                            if (((info.TabId <= 1) || (info.TabId > 4)) && (info.State == SpellInfo.SpellState.Known))
                            {
                                list.Add(info.ID);
                            }
                            Application.DoEvents();
                        }
                        uint num5 = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe02f40);
                        uint num6 = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe02f44);
                        for (uint j = 0; j < num5; j++)
                        {
                            uint item = Memory.WowMemory.Memory.ReadUInt(num6 + (j * 4));
                            if (item > 0)
                            {
                                list.Add(item);
                            }
                            Application.DoEvents();
                        }
                        uint num9 = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe0309c);
                        uint num10 = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe03094);
                        while (((num9 & 1) == 0) && (num9 != 0))
                        {
                            uint num11 = Memory.WowMemory.Memory.ReadUInt(num9 + 20);
                            uint num12 = Memory.WowMemory.Memory.ReadUInt(num9 + 0x1c);
                            num9 = Memory.WowMemory.Memory.ReadUInt((num9 + 4) + num10);
                            if (num12 == 0)
                            {
                                break;
                            }
                            list.Remove(num11);
                            list.Add(num12);
                        }
                        _spellBookID = list;
                        _usedSbid = false;
                        SpellBookLoaded = true;
                    }
                    return _spellBookID;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("SpellBookID(): " + exception, true);
                }
                return new List<uint>();
            }
        }

        public static void SpellInfoCreateCache(List<uint> listId)
        {
            lock (SpellManagerLocker)
            {
                try
                {
                    string str = "{ ";
                    foreach (uint num in listId)
                    {
                        str = str + num + " ,";
                    }
                    if (str.EndsWith(","))
                    {
                        str = str.Remove(str.Length - 1);
                    }
                    str = str + "}";
                    string randomString = Others.GetRandomString(Others.Random(5, 10));
                    string str4 = Lua.LuaDoString(randomString + " = \"\"; local spellBookList = " + str + " for arrayId = 1, table.getn(spellBookList) do local name, rank, icon, castTime, minRange, maxRange, spellId = GetSpellInfo(spellBookList[arrayId]); " + randomString + " = " + randomString + " .. tostring(name) .. \"##\" .. tostring(icon) .. \"##\" .. tostring(castTime) .. \"##\" .. tostring(minRange)  .. \"##\" .. tostring(maxRange)  .. \"##\" .. tostring(spellId);" + randomString + " = " + randomString + " .. \"||\"end ", randomString, false);
                    if (!string.IsNullOrWhiteSpace(str4))
                    {
                        string[] strArray = str4.Split(new string[] { "||" }, StringSplitOptions.None);
                        if (strArray.Length > 0)
                        {
                            foreach (string str5 in strArray)
                            {
                                if (string.IsNullOrWhiteSpace(str5))
                                {
                                    goto Label_02FD;
                                }
                                string[] strArray2 = str5.Split(new string[] { "##" }, StringSplitOptions.None);
                                if (strArray2.Length == 6)
                                {
                                    int num2;
                                    float num3;
                                    SpellInfoLua lua = new SpellInfoLua();
                                    if (!string.IsNullOrWhiteSpace(strArray2[0]) && (strArray2[0] != "nil"))
                                    {
                                        lua.Name = strArray2[0];
                                    }
                                    if (!string.IsNullOrWhiteSpace(strArray2[1]))
                                    {
                                        lua.Icon = strArray2[1];
                                    }
                                    if (!string.IsNullOrWhiteSpace(strArray2[2]) && int.TryParse(strArray2[2].Replace(".", ","), out num2))
                                    {
                                        lua.CastTime = num2;
                                    }
                                    if (!string.IsNullOrWhiteSpace(strArray2[3]) && float.TryParse(strArray2[3].Replace(".", ","), out num3))
                                    {
                                        lua.MinRange = num3;
                                    }
                                    if (!string.IsNullOrWhiteSpace(strArray2[4]) && float.TryParse(strArray2[4].Replace(".", ","), out num3))
                                    {
                                        lua.MaxRange = num3;
                                    }
                                    if (!string.IsNullOrWhiteSpace(strArray2[5]) && int.TryParse(strArray2[5].Replace(".", ","), out num2))
                                    {
                                        lua.ID = (uint) num2;
                                    }
                                    if (listId.Contains(lua.ID) && !_spellInfos.ContainsKey(lua.ID))
                                    {
                                        _spellInfos.Add(lua.ID, lua);
                                    }
                                }
                                else
                                {
                                    Logging.WriteDebug("Return as bad format: public static SpellInfo SpellInfoCreateCache()");
                                }
                            }
                        }
                    }
                    else
                    {
                        Logging.WriteDebug("Cannot find spell: public static SpellInfo SpellInfoCreateCache()");
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("SpellInfo GetSpellInfo(uint id): " + exception, true);
                }
            Label_02FD:;
            }
        }

        public static Spell SpellInfoLUA(uint spellID)
        {
            try
            {
                return new Spell(spellID);
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellInfoLUA(uint spellID): " + exception, true);
            }
            return new Spell("");
        }

        public static double TimeLeftOnSpellCooldownLUA(uint spellId)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            string str2 = Others.GetRandomString(Others.Random(4, 10));
            string str3 = Others.GetRandomString(Others.Random(4, 10));
            string commandline = Others.GetRandomString(Others.Random(4, 10));
            string str6 = string.Concat(new object[] { randomString, ",", str2, ",_=GetSpellCooldown(", spellId, ") " }) + str3 + "=GetTime() ";
            string str7 = ((str6 + "if " + randomString + " == 0 or " + str2 + " == 0 then ") + commandline + " = 0 ") + "else ";
            Lua.LuaDoString((str7 + commandline + " = (" + randomString + " + " + str2 + " - " + str3 + ")*1000 ") + "end", false, false);
            return Convert.ToDouble(Lua.GetLocalizedText(commandline));
        }

        public static void UpdateSpellBook()
        {
            Thread thread2 = new Thread(new ThreadStart(SpellManager.UpdateSpellBookThread)) {
                Name = "SpellBook Update"
            };
            thread2.Start();
        }

        public static void UpdateSpellBookThread()
        {
            lock (SpellManagerLocker)
            {
                try
                {
                    Logging.Write("Initialize Character's SpellBook update.");
                    uint num = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe02ee4);
                    uint num2 = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe02ee8);
                    for (uint i = 0; i < num; i++)
                    {
                        uint dwAddress = Memory.WowMemory.Memory.ReadUInt(num2 + (i * 4));
                        SpellInfo info = (SpellInfo) Memory.WowMemory.Memory.ReadObject(dwAddress, typeof(SpellInfo));
                        if (((info.TabId <= 1) || (info.TabId > 4)) && (info.State == SpellInfo.SpellState.Known))
                        {
                            if (!_spellBookID.Contains(info.ID))
                            {
                                _spellBookID.Add(info.ID);
                            }
                            Spell item = SpellInfoLUA(info.ID);
                            if (!_spellBookSpell.Contains(item))
                            {
                                _spellBookSpell.Add(item);
                            }
                        }
                        Application.DoEvents();
                    }
                    uint num5 = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe02f40);
                    uint num6 = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xe02f44);
                    for (uint j = 0; j < num5; j++)
                    {
                        uint num8 = Memory.WowMemory.Memory.ReadUInt(num6 + (j * 4));
                        if (num8 > 0)
                        {
                            if (!_spellBookID.Contains(num8))
                            {
                                _spellBookID.Add(num8);
                            }
                            Spell spell2 = SpellInfoLUA(num8);
                            if (!_spellBookSpell.Contains(spell2))
                            {
                                _spellBookSpell.Add(spell2);
                            }
                        }
                        Application.DoEvents();
                    }
                    Logging.Write("Character's SpellBook is currently being fully updated. May take few seconds...");
                    Memory.WowMemory.GameFrameLock();
                    foreach (Spell spell3 in _spellBookSpell)
                    {
                        spell3.Update();
                    }
                    Memory.WowMemory.GameFrameUnLock();
                    if (CombatClass.IsAliveCombatClass)
                    {
                        CombatClass.ResetCombatClass();
                    }
                    if (HealerClass.IsAliveHealerClass)
                    {
                        HealerClass.ResetHealerClass();
                    }
                    nManager.Plugins.Plugins.ReLoadPlugins();
                    Logging.Write("Character's SpellBook fully updated. Found " + _spellBookID.Count + " spells, mounts and professions.");
                }
                catch (Exception exception)
                {
                    Logging.WriteError("UpdateSpellBook(): " + exception, true);
                }
                finally
                {
                    Memory.WowMemory.GameFrameUnLock();
                }
            }
        }

        public static List<SpellCooldownEntry> GetAllSpellsOnCooldown
        {
            get
            {
                BlackMagic memory = Memory.WowMemory.Memory;
                uint dwAddress = memory.ReadUInt((Memory.WowProcess.WowModule + 0xc8ac80) + 8);
                List<SpellCooldownEntry> list = new List<SpellCooldownEntry>();
                while ((dwAddress != 0) && ((dwAddress & 1) == 0))
                {
                    SpellCooldownEntry item = (SpellCooldownEntry) memory.ReadObject(dwAddress, typeof(SpellCooldownEntry));
                    list.Add(item);
                    dwAddress = memory.ReadUInt(dwAddress + 4);
                }
                return list;
            }
        }

        public static bool IsOnGlobalCooldown
        {
            get
            {
                foreach (SpellCooldownEntry entry in GetAllSpellsOnCooldown)
                {
                    if (entry.GCDDuration > 0)
                    {
                        int num2 = Usefuls.GetWoWTime - entry.GCDStartTime;
                        if (entry.GCDDuration >= num2)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpellCooldownEntry
        {
            public uint Previous;
            public uint Next;
            public uint SpellId;
            public uint ItemId;
            public int StartTime;
            public int SpellOrItemCooldownDuration;
            public uint SpellCategoryId;
            public int CategoryCooldownStartTime;
            public int CategoryCooldownDuration;
            public byte HasCooldown;
            public byte pad1;
            public byte pad2;
            public byte pad3;
            public int GCDStartTime;
            public uint StartRecoveryCategoryId;
            public int GCDDuration;
        }

        [StructLayout(LayoutKind.Explicit, Size=0x10)]
        private struct SpellInfo
        {
            [FieldOffset(4)]
            public readonly uint ID;
            [FieldOffset(8)]
            public readonly uint Level;
            [FieldOffset(0)]
            public readonly SpellState State;
            [FieldOffset(12)]
            public readonly uint TabId;

            public enum SpellState : uint
            {
                Flyout = 4,
                FutureSpell = 2,
                Known = 1,
                PetAction = 3
            }
        }

        public class SpellInfoLua
        {
            public int CastTime;
            public int Cost;
            public string Icon = "";
            public uint ID;
            public bool IsFunnel;
            public float MaxRange;
            public float MinRange;
            public string Name = "";
            public nManager.Wow.Enums.PowerType PowerType;
            public string Rank = "";
        }

        public class SpellListManager
        {
            private static readonly object LoadSpellListLock = new object();

            internal static void LoadSpellList(string fileName)
            {
                try
                {
                    lock (LoadSpellListLock)
                    {
                        if (ListSpell == null)
                        {
                            Dictionary<uint, string> dictionary = new Dictionary<uint, string>();
                            foreach (string str in Others.ReadFileAllLines(fileName))
                            {
                                if (!string.IsNullOrWhiteSpace(str) && str.Contains(";"))
                                {
                                    string[] strArray2 = str.Split(new char[] { ';' });
                                    if (((strArray2.Length == 2) && !string.IsNullOrWhiteSpace(strArray2[0])) && !string.IsNullOrWhiteSpace(strArray2[1]))
                                    {
                                        uint key = Others.ToUInt32(strArray2[0]);
                                        if (!dictionary.ContainsKey(key))
                                        {
                                            dictionary.Add(key, strArray2[1]);
                                        }
                                    }
                                }
                            }
                            ListSpell = dictionary;
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("LoadSpellListe(string fileName): " + exception, true);
                    if (ListSpell == null)
                    {
                        Logging.WriteError("ListSpell == null", true);
                    }
                    else
                    {
                        Logging.WriteError("ListSpell.Count = " + ListSpell.Count, true);
                    }
                }
            }

            public static List<uint> SpellIdByName(string spellName)
            {
                List<uint> list2;
                lock (SpellManager.SpellManagerLocker)
                {
                    List<uint> list = new List<uint>();
                    try
                    {
                        spellName = spellName.ToLower();
                        if (SpellManager.CacheSpellIdByName.TryGetValue(spellName, out list))
                        {
                            return list;
                        }
                        list = new List<uint>();
                        foreach (KeyValuePair<uint, string> pair in ListSpell)
                        {
                            if (pair.Value.ToLower() == spellName)
                            {
                                list.Add(pair.Key);
                            }
                        }
                        SpellManager.CacheSpellIdByName.Add(spellName, list);
                        list2 = list;
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("SpellIdByName(string spellName): " + exception, true);
                        list2 = list;
                    }
                }
                return list2;
            }

            public static void SpellIdByNameCreateCache()
            {
                lock (SpellManager.SpellManagerLocker)
                {
                    try
                    {
                        foreach (KeyValuePair<uint, string> pair in ListSpell)
                        {
                            string key = pair.Value.ToLower();
                            if (!SpellManager.CacheSpellIdByName.ContainsKey(key))
                            {
                                SpellManager.CacheSpellIdByName.Add(key, new List<uint> { pair.Key });
                            }
                            else if (!SpellManager.CacheSpellIdByName[key].Contains(pair.Key))
                            {
                                SpellManager.CacheSpellIdByName[key].Add(pair.Key);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("SpellIdByNameCreateCache(): " + exception, true);
                    }
                }
            }

            public static string SpellNameById(uint spellId)
            {
                try
                {
                    if (ListSpell.ContainsKey(spellId))
                    {
                        return ListSpell[spellId];
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("SpellNameById(UInt32 spellId): " + exception, true);
                }
                return "";
            }

            public static Dictionary<uint, string> ListSpell
            {
                [CompilerGenerated]
                get
                {
                    return <ListSpell>k__BackingField;
                }
                [CompilerGenerated]
                private set
                {
                    <ListSpell>k__BackingField = value;
                }
            }
        }
    }
}

