namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Archaeology
    {
        private static List<Digsite> _allDigsiteZone = new List<Digsite>();
        private static List<int> _archaeologyItemsFindList;
        private static string _CrateRestoredArtifactSpellName;
        private static Spell archaeologySpell;
        public static bool ForceReloadDigsites = false;
        public static List<uint> SurveyList = new List<uint> { 0x2777, 0x2776, 0x2775 };

        public static void ClearList()
        {
            try
            {
                _allDigsiteZone = new List<Digsite>();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Archaeology > ClearList(): " + exception, true);
            }
        }

        public static void CrateRestoredArtifact()
        {
            foreach (WoWItem item in nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWItem())
            {
                Memory.WowMemory.GameFrameLock();
                if (ItemsManager.GetItemSpell(item.Entry) == _CrateRestoredArtifactSpellName)
                {
                    Memory.WowMemory.GameFrameUnLock();
                    ItemsManager.UseItem(item.Entry);
                    Thread.Sleep((int) (500 + Usefuls.Latency));
                    while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(150);
                    }
                }
            }
            Memory.WowMemory.GameFrameUnLock();
        }

        public static bool DigsiteZoneIsAvailable(Digsite digsitesZone)
        {
            try
            {
                List<DigsitesZoneLua> digsitesZoneLua = GetDigsitesZoneLua();
                if (digsitesZoneLua.Count > 0)
                {
                    foreach (DigsitesZoneLua lua in digsitesZoneLua)
                    {
                        if (digsitesZone.name == lua.name)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Archaeology > DigsiteZoneIsAvailable(DigsitesZone digsitesZone): " + exception, true);
            }
            return false;
        }

        public static List<Digsite> GenerateOrUpdate(List<Digsite> listDigsitesZoneFromXML)
        {
            bool flag = listDigsitesZoneFromXML != null;
            if (flag)
            {
                (from c in listDigsitesZoneFromXML
                    orderby c.id descending
                    select c).ToList<Digsite>();
            }
            List<Digsite> first = new List<Digsite>();
            WoWResearchSite site = WoWResearchSite.FromId(0);
            int minIndex = site.MinIndex;
            int maxIndex = site.MaxIndex;
            for (int i = minIndex; i <= maxIndex; i++)
            {
                site = WoWResearchSite.FromId(i);
                if (site.Record.Id != 0)
                {
                    Digsite item = new Digsite {
                        id = (int) site.Record.Id,
                        name = site.Name,
                        PriorityDigsites = 1f,
                        Active = true
                    };
                    first.Add(item);
                }
            }
            if (!flag)
            {
                return first;
            }
            return (from g in first.Concat<Digsite>(listDigsitesZoneFromXML).ToLookup<Digsite, int>(p => p.id) select g.Aggregate<Digsite>((p1, p2) => new Digsite { id = p1.id, name = p1.name, PriorityDigsites = p2.PriorityDigsites, Active = p2.Active })).ToList<Digsite>();
        }

        public static List<Digsite> GetAllDigsitesZone()
        {
            try
            {
                if ((_allDigsiteZone.Count <= 0) || ForceReloadDigsites)
                {
                    ForceReloadDigsites = false;
                    List<Digsite> listDigsitesZoneFromXML = new List<Digsite>();
                    try
                    {
                        Logging.Write("Loading ArchaeologistDigsites.xml");
                        listDigsitesZoneFromXML = XmlSerializer.Deserialize<List<Digsite>>(Application.StartupPath + @"\Data\ArchaeologistDigsites.xml");
                        listDigsitesZoneFromXML = GenerateOrUpdate(listDigsitesZoneFromXML);
                        XmlSerializer.Serialize(Application.StartupPath + @"\Data\ArchaeologistDigsites.xml", listDigsitesZoneFromXML);
                        listDigsitesZoneFromXML = (from c in listDigsitesZoneFromXML
                            orderby c.PriorityDigsites descending
                            select c).ToList<Digsite>();
                        Logging.Write(listDigsitesZoneFromXML.Count + " Archaeology Digsites Zones in the data base.");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("ArchaeologistDigsites.xml: " + exception);
                    }
                    _allDigsiteZone = listDigsitesZoneFromXML;
                }
                return _allDigsiteZone;
            }
            catch (Exception exception2)
            {
                Logging.WriteError("Archaeology > GetAllDigsitesZone(): " + exception2, true);
            }
            return new List<Digsite>();
        }

        public static List<Digsite> GetDigsitesZoneAvailable(string NameForSecond = null)
        {
            try
            {
                GetAllDigsitesZone();
                List<Digsite> list = new List<Digsite>();
                List<DigsitesZoneLua> digsitesZoneLua = GetDigsitesZoneLua();
                if (digsitesZoneLua.Count > 0)
                {
                    foreach (DigsitesZoneLua lua in digsitesZoneLua)
                    {
                        WoWResearchSite site;
                        bool flag = false;
                        if (NameForSecond == lua.name)
                        {
                            site = WoWResearchSite.FromName(lua.name, true);
                        }
                        else
                        {
                            site = WoWResearchSite.FromName(lua.name, false);
                        }
                        WoWQuestPOIPoint point = WoWQuestPOIPoint.FromSetId(site.Record.QuestIdPoint);
                        for (int i = 0; i <= (_allDigsiteZone.Count - 1); i++)
                        {
                            if ((_allDigsiteZone[i].id == site.Record.Id) && _allDigsiteZone[i].Active)
                            {
                                list.Add(_allDigsiteZone[i]);
                                Logging.Write(string.Concat(new object[] { "Digsite zone found: Name: ", lua.name, " - Distance = ", point.Center.DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) }));
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            Logging.Write("Digsite zone not found in database: Name=" + lua.name);
                        }
                    }
                }
                else
                {
                    Logging.Write("No digsites zones found, make sure 'Show Digsites' is activated in your map window.");
                    Thread.Sleep(500);
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Archaeology > GetDigsitesZoneAvailable(): " + exception, true);
                return new List<Digsite>();
            }
        }

        private static List<DigsitesZoneLua> GetDigsitesZoneLua()
        {
            try
            {
                string localizedText;
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                List<DigsitesZoneLua> list = new List<DigsitesZoneLua>();
                string command = "";
                command = ((((command + randomString + " = '' ") + "SetMapToCurrentZone() ") + "continent = GetCurrentMapContinent() " + "SetMapZoom(continent) ") + "local totalPOIs = GetNumMapLandmarks() " + "for index = 1 , totalPOIs do ") + "\tlocal name, description, textureIndex, px, py = GetMapLandmarkInfo(index) " + "\tif textureIndex == 177 then ";
                command = (command + "\t\t" + randomString + " = " + randomString + " .. name .. '#' ") + "\tend " + "end ";
                lock (typeof(Archaeology))
                {
                    Lua.LuaDoString(command, false, true);
                    localizedText = Lua.GetLocalizedText(randomString);
                }
                if (localizedText.Replace(" ", "").Length > 0)
                {
                    try
                    {
                        foreach (string str4 in localizedText.Split(new char[] { Others.ToChar("#") }))
                        {
                            if (str4.Replace(" ", "").Length > 0)
                            {
                                try
                                {
                                    string[] strArray2 = str4.Split(new char[] { Others.ToChar("^") });
                                    DigsitesZoneLua item = new DigsitesZoneLua {
                                        name = strArray2[0]
                                    };
                                    list.Add(item);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Archaeology > GetDigsitesZoneLua()#3: " + exception, true);
                return new List<DigsitesZoneLua>();
            }
        }

        public static void Initialize()
        {
            _allDigsiteZone = new List<Digsite>();
            Spell spell = new Spell(0x1efd7);
            _CrateRestoredArtifactSpellName = spell.NameInGame;
        }

        public static int SolveAllArtifact(bool useKeystone)
        {
            try
            {
                int num = 0;
                if (archaeologySpell == null)
                {
                    archaeologySpell = new Spell("Archaeology");
                }
                Lua.RunMacroText("/cast " + archaeologySpell.NameInGame);
                int num2 = 1;
                while (num2 <= 15)
                {
                    Lua.RunMacroText("/click ArchaeologyFrameSummaryButton");
                    if (num2 == 13)
                    {
                        Lua.RunMacroText("/click ArchaeologyFrameSummarytButton");
                        Lua.RunMacroText("/click ArchaeologyFrameSummaryPageNextPageButton");
                    }
                    if (num2 > 12)
                    {
                        Lua.RunMacroText("/click ArchaeologyFrameSummaryPageRace" + (num2 - 12));
                    }
                    else
                    {
                        Lua.RunMacroText("/click ArchaeologyFrameSummaryPageRace" + num2);
                    }
                    Thread.Sleep((int) (200 + Usefuls.Latency));
                    if (useKeystone)
                    {
                        bool flag;
                        do
                        {
                            flag = false;
                            string str = Others.GetRandomString(Others.Random(4, 10));
                            string str2 = Others.GetRandomString(Others.Random(4, 10));
                            Lua.LuaDoString(str + " = SocketItemToArtifact(); if " + str + " then " + str2 + "=\"1\" else " + str2 + "=\"0\" end", false, true);
                            if (Lua.GetLocalizedText(str2) == "1")
                            {
                                flag = true;
                                Thread.Sleep((int) (250 + Usefuls.Latency));
                            }
                        }
                        while (flag);
                    }
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    string commandline = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + " = CanSolveArtifact(); if " + randomString + " then " + commandline + "=\"1\" else " + commandline + "=\"0\" end", false, true);
                    if (Lua.GetLocalizedText(commandline) == "1")
                    {
                        num++;
                        Lua.RunMacroText("/click ArchaeologyFrameArtifactPageSolveFrameSolveButton");
                        if (Usefuls.GetContainerNumFreeSlots >= 1)
                        {
                            num2--;
                        }
                        Thread.Sleep((int) (0x802 + Usefuls.Latency));
                    }
                    num2++;
                    Thread.Sleep((int) (200 + Usefuls.Latency));
                }
                Lua.RunMacroText("/click ArchaeologyFrameCloseButton");
                if ((nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 90) && (Usefuls.GetContainerNumFreeSlots > 0))
                {
                    for (int i = 0x13818; i <= 0x1382d; i++)
                    {
                        if ((i != 0x13822) && (i != 0x13823))
                        {
                            WoWItem woWItemById = nManager.Wow.ObjectManager.ObjectManager.GetWoWItemById(i);
                            if ((((woWItemById != null) && woWItemById.IsValid) && !ItemsManager.IsItemOnCooldown(i)) && ItemsManager.IsItemUsable(i))
                            {
                                goto Label_0296;
                            }
                        }
                        continue;
                    Label_0279:
                        MountTask.DismountMount(true);
                        ItemsManager.UseItem(i);
                        Thread.Sleep((int) (0x60e + Usefuls.Latency));
                    Label_0296:
                        if (ItemsManager.GetItemCount(i) > 0)
                        {
                            goto Label_0279;
                        }
                    }
                    for (int j = 0x1748f; j <= 0x17496; j++)
                    {
                        WoWItem item2 = nManager.Wow.ObjectManager.ObjectManager.GetWoWItemById(j);
                        if ((((item2 != null) && item2.IsValid) && !ItemsManager.IsItemOnCooldown(j)) && ItemsManager.IsItemUsable(j))
                        {
                            goto Label_02FF;
                        }
                        continue;
                    Label_02E2:
                        MountTask.DismountMount(true);
                        ItemsManager.UseItem(j);
                        Thread.Sleep((int) (0x60e + Usefuls.Latency));
                    Label_02FF:
                        if (ItemsManager.GetItemCount(j) > 0)
                        {
                            goto Label_02E2;
                        }
                    }
                }
                return num;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Archaeology >  solveAllArtifact(): " + exception, true);
                return 0;
            }
        }

        public static List<int> ArchaeologyItemsFindList
        {
            get
            {
                try
                {
                    if (_archaeologyItemsFindList == null)
                    {
                        _archaeologyItemsFindList = new List<int>();
                        foreach (string str in Others.ReadFileAllLines(Application.StartupPath + @"\Data\archaeologyFind.txt"))
                        {
                            if (!string.IsNullOrWhiteSpace(str))
                            {
                                _archaeologyItemsFindList.Add(Others.ToInt32(str));
                            }
                        }
                    }
                    return _archaeologyItemsFindList;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Archaeology > ArchaeologyItemsFindList : " + exception, true);
                    return new List<int>();
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DigsitesZoneLua
        {
            public string name;
        }
    }
}

