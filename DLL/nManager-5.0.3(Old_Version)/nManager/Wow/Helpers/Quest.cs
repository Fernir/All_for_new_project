namespace nManager.Wow.Helpers
{
    using nManager;
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using nManager.Wow.Patchables;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Quest
    {
        public static int AbandonnedId = 0;
        public static List<int> FinishedQuestSet = new List<int>();
        public static List<int> KilledMobsToCount = new List<int>();

        static Quest()
        {
            GetSetIgnoreFight = false;
        }

        public static void AbandonQuest(int questId)
        {
            if (questId != 0)
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { randomString, " = GetQuestLogIndexByID(", questId, ")" }), false, true);
                int num = Others.ToInt32(Lua.GetLocalizedText(randomString));
                if (num > 0)
                {
                    Lua.LuaDoString("SelectQuestLogEntry(" + num + ") SetAbandonQuest() AbandonQuest()", false, true);
                    Thread.Sleep((int) (Usefuls.Latency + 500));
                }
            }
        }

        public static void AcceptQuest()
        {
            Lua.LuaDoString("AcceptQuest()", false, true);
            if (Others.IsFrameVisible("QuestFrameCompleteQuestButton"))
            {
                Lua.RunMacroText("/click QuestFrameCompleteQuestButton");
            }
        }

        public static void AutoCompleteQuest(List<int> autoComplete)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumAutoQuestPopUps();", false, true);
            int num = Others.ToInt32(Lua.GetLocalizedText(randomString));
            for (int i = 1; i <= num; i++)
            {
                string commandline = Others.GetRandomString(Others.Random(4, 10));
                string str3 = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { commandline, ", ", str3, " = GetAutoQuestPopUp(", i, ");" }), false, true);
                if (Lua.GetLocalizedText(str3) == "COMPLETE")
                {
                    int item = Others.ToInt32(Lua.GetLocalizedText(commandline));
                    if (autoComplete.Contains(item))
                    {
                        string str5 = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(string.Concat(new object[] { str5, " = GetQuestLogIndexByID(", item, ") " }) + "ShowQuestComplete(" + str5 + ")", false, true);
                        CompleteQuest();
                        Thread.Sleep(500);
                    }
                }
            }
        }

        public static void CloseQuestWindow()
        {
            Lua.LuaDoString("CloseQuest()", false, true);
            Lua.LuaDoString("CloseGossip()", false, true);
            Thread.Sleep(150);
        }

        public static nManager.Wow.Class.ItemInfo CompleteQuest()
        {
            nManager.Wow.Class.ItemInfo info2;
            Lua.LuaDoString("CompleteQuest()", false, true);
            Thread.Sleep(300);
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumQuestChoices()", false, true);
            int num = Others.ToInt32(Lua.GetLocalizedText(randomString));
            Logging.WriteDebug("There is " + num + " rewards");
            nManager.Wow.Class.ItemInfo info = null;
            uint num2 = 1;
            uint num3 = 0;
            uint num4 = 0;
            for (uint i = 1; i <= num; i++)
            {
                string commandline = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { commandline, " = GetQuestItemLink(\"choice\", ", i, "); " }), false, true);
                int num6 = ItemSelection.EvaluateItemStatsVsEquiped(Lua.GetLocalizedText(commandline), out info2);
                if (num6 > 0)
                {
                    if (num3 < num6)
                    {
                        num3 = (uint) num6;
                        num2 = i;
                    }
                }
                else
                {
                    num4 = i;
                    info = info2;
                    break;
                }
            }
            if (info == null)
            {
                randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = GetNumQuestRewards()", false, true);
                num = Others.ToInt32(Lua.GetLocalizedText(randomString));
                for (uint j = 1; j <= num; j++)
                {
                    string str5 = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(string.Concat(new object[] { str5, " = GetQuestItemLink(\"reward\", ", j, "); " }), false, true);
                    if (ItemSelection.EvaluateItemStatsVsEquiped(Lua.GetLocalizedText(str5), out info2) <= 0)
                    {
                        info = info2;
                    }
                }
            }
            if (num4 > 0)
            {
                Logging.WriteDebug("Going to select reward " + num4 + " for item stats");
                Lua.LuaDoString("GetQuestReward(" + num4 + ")", false, true);
            }
            else
            {
                Logging.WriteDebug("Going to select reward " + num2 + " for its money value");
                Lua.LuaDoString("GetQuestReward(" + num2 + ")", false, true);
            }
            Thread.Sleep(500);
            return info;
        }

        public static void ConsumeQuestsCompletedRequest()
        {
            FinishedQuestSet.Clear();
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            string commandline = Others.GetRandomString(Others.Random(4, 10));
            string str3 = "";
            string str6 = str3 + "local " + randomString + " = GetQuestsCompleted() ";
            string str7 = ((str6 + "if " + randomString + " == nil then " + commandline + " = \"NIL\" else ") + commandline + " = \"\" ") + "for key,value in pairs(" + randomString + ") do ";
            Lua.LuaDoString((str7 + commandline + " = " + commandline + " .. \"^\" .. key ") + "end end", false, true);
            string localizedText = Lua.GetLocalizedText(commandline);
            if (localizedText != "NIL")
            {
                foreach (string str5 in localizedText.Split(new char[] { '^' }))
                {
                    if (str5 != string.Empty)
                    {
                        FinishedQuestSet.Add(Others.ToInt32(str5));
                    }
                }
            }
        }

        public static string GetActiveTitle(int index)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(string.Concat(new object[] { randomString, " = GetActiveTitle(", index, ")" }), false, true);
            return Lua.GetLocalizedText(randomString);
        }

        public static string GetAvailableTitle(int index)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(string.Concat(new object[] { randomString, " = GetAvailableTitle(", index, ")" }), false, true);
            return Lua.GetLocalizedText(randomString);
        }

        public static bool GetGossipActiveQuestsWorks()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + ", _ = GetGossipActiveQuests()", false, true);
            return (Lua.GetLocalizedText(randomString) != "");
        }

        public static bool GetGossipAvailableQuestsWorks()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + ", _ = GetGossipAvailableQuests()", false, true);
            return (Lua.GetLocalizedText(randomString) != "");
        }

        public static List<int> GetLogQuestId()
        {
            try
            {
                uint num2 = Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + Descriptors.StartDescriptors) + 0x3a8;
                List<int> list = new List<int>();
                for (int i = 0; i < 0x19; i++)
                {
                    uint dwAddress = num2 + ((uint) (Marshal.SizeOf(typeof(PlayerQuest)) * i));
                    PlayerQuest quest = (PlayerQuest) Memory.WowMemory.Memory.ReadObject(dwAddress, typeof(PlayerQuest));
                    if (quest.ID > 0)
                    {
                        list.Add(quest.ID);
                    }
                }
                return list;
            }
            catch
            {
                return new List<int>();
            }
        }

        public static bool GetLogQuestIsComplete(int questId)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { "questIndex = GetQuestLogIndexByID(", questId, ");_, _, _, _, _, ", randomString, " = GetQuestLogTitle(questIndex);", randomString, " = tostring(", randomString, ")" }), false, true);
                return (Lua.GetLocalizedText(randomString) == "1");
            }
            catch
            {
                return false;
            }
        }

        public static string GetLogQuestTitle(int questId)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(string.Concat(new object[] { "questIndex = GetQuestLogIndexByID(", questId, ");", randomString, ", _ = GetQuestLogTitle(questIndex);" }), false, true);
            return Lua.GetLocalizedText(randomString);
        }

        public static int GetNumGossipActiveQuests()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumGossipActiveQuests()", false, true);
            return Others.ToInt32(Lua.GetLocalizedText(randomString));
        }

        public static int GetNumGossipAvailableQuests()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumGossipAvailableQuests()", false, true);
            return Others.ToInt32(Lua.GetLocalizedText(randomString));
        }

        public static int GetNumGossipOptions()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumGossipOptions()", false, true);
            return Others.ToInt32(Lua.GetLocalizedText(randomString));
        }

        public static bool GetQuestCompleted(List<int> qList)
        {
            foreach (int num in qList)
            {
                if (GetQuestCompleted(num))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool GetQuestCompleted(int questId)
        {
            return FinishedQuestSet.Contains(questId);
        }

        public static int GetQuestID()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetQuestID()", false, true);
            return Others.ToInt32(Lua.GetLocalizedText(randomString));
        }

        public static void InteractTarget(ref Npc npc, uint baseAddress)
        {
            WoWGameObject obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry(npc.Entry), npc.Position, false);
            Interact.InteractWith(baseAddress, false);
            if (obj2.IsValid)
            {
                Thread.Sleep(0xc1c);
            }
            else
            {
                Thread.Sleep((int) (Usefuls.Latency + 500));
                if ((nManager.Wow.ObjectManager.ObjectManager.Target.GetBaseAddress == 0) || (nManager.Wow.ObjectManager.ObjectManager.Target.GetBaseAddress != baseAddress))
                {
                    Logging.WriteDebug("Using LUA to target " + npc.Name);
                    Lua.LuaDoString("TargetUnit(\"" + npc.Name + "\")", false, true);
                    Thread.Sleep((int) (Usefuls.Latency + 500));
                    Interact.InteractWith(nManager.Wow.ObjectManager.ObjectManager.Target.GetBaseAddress, false);
                }
            }
        }

        public static bool IsObjectiveCompleted(int questId, uint ObjectiveInternalIndex, int count)
        {
            uint num2 = Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + Descriptors.StartDescriptors) + 0x3a8;
            for (int i = 0; i < 0x19; i++)
            {
                PlayerQuest quest = (PlayerQuest) Memory.WowMemory.Memory.ReadObject(num2 + ((uint) (Marshal.SizeOf(typeof(PlayerQuest)) * i)), typeof(PlayerQuest));
                if (quest.ID == questId)
                {
                    return (quest.ObjectiveRequiredCounts[(int) ((IntPtr) (ObjectiveInternalIndex - 1))] >= count);
                }
            }
            return false;
        }

        public static bool IsQuestFlaggedCompletedLUA(int internalQuestId)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(string.Concat(new object[] { randomString, " = tostring(IsQuestFlaggedCompleted(", internalQuestId, "))" }), false, true);
            return (Lua.GetLocalizedText(randomString) == "true");
        }

        public static void QuestPickUp(ref Npc npc, string questName, int questId)
        {
            bool flag;
            QuestPickUp(ref npc, questName, questId, out flag);
        }

        public static void QuestPickUp(ref Npc npc, string questName, int questId, out bool cancelPickUp)
        {
            cancelPickUp = false;
            if (AbandonnedId == questId)
            {
                AbandonnedId = 0;
            }
            else
            {
                if (AbandonnedId != 0)
                {
                    AbandonQuest(AbandonnedId);
                }
                AbandonnedId = 0;
                uint address = MovementManager.FindTarget(ref npc, 5f, true, true, 0f);
                WoWUnit unit = new WoWUnit(address);
                if (((unit.IsValid && (unit.UnitQuestGiverStatus != UnitQuestGiverStatus.Available)) && ((unit.UnitQuestGiverStatus != UnitQuestGiverStatus.AvailableRepeatable) && (unit.UnitQuestGiverStatus != UnitQuestGiverStatus.LowLevelAvailable))) && (unit.UnitQuestGiverStatus != UnitQuestGiverStatus.LowLevelAvailableRepeatable))
                {
                    nManagerSetting.AddBlackList(unit.Guid, 0xea60);
                    Logging.Write(string.Concat(new object[] { "Npc QuestGiver ", unit.Name, " (", unit.Entry, ", distance: ", unit.GetDistance, ") does not have any available quest for the moment. Blacklisting it one minute." }));
                    cancelPickUp = true;
                }
                else if (!MovementManager.InMovement)
                {
                    if (npc.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 6f)
                    {
                        if (address <= 0)
                        {
                            cancelPickUp = true;
                        }
                        InteractTarget(ref npc, address);
                        Logging.Write(string.Concat(new object[] { "PickUp Quest ", questName, " id: ", questId }));
                        int questID = GetQuestID();
                        if ((GetNumGossipAvailableQuests() == 0) && (questID == questId))
                        {
                            AcceptQuest();
                            Thread.Sleep((int) (Usefuls.Latency + 500));
                        }
                        if (GetLogQuestId().Contains(questId))
                        {
                            CloseQuestWindow();
                        }
                        else
                        {
                            bool gossipAvailableQuestsWorks = GetGossipAvailableQuestsWorks();
                            if (gossipAvailableQuestsWorks)
                            {
                                for (int i = 1; i <= GetNumGossipAvailableQuests(); i++)
                                {
                                    SelectGossipAvailableQuest(i);
                                    Thread.Sleep((int) (Usefuls.Latency + 500));
                                    questID = GetQuestID();
                                    if (questID == 0)
                                    {
                                        gossipAvailableQuestsWorks = false;
                                        break;
                                    }
                                    if (questID == questId)
                                    {
                                        AcceptQuest();
                                        Thread.Sleep((int) (Usefuls.Latency + 500));
                                        questID = GetQuestID();
                                        CloseQuestWindow();
                                        if (questID != questId)
                                        {
                                            AbandonQuest(questID);
                                        }
                                        break;
                                    }
                                    CloseQuestWindow();
                                    Thread.Sleep((int) (Usefuls.Latency + 500));
                                    AbandonQuest(questID);
                                    Interact.InteractWith(address, false);
                                    Thread.Sleep((int) (Usefuls.Latency + 500));
                                }
                            }
                            if (!gossipAvailableQuestsWorks)
                            {
                                for (int j = 1; GetAvailableTitle(j) != ""; j++)
                                {
                                    SelectAvailableQuest(j);
                                    Thread.Sleep((int) (Usefuls.Latency + 500));
                                    questID = GetQuestID();
                                    if (questID == questId)
                                    {
                                        AcceptQuest();
                                        Thread.Sleep((int) (Usefuls.Latency + 500));
                                        questID = GetQuestID();
                                        CloseQuestWindow();
                                        if (questID != questId)
                                        {
                                            AbandonQuest(questID);
                                        }
                                        break;
                                    }
                                    CloseQuestWindow();
                                    Thread.Sleep((int) (Usefuls.Latency + 500));
                                    AbandonQuest(questID);
                                    Interact.InteractWith(address, false);
                                    Thread.Sleep((int) (Usefuls.Latency + 500));
                                }
                            }
                        }
                        KilledMobsToCount.Clear();
                        Thread.Sleep(Usefuls.Latency);
                    }
                    Lua.LuaDoString("ClearTarget()", false, true);
                }
            }
        }

        public static void QuestTurnIn(ref Npc npc, string questName, int questId)
        {
            uint baseAddress = MovementManager.FindTarget(ref npc, 5f, true, false, 0f);
            if (!MovementManager.InMovement)
            {
                nManager.Wow.Class.ItemInfo item = null;
                if (npc.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 6f)
                {
                    InteractTarget(ref npc, baseAddress);
                    Logging.Write(string.Concat(new object[] { "turnIn Quest ", questName, " id: ", questId }));
                    int questID = GetQuestID();
                    if ((GetNumGossipActiveQuests() == 0) && (questID == questId))
                    {
                        item = CompleteQuest();
                        Thread.Sleep((int) (Usefuls.Latency + 500));
                    }
                    if (!GetLogQuestId().Contains(questId))
                    {
                        questID = GetQuestID();
                        FinishedQuestSet.Add(questId);
                        CloseQuestWindow();
                        AbandonnedId = questID;
                    }
                    else
                    {
                        bool gossipActiveQuestsWorks = GetGossipActiveQuestsWorks();
                        if (gossipActiveQuestsWorks)
                        {
                            for (int i = 1; i <= GetNumGossipActiveQuests(); i++)
                            {
                                SelectGossipActiveQuest(i);
                                Thread.Sleep((int) (Usefuls.Latency + 500));
                                questID = GetQuestID();
                                if (questID == 0)
                                {
                                    gossipActiveQuestsWorks = false;
                                    break;
                                }
                                if (questID == questId)
                                {
                                    item = CompleteQuest();
                                    Thread.Sleep((int) (Usefuls.Latency + 500));
                                    questID = GetQuestID();
                                    CloseQuestWindow();
                                    if (GetLogQuestId().Contains(questId))
                                    {
                                        item = null;
                                        Logging.WriteError(string.Concat(new object[] { "Could not turn-in quest ", questId, ": \"", questName, "\"" }), true);
                                    }
                                    else
                                    {
                                        FinishedQuestSet.Add(questId);
                                        AbandonnedId = questID;
                                    }
                                    break;
                                }
                                CloseQuestWindow();
                                Thread.Sleep((int) (Usefuls.Latency + 500));
                                Interact.InteractWith(baseAddress, false);
                                Thread.Sleep((int) (Usefuls.Latency + 500));
                            }
                        }
                        if (!gossipActiveQuestsWorks)
                        {
                            for (int j = 1; GetActiveTitle(j) != ""; j++)
                            {
                                SelectActiveQuest(j);
                                Thread.Sleep((int) (Usefuls.Latency + 500));
                                if (GetQuestID() == questId)
                                {
                                    item = CompleteQuest();
                                    Thread.Sleep((int) (Usefuls.Latency + 500));
                                    CloseQuestWindow();
                                    if (GetLogQuestId().Contains(questId))
                                    {
                                        item = null;
                                        Logging.WriteError(string.Concat(new object[] { "Could not turn-in quest ", questId, ": \"", questName, "\"" }), true);
                                    }
                                    else
                                    {
                                        FinishedQuestSet.Add(questId);
                                    }
                                    break;
                                }
                                CloseQuestWindow();
                                Thread.Sleep((int) (Usefuls.Latency + 500));
                                Interact.InteractWith(baseAddress, false);
                                Thread.Sleep((int) (Usefuls.Latency + 500));
                            }
                        }
                    }
                }
                Thread.Sleep(Usefuls.Latency);
                if (item != null)
                {
                    ItemSelection.EquipItem(item);
                    Thread.Sleep((int) (Usefuls.Latency + 500));
                }
                Lua.LuaDoString("ClearTarget()", false, true);
            }
        }

        public static void SelectActiveQuest(int index)
        {
            Lua.LuaDoString("SelectActiveQuest(" + index + ")", false, true);
        }

        public static void SelectAvailableQuest(int index)
        {
            Lua.LuaDoString("SelectAvailableQuest(" + index + ")", false, true);
        }

        public static void SelectGossipActiveQuest(int index)
        {
            Lua.LuaDoString("SelectGossipActiveQuest(" + index + ")", false, true);
        }

        public static void SelectGossipAvailableQuest(int GossipOption)
        {
            Lua.LuaDoString("SelectGossipAvailableQuest(" + GossipOption + ")", false, true);
        }

        public static void SelectGossipOption(int GossipOption)
        {
            Lua.LuaDoString("SelectGossipOption(" + GossipOption + ")", false, true);
        }

        public static bool GetSetIgnoreFight
        {
            [CompilerGenerated]
            get
            {
                return <GetSetIgnoreFight>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <GetSetIgnoreFight>k__BackingField = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PlayerQuest
        {
            public int ID;
            public StateFlag State;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x17)]
            public short[] ObjectiveRequiredCounts;
            public int Time;
            public enum StateFlag : uint
            {
                Complete = 1,
                Failed = 2,
                None = 0
            }
        }
    }
}

