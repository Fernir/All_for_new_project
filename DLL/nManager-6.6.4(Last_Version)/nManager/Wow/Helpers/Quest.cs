namespace nManager.Wow.Helpers
{
    using nManager;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.ObjectManager;
    using nManager.Wow.Patchables;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Quest
    {
        private static bool _ecocuhuawaujov = false;
        private static Point _iqodounotUhaigieca = null;
        private static object _udeuraek = new object();
        public static int AbandonnedId = 0;
        public static List<int> FinishedQuestSet = new List<int>();
        public static List<int> KilledMobsToCount = new List<int>();

        static Quest()
        {
            GetSetIgnoreFight = false;
            GetSetIgnoreAllFight = false;
            GetSetDismissPet = false;
            MountTask.AllowMounting = true;
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

        public static void AutoCompleteQuest()
        {
            lock (_udeuraek)
            {
                for (int i = 1; i < 20; i++)
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    string commandline = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(string.Concat(new object[] { randomString, ", ", commandline, " = GetAutoQuestPopUp(", i, ");" }), false, true);
                    string localizedText = Lua.GetLocalizedText(commandline);
                    int item = Others.ToInt32(Lua.GetLocalizedText(randomString));
                    if (((item == 0) && string.IsNullOrEmpty(localizedText)) && (i > 10))
                    {
                        break;
                    }
                    if ((item != 0) || !string.IsNullOrEmpty(localizedText))
                    {
                        if (localizedText == "COMPLETE")
                        {
                            if (GetLogQuestId().Contains(item))
                            {
                                string str4 = Others.GetRandomString(Others.Random(4, 10));
                                Lua.LuaDoString(string.Concat(new object[] { str4, " = GetQuestLogIndexByID(", item, "); " }) + "ShowQuestComplete(" + str4 + ");", false, true);
                                Thread.Sleep(300);
                                if (Others.IsFrameVisible("QuestFrameCompleteButton") && !Others.IsFrameVisible("QuestFrameCompleteQuestButton"))
                                {
                                    Lua.RunMacroText("/click QuestFrameCompleteButton");
                                    Thread.Sleep(300);
                                }
                                CompleteQuest();
                                Thread.Sleep(500);
                                if (!GetLogQuestId().Contains(item))
                                {
                                    FinishedQuestSet.Add(item);
                                }
                            }
                        }
                        else if (((localizedText == "OFFER") && !GetLogQuestId().Contains(item)) && (!GetLogQuestIsComplete(item) && !IsQuestFlaggedCompletedLUA(item)))
                        {
                            string str6 = Others.GetRandomString(Others.Random(4, 10));
                            Lua.LuaDoString(string.Concat(new object[] { str6, " = GetQuestLogIndexByID(", item, "); " }) + "ShowQuestOffer(" + str6 + ");", false, true);
                            Thread.Sleep(300);
                            if (Others.IsFrameVisible("QuestFrameAcceptButton") && !Others.IsFrameVisible("QuestFrameAcceptQuestButton"))
                            {
                                Lua.RunMacroText("/click QuestFrameAcceptButton");
                                Thread.Sleep(300);
                            }
                            AcceptQuest();
                            Thread.Sleep(500);
                        }
                    }
                }
            }
        }

        public static void CloseWindow()
        {
            try
            {
                Memory.WowMemory.GameFrameLock();
                Lua.LuaDoString("CloseQuest()", false, true);
                Lua.LuaDoString("CloseGossip()", false, true);
                Lua.LuaDoString("CloseBankFrame()", false, true);
                Lua.LuaDoString("CloseMail()", false, true);
                Lua.LuaDoString("CloseMerchant()", false, true);
                Lua.LuaDoString("ClosePetStables()", false, true);
                Lua.LuaDoString("CloseTaxiMap()", false, true);
                Lua.LuaDoString("CloseTrainer()", false, true);
                Lua.LuaDoString("CloseAuctionHouse()", false, true);
                Lua.LuaDoString("CloseGuildBankFrame()", false, true);
                Lua.RunMacroText("/Click QuestFrameCloseButton");
                Lua.LuaDoString("ClearTarget()", false, true);
                Thread.Sleep(150);
            }
            catch (Exception exception)
            {
                Logging.WriteError("public static void CloseWindow(): " + exception, true);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
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

        public static void DumpInternalIndexForQuestId(int questId)
        {
            Logging.Write("Dumping current InternalIndexes for QuestId: " + questId);
            uint num2 = Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + Descriptors.StartDescriptors) + 940;
            for (int i = 0; i < 50; i++)
            {
                PlayerQuest quest = (PlayerQuest) Memory.WowMemory.Memory.ReadObject(num2 + ((uint) (Marshal.SizeOf(typeof(PlayerQuest)) * i)), typeof(PlayerQuest));
                if (quest.ID == questId)
                {
                    for (int j = 0; j <= (quest.ObjectiveRequiredCounts.Length - 1); j++)
                    {
                        if (quest.ObjectiveRequiredCounts[j] > 0)
                        {
                            Logging.Write(string.Concat(new object[] { "InternalIndex: ", j + 1, ", Count: ", quest.ObjectiveRequiredCounts[j] }));
                        }
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

        public static int GetCurrentInternalIndexCount(int questId, uint ObjectiveInternalIndex)
        {
            uint num2 = Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + Descriptors.StartDescriptors) + 940;
            for (int i = 0; i < 50; i++)
            {
                PlayerQuest quest = (PlayerQuest) Memory.WowMemory.Memory.ReadObject(num2 + ((uint) (Marshal.SizeOf(typeof(PlayerQuest)) * i)), typeof(PlayerQuest));
                if (quest.ID == questId)
                {
                    return quest.ObjectiveRequiredCounts[(int) ((IntPtr) (ObjectiveInternalIndex - 1))];
                }
            }
            return 0;
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
                uint num2 = Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + Descriptors.StartDescriptors) + 940;
                List<int> list = new List<int>();
                for (int i = 0; i < 50; i++)
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
                if (num == -1)
                {
                    return false;
                }
                if (GetQuestCompleted(num))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool GetQuestCompleted(int questId)
        {
            if (questId == -1)
            {
                return false;
            }
            return FinishedQuestSet.Contains(questId);
        }

        public static int GetQuestID()
        {
            int num = Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xfcf6dc);
            if (num > 0)
            {
                return num;
            }
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

        public static bool IsNearQuestGiver(Point p)
        {
            return (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(p) <= 5f);
        }

        public static bool IsObjectiveCompleted(int questId, uint ObjectiveInternalIndex, int count)
        {
            uint num2 = Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + Descriptors.StartDescriptors) + 940;
            for (int i = 0; i < 50; i++)
            {
                PlayerQuest quest = (PlayerQuest) Memory.WowMemory.Memory.ReadObject(num2 + ((uint) (Marshal.SizeOf(typeof(PlayerQuest)) * i)), typeof(PlayerQuest));
                if (quest.ID == questId)
                {
                    return (quest.ObjectiveRequiredCounts[(int) ((IntPtr) (ObjectiveInternalIndex - 1))] >= count);
                }
            }
            return false;
        }

        public static bool IsQuestFailed(int questId)
        {
            uint num2 = Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + Descriptors.StartDescriptors) + 940;
            for (int i = 0; i < 50; i++)
            {
                PlayerQuest quest = (PlayerQuest) Memory.WowMemory.Memory.ReadObject(num2 + ((uint) (Marshal.SizeOf(typeof(PlayerQuest)) * i)), typeof(PlayerQuest));
                if (quest.ID == questId)
                {
                    return (quest.State == PlayerQuest.StateFlag.Failed);
                }
            }
            return false;
        }

        public static bool IsQuestFlaggedCompletedLUA(int internalQuestId)
        {
            if (internalQuestId <= 0)
            {
                return true;
            }
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(string.Concat(new object[] { randomString, " = tostring(IsQuestFlaggedCompleted(", internalQuestId, "))" }), false, true);
            return (Lua.GetLocalizedText(randomString) == "true");
        }

        public static void QuestPickUp(ref Npc npc, string questName, int questId, bool ignoreBlacklist = false, bool forceTravel = false)
        {
            bool flag;
            QuestPickUp(ref npc, questName, questId, out flag, ignoreBlacklist, forceTravel);
        }

        public static void QuestPickUp(ref Npc npc, string questName, int questId, out bool cancelPickUp, bool ignoreBlacklist = false, bool forceTravel = false)
        {
            if (npc.ForceTravel)
            {
                forceTravel = true;
            }
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
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(npc.Entry, true), false, ignoreBlacklist, true);
                if (unit.IsValid && unit.HasQuests)
                {
                    npc.Position = unit.Position;
                }
                WoWGameObject nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry(npc.Entry), ignoreBlacklist);
                if (nearestWoWGameObject.IsValid && nearestWoWGameObject.HasQuests)
                {
                    npc.Position = nearestWoWGameObject.Position;
                }
                bool resultSuccess = false;
                if (position.DistanceTo(npc.Position) <= 800f)
                {
                    PathFinder.FindPath(npc.Position, out resultSuccess, true, false);
                }
                if (Usefuls.IsFlying && (npc.ContinentIdInt == Usefuls.ContinentId))
                {
                    resultSuccess = true;
                }
                else if (Usefuls.IsFlying)
                {
                    MountTask.DismountMount(true);
                }
                if ((!resultSuccess && ((_iqodounotUhaigieca == null) || (_iqodounotUhaigieca.DistanceTo(position) > 0.1f))) && !_ecocuhuawaujov)
                {
                    MovementManager.StopMove();
                    Logging.Write(string.Concat(new object[] { "Calling travel system for PickUpQuest ", questName, "(", questId, ") from ", npc.Name, " (", npc.Entry, ")..." }));
                    nManager.Products.Products.TravelToContinentId = npc.ContinentIdInt;
                    nManager.Products.Products.TravelTo = npc.Position;
                    nManager.Products.Products.TravelFromContinentId = Usefuls.ContinentId;
                    nManager.Products.Products.TravelFrom = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                    nManager.Products.Products.ForceTravel = forceTravel;
                    nManager.Products.Products.TargetValidationFct = new Func<Point, bool>(Quest.IsNearQuestGiver);
                    _iqodounotUhaigieca = position;
                }
                else
                {
                    if ((_iqodounotUhaigieca != null) && (_iqodounotUhaigieca.DistanceTo(position) <= 0.1f))
                    {
                        _ecocuhuawaujov = true;
                    }
                    uint baseAddress = MovementManager.FindTarget(ref npc, 5f, true, true, 0f, ignoreBlacklist);
                    if (!MovementManager.InMovement)
                    {
                        _ecocuhuawaujov = false;
                        if (baseAddress > 0)
                        {
                            WoWObject objectByGuid = nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(npc.Guid);
                            if (objectByGuid is WoWUnit)
                            {
                                WoWUnit unit2 = objectByGuid as WoWUnit;
                                if ((unit2.IsValid && (unit2.GetDistance < 20f)) && !unit2.HasQuests)
                                {
                                    _ecocuhuawaujov = false;
                                    nManagerSetting.AddBlackList(unit2.Guid, 0x7530);
                                    Logging.Write(string.Concat(new object[] { "Npc QuestGiver ", unit2.Name, " (", unit2.Entry, ", distance: ", unit2.GetDistance, ") does not have any available quest for the moment. Blacklisting it for 30 seconds." }));
                                    cancelPickUp = true;
                                    return;
                                }
                            }
                        }
                        if (npc.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 6f)
                        {
                            if (baseAddress <= 0)
                            {
                                cancelPickUp = true;
                            }
                            InteractTarget(ref npc, baseAddress);
                            Logging.Write(string.Concat(new object[] { "PickUpQuest ", questName, " (", questId, ") from ", npc.Name, " (", npc.Entry, ")" }));
                            int questID = GetQuestID();
                            if ((GetNumGossipAvailableQuests() == 0) && (questID == questId))
                            {
                                AcceptQuest();
                                Thread.Sleep((int) (Usefuls.Latency + 500));
                            }
                            if (GetLogQuestId().Contains(questId))
                            {
                                CloseWindow();
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
                                            CloseWindow();
                                            if (questID != questId)
                                            {
                                                AbandonQuest(questID);
                                            }
                                            break;
                                        }
                                        CloseWindow();
                                        Thread.Sleep((int) (Usefuls.Latency + 500));
                                        AbandonQuest(questID);
                                        Interact.InteractWith(baseAddress, false);
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
                                            CloseWindow();
                                            if (questID != questId)
                                            {
                                                AbandonQuest(questID);
                                            }
                                            break;
                                        }
                                        CloseWindow();
                                        Thread.Sleep((int) (Usefuls.Latency + 500));
                                        AbandonQuest(questID);
                                        Interact.InteractWith(baseAddress, false);
                                        Thread.Sleep((int) (Usefuls.Latency + 500));
                                    }
                                }
                            }
                            KilledMobsToCount.Clear();
                            Thread.Sleep(Usefuls.Latency);
                        }
                        CloseWindow();
                    }
                }
            }
        }

        public static void QuestTurnIn(ref Npc npc, string questName, int questId, bool ignoreBlacklist = false, bool forceTravel = false)
        {
            if (npc.ForceTravel)
            {
                forceTravel = true;
            }
            Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
            WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(npc.Entry, true), false, ignoreBlacklist, true);
            if (unit.IsValid && unit.CanTurnIn)
            {
                npc.Position = unit.Position;
            }
            WoWGameObject nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry(npc.Entry), ignoreBlacklist);
            if (nearestWoWGameObject.IsValid && nearestWoWGameObject.CanTurnIn)
            {
                npc.Position = nearestWoWGameObject.Position;
            }
            bool resultSuccess = false;
            if (position.DistanceTo(npc.Position) <= 800f)
            {
                PathFinder.FindPath(npc.Position, out resultSuccess, true, false);
            }
            if (Usefuls.IsFlying && (npc.ContinentIdInt == Usefuls.ContinentId))
            {
                resultSuccess = true;
            }
            else if (Usefuls.IsFlying)
            {
                MountTask.DismountMount(true);
            }
            if ((!resultSuccess && ((_iqodounotUhaigieca == null) || (_iqodounotUhaigieca.DistanceTo(position) > 0.1f))) && !_ecocuhuawaujov)
            {
                MovementManager.StopMove();
                Logging.Write(string.Concat(new object[] { "Calling travel system for TurnInQuest ", questName, "(", questId, ") from ", npc.Name, " (", npc.Entry, ")..." }));
                nManager.Products.Products.TravelToContinentId = npc.ContinentIdInt;
                nManager.Products.Products.TravelTo = npc.Position;
                nManager.Products.Products.TravelFromContinentId = Usefuls.ContinentId;
                nManager.Products.Products.TravelFrom = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                nManager.Products.Products.ForceTravel = forceTravel;
                nManager.Products.Products.TargetValidationFct = new Func<Point, bool>(Quest.IsNearQuestGiver);
                _iqodounotUhaigieca = position;
            }
            else
            {
                if ((_iqodounotUhaigieca != null) && (_iqodounotUhaigieca.DistanceTo(position) <= 0.1f))
                {
                    _ecocuhuawaujov = true;
                }
                uint baseAddress = MovementManager.FindTarget(ref npc, 5f, true, true, 0f, ignoreBlacklist);
                if (!MovementManager.InMovement)
                {
                    if (baseAddress > 0)
                    {
                        WoWObject objectByGuid = nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(npc.Guid);
                        if (objectByGuid is WoWUnit)
                        {
                            WoWUnit unit2 = objectByGuid as WoWUnit;
                            if ((unit2.IsValid && (unit2.GetDistance < 20f)) && !unit2.CanTurnIn)
                            {
                                _ecocuhuawaujov = false;
                                nManagerSetting.AddBlackList(unit2.Guid, 0x7530);
                                Logging.Write(string.Concat(new object[] { "Npc QuestGiver ", unit2.Name, " (", unit2.Entry, ", distance: ", unit2.GetDistance, ") cannot TurnIn any quest right now. Blacklisting it for 30 seconds." }));
                                return;
                            }
                        }
                    }
                    _ecocuhuawaujov = false;
                    nManager.Wow.Class.ItemInfo item = null;
                    if (npc.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 6f)
                    {
                        InteractTarget(ref npc, baseAddress);
                        Logging.Write(string.Concat(new object[] { "QuestTurnIn ", questName, " (", questId, ") to ", npc.Name, " (", npc.Entry, ")" }));
                        int questID = GetQuestID();
                        if ((GetNumGossipActiveQuests() == 0) && (questID == questId))
                        {
                            if (Others.IsFrameVisible("QuestFrameCompleteButton") && !Others.IsFrameVisible("QuestFrameCompleteQuestButton"))
                            {
                                Lua.RunMacroText("/click QuestFrameCompleteButton");
                                Thread.Sleep(300);
                            }
                            item = CompleteQuest();
                            Thread.Sleep((int) (Usefuls.Latency + 500));
                        }
                        if (!GetLogQuestId().Contains(questId))
                        {
                            questID = GetQuestID();
                            FinishedQuestSet.Add(questId);
                            CloseWindow();
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
                                        if (Others.IsFrameVisible("QuestFrameCompleteButton") && !Others.IsFrameVisible("QuestFrameCompleteQuestButton"))
                                        {
                                            Lua.RunMacroText("/click QuestFrameCompleteButton");
                                            Thread.Sleep(300);
                                        }
                                        item = CompleteQuest();
                                        Thread.Sleep((int) (Usefuls.Latency + 500));
                                        questID = GetQuestID();
                                        CloseWindow();
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
                                    CloseWindow();
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
                                        if (Others.IsFrameVisible("QuestFrameCompleteButton") && !Others.IsFrameVisible("QuestFrameCompleteQuestButton"))
                                        {
                                            Lua.RunMacroText("/click QuestFrameCompleteButton");
                                            Thread.Sleep(300);
                                        }
                                        item = CompleteQuest();
                                        Thread.Sleep((int) (Usefuls.Latency + 500));
                                        CloseWindow();
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
                                    CloseWindow();
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
                    CloseWindow();
                }
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

        public static bool TravelToQuestZone(Point destination, ref bool travelToQuestZone, int continentId = -1, bool forceTravel = false, string reason = "TravelToQuestZone")
        {
            bool resultSuccess = forceTravel;
            if (continentId == -1)
            {
                continentId = Usefuls.ContinentId;
            }
            if (((continentId != Usefuls.ContinentId) || !Usefuls.IsFlying) || forceTravel)
            {
                if (((continentId == Usefuls.ContinentId) && Usefuls.IsFlying) && forceTravel)
                {
                    MountTask.DismountMount(true);
                }
                if (continentId != Usefuls.ContinentId)
                {
                    resultSuccess = true;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(destination) > 800f)
                {
                    resultSuccess = true;
                }
                else if (!resultSuccess)
                {
                    PathFinder.FindPath(destination, out resultSuccess, true, false);
                    resultSuccess = !resultSuccess;
                }
                if (!travelToQuestZone && !forceTravel)
                {
                    resultSuccess = false;
                }
                if ((resultSuccess && ((_iqodounotUhaigieca == null) || (_iqodounotUhaigieca.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 0.1f))) && !_ecocuhuawaujov)
                {
                    MovementManager.StopMove();
                    Logging.Write("Calling travel system for " + reason + "...");
                    travelToQuestZone = false;
                    nManager.Products.Products.TravelToContinentId = continentId;
                    nManager.Products.Products.TravelTo = destination;
                    nManager.Products.Products.TravelFromContinentId = Usefuls.ContinentId;
                    nManager.Products.Products.TravelFrom = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                    nManager.Products.Products.ForceTravel = forceTravel;
                    nManager.Products.Products.TargetValidationFct = new Func<Point, bool>(Quest.IsNearQuestGiver);
                    _iqodounotUhaigieca = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                    return true;
                }
                if ((_iqodounotUhaigieca != null) && (_iqodounotUhaigieca.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= 0.1f))
                {
                    _ecocuhuawaujov = true;
                }
            }
            return false;
        }

        public static bool GetSetDismissPet
        {
            [CompilerGenerated]
            get
            {
                return <GetSetDismissPet>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <GetSetDismissPet>k__BackingField = value;
            }
        }

        public static bool GetSetIgnoreAllFight
        {
            [CompilerGenerated]
            get
            {
                return <GetSetIgnoreAllFight>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <GetSetIgnoreAllFight>k__BackingField = value;
            }
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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x19)]
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

