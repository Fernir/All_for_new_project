namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class Gossip
    {
        public static void CloseGossip()
        {
            Lua.LuaDoString("CloseGossip()", false, true);
        }

        public static void ExportTaxiInfo()
        {
            if ((nManager.Wow.ObjectManager.ObjectManager.Me.Target > 0) && nManager.Wow.ObjectManager.ObjectManager.Target.IsNpcFlightMaster)
            {
                Logging.WriteDebug(string.Concat(new object[] { "ExportTaxiInfo of NPC ", nManager.Wow.ObjectManager.ObjectManager.Target.Name, " (TaxiEntry: ", nManager.Wow.ObjectManager.ObjectManager.Target.Entry, ")" }));
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = NumTaxiNodes()", false, true);
                int num = Others.ToInt32(Lua.GetLocalizedText(randomString));
                if (num <= 0)
                {
                    Logging.WriteDebug("No TaxiNodes found, make sure to have the window opened and to know at least one path.");
                }
                else
                {
                    Logging.WriteDebug("Found " + num + " Taxi Path for this Flight Master.");
                    for (int i = 1; i <= num; i++)
                    {
                        string commandline = Others.GetRandomString(Others.Random(4, 10));
                        string str3 = Others.GetRandomString(Others.Random(4, 10));
                        string str4 = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(string.Concat(new object[] { str4, " = TaxiNodeName(", i, ");" }), false, true);
                        Lua.LuaDoString(string.Concat(new object[] { commandline, ",", str3, " = TaxiNodePosition(", i, ");" }), false, true);
                        string localizedText = Lua.GetLocalizedText(commandline);
                        string str6 = Lua.GetLocalizedText(str3);
                        string str7 = Lua.GetLocalizedText(str4);
                        Logging.WriteDebug(string.Concat(new object[] { "Slot ", i, ", Destination Name ", str7, ", px ", localizedText, ", py ", str6 }));
                    }
                }
            }
        }

        public static List<Taxi> GetAllTaxisAvailable()
        {
            List<Taxi> list = new List<Taxi>();
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = NumTaxiNodes()", false, true);
            int num = Others.ToInt32(Lua.GetLocalizedText(randomString));
            for (int i = 1; i <= num; i++)
            {
                string commandline = Others.GetRandomString(Others.Random(4, 10));
                string str3 = Others.GetRandomString(Others.Random(4, 10));
                string str4 = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { commandline, ",", str3, " = TaxiNodePosition(", i, "); ", str4, "= TaxiNodeGetType(", i, ");" }), false, true);
                string localizedText = Lua.GetLocalizedText(commandline);
                string str6 = Lua.GetLocalizedText(str3);
                string str7 = Lua.GetLocalizedText(str4);
                if ((str7 != "DISTANT") && (str7 != "NONE"))
                {
                    Taxi item = new Taxi {
                        Xcoord = localizedText,
                        Ycoord = str6
                    };
                    list.Add(item);
                }
                else if (str7 == "DISTANT")
                {
                    Logging.WriteDebug("Player is missing taxi X: " + localizedText + ", Y: " + str6);
                }
            }
            return list;
        }

        public static bool IsTaxiWindowOpen()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = tostring((TaxiFrame and TaxiFrame:IsVisible()) or (FlightMapFrame and FlightMapFrame:IsVisible()))", false, true);
            return (Lua.GetLocalizedText(randomString) == "true");
        }

        public static bool SelectGossip(GossipOption option)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumGossipOptions()", false, true);
            int num = Others.ToInt32(Lua.GetLocalizedText(randomString));
            if (num != 0)
            {
                int num2 = num;
                if (num > 1)
                {
                    string commandline = Others.GetRandomString(Others.Random(4, 10));
                    string str3 = Others.GetRandomString(Others.Random(4, 10));
                    string str4 = Others.GetRandomString(Others.Random(4, 10));
                    string str5 = Others.GetRandomString(Others.Random(4, 10));
                    string str8 = ("local " + str3 + " = { GetGossipOptions() } ") + commandline + " = 0 ";
                    string str9 = str8 + "for " + str4 + "," + str5 + " in pairs(" + str3 + ") do ";
                    Lua.LuaDoString((str9 + "if string.lower(" + str5 + ") == \"" + option.Value.ToLower() + "\" then " + commandline + " = " + str4 + "/2 ") + "end end", false, true);
                    num2 = Others.ToInt32(Lua.GetLocalizedText(commandline));
                }
                if (num2 == 0)
                {
                    Logging.WriteError("No gossip option " + ((string) option) + " available for this NPC", true);
                    return false;
                }
                Lua.LuaDoString("SelectGossipOption(" + num2 + ")", false, true);
                Thread.Sleep((int) (500 + Usefuls.Latency));
            }
            return true;
        }

        public static void TakeTaxi(string px, string py)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = NumTaxiNodes()", false, true);
            int num = Others.ToInt32(Lua.GetLocalizedText(randomString));
            for (int i = 1; i <= num; i++)
            {
                string commandline = Others.GetRandomString(Others.Random(4, 10));
                string str3 = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { commandline, ",", str3, " = TaxiNodePosition(", i, ")" }), false, true);
                string localizedText = Lua.GetLocalizedText(commandline);
                string str5 = Lua.GetLocalizedText(str3);
                if ((localizedText == px) && (str5 == py))
                {
                    Lua.LuaDoString("TakeTaxiNode(" + i + ")", false, true);
                    return;
                }
            }
        }

        public static void TrainAllAvailableSpells()
        {
            try
            {
                if (SelectGossip(GossipOption.Trainer))
                {
                    Lua.LuaDoString("SetTrainerServiceTypeFilter(\"available\",1);", false, true);
                    Lua.LuaDoString("SetTrainerServiceTypeFilter(\"unavailable\",0);", false, true);
                    Lua.LuaDoString("SetTrainerServiceTypeFilter(\"used\",0);", false, true);
                    Thread.Sleep(0x3e8);
                    Lua.LuaDoString("for i=0,GetNumTrainerServices(),1 do BuyTrainerService(1); end", false, true);
                    Thread.Sleep(500);
                    Lua.LuaDoString("CloseTrainer()", false, true);
                    Thread.Sleep(500);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("TrainingSpell(): " + exception, true);
            }
        }

        public sealed class GossipOption
        {
            public static readonly nManager.Wow.Helpers.Gossip.GossipOption Banker = new nManager.Wow.Helpers.Gossip.GossipOption("Banker");
            public static readonly nManager.Wow.Helpers.Gossip.GossipOption BattleMaster = new nManager.Wow.Helpers.Gossip.GossipOption("BattleMaster");
            public static readonly nManager.Wow.Helpers.Gossip.GossipOption Binder = new nManager.Wow.Helpers.Gossip.GossipOption("Binder");
            public static readonly nManager.Wow.Helpers.Gossip.GossipOption Gossip = new nManager.Wow.Helpers.Gossip.GossipOption("Gossip");
            public static readonly nManager.Wow.Helpers.Gossip.GossipOption Healer = new nManager.Wow.Helpers.Gossip.GossipOption("Healer");
            public static readonly nManager.Wow.Helpers.Gossip.GossipOption Petition = new nManager.Wow.Helpers.Gossip.GossipOption("Petition");
            public static readonly nManager.Wow.Helpers.Gossip.GossipOption Tabard = new nManager.Wow.Helpers.Gossip.GossipOption("Tabard");
            public static readonly nManager.Wow.Helpers.Gossip.GossipOption Taxi = new nManager.Wow.Helpers.Gossip.GossipOption("Taxi");
            public static readonly nManager.Wow.Helpers.Gossip.GossipOption Trainer = new nManager.Wow.Helpers.Gossip.GossipOption("Trainer");
            public static readonly nManager.Wow.Helpers.Gossip.GossipOption Unlearn = new nManager.Wow.Helpers.Gossip.GossipOption("Unlearn");
            public static readonly nManager.Wow.Helpers.Gossip.GossipOption Vendor = new nManager.Wow.Helpers.Gossip.GossipOption("Vendor");

            private GossipOption(string value)
            {
                this.Value = value;
            }

            public static implicit operator string(nManager.Wow.Helpers.Gossip.GossipOption go)
            {
                return go.Value;
            }

            public string Value { get; private set; }
        }
    }
}

