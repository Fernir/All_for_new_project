namespace nManager.Wow.Helpers
{
    using nManager;
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class QuestersDB
    {
        private static List<Npc> _mufuwigo;

        public static void AddNpc(Npc npc)
        {
            try
            {
                List<Npc> npcList = new List<Npc> {
                    npc
                };
                AddNpcRange(npcList, false);
            }
            catch (Exception exception)
            {
                Logging.WriteError("QuestersDB > AddNpc(Npc npc): " + exception, true);
            }
        }

        public static int AddNpcRange(List<Npc> npcList, bool neutralIfPossible = false)
        {
            int num4;
            try
            {
                int num = 0;
                JakeugioxikNo();
                lock (typeof(QuestersDB))
                {
                    for (int i = 0; i < npcList.Count; i++)
                    {
                        Npc item = npcList[i];
                        if (!string.IsNullOrEmpty(item.Name))
                        {
                            bool flag = false;
                            bool flag2 = false;
                            Npc npc2 = new Npc();
                            for (int j = 0; j < ListNpc.Count; j++)
                            {
                                Npc npc3 = ListNpc[j];
                                if (((npc3.Entry == item.Entry) && (npc3.Type == item.Type)) && (npc3.Position.DistanceTo(item.Position) < 30f))
                                {
                                    flag = true;
                                    if ((npc3.Faction != item.Faction) && (npc3.Faction != Npc.FactionType.Neutral))
                                    {
                                        if (neutralIfPossible)
                                        {
                                            item.Faction = Npc.FactionType.Neutral;
                                        }
                                        npc2 = npc3;
                                        flag2 = true;
                                    }
                                    break;
                                }
                            }
                            if (flag && flag2)
                            {
                                ListNpc.Remove(npc2);
                                ListNpc.Add(item);
                                num++;
                            }
                            else if (!flag)
                            {
                                ListNpc.Add(item);
                                num++;
                            }
                        }
                    }
                    if (num != 0)
                    {
                        _mufuwigo.Sort(delegate (Npc x, Npc y) {
                            if (x.Entry >= y.Entry)
                            {
                                return 1;
                            }
                            return -1;
                        });
                        XmlSerializer.Serialize(Application.StartupPath + @"\Data\QuestersDB.xml", _mufuwigo);
                    }
                    num4 = num;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("QuestersDB > AddNpcRange(List<Npc> npcList)): " + exception, true);
                num4 = 0;
            }
            return num4;
        }

        public static void BuildNewList(List<Npc> npcList)
        {
            try
            {
                File.Delete(Application.StartupPath + @"\Data\QuestersDB.xml");
                ListNpc.Clear();
                lock (typeof(QuestersDB))
                {
                    foreach (Npc npc in npcList)
                    {
                        ListNpc.Add(npc);
                    }
                    Logging.Write("List builded with " + ListNpc.Count<Npc>() + "NPC inside.");
                    _mufuwigo.Sort(delegate (Npc x, Npc y) {
                        if (x.Entry >= y.Entry)
                        {
                            return 1;
                        }
                        return -1;
                    });
                    XmlSerializer.Serialize(Application.StartupPath + @"\Data\QuestersDB.xml", ListNpc);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("QuestersDB > BuildNewList(List<Npc> npcList)): " + exception, true);
            }
        }

        public static void DelNpc(Npc npc)
        {
            try
            {
                lock (typeof(QuestersDB))
                {
                    foreach (Npc npc2 in ListNpc)
                    {
                        if (((npc2.Entry == npc.Entry) && (npc2.Type == npc.Type)) && (npc2.Position.DistanceTo(npc.Position) < 1f))
                        {
                            ListNpc.Remove(npc2);
                            break;
                        }
                    }
                    if (CS$<>9__CachedAnonymousMethodDelegate4 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate4 = delegate (Npc x, Npc y) {
                            if (x.Entry >= y.Entry)
                            {
                                return 1;
                            }
                            return -1;
                        };
                    }
                    _mufuwigo.Sort(CS$<>9__CachedAnonymousMethodDelegate4);
                    XmlSerializer.Serialize(Application.StartupPath + @"\Data\QuestersDB.xml", _mufuwigo);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("QuestersDB > DelNpc(Npc npc): " + exception, true);
            }
        }

        public static Npc GetNpcByEntry(int entry)
        {
            foreach (Npc npc in ListNpc)
            {
                if (npc.Entry == entry)
                {
                    return npc;
                }
            }
            return new Npc();
        }

        public static Npc GetNpcNearby(Npc.NpcType type, bool ignoreRadiusSettings = false)
        {
            try
            {
                Npc.FactionType faction = (Npc.FactionType) Enum.Parse(typeof(Npc.FactionType), nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction);
                return GetNpcNearby(type, faction, Usefuls.ContinentId, nManager.Wow.ObjectManager.ObjectManager.Me.Position, ignoreRadiusSettings);
            }
            catch (Exception exception)
            {
                Logging.WriteError("QuestersDB > GetNpcNearby(Npc.NpcType type, bool ignoreRadiusSettings = false): " + exception, true);
                return new Npc();
            }
        }

        public static Npc GetNpcNearby(Npc.NpcType type, Npc.FactionType faction, int continentId, Point currentPosition, bool ignoreRadiusSettings = false)
        {
            try
            {
                Npc npc = new Npc();
                foreach (Npc npc2 in ListNpc)
                {
                    if ((((npc2.Faction == faction) || (npc2.Faction == Npc.FactionType.Neutral)) && (((npc2.Type == type) && (npc2.ContinentIdInt == continentId)) && ((npc.Position.DistanceTo(currentPosition) > npc2.Position.DistanceTo(currentPosition)) || (npc.Position.X == 0f)))) && (ignoreRadiusSettings || (npc2.Position.DistanceTo(currentPosition) <= nManagerSetting.CurrentSetting.MaxDistanceToGoToMailboxesOrNPCs)))
                    {
                        npc = npc2;
                    }
                }
                return npc;
            }
            catch (Exception exception)
            {
                Logging.WriteError("QuestersDB > GetNpcNearby(Npc.NpcType type, Npc.FactionType faction, Enums.ContinentId continentId, Point currentPosition, bool ignoreRadiusSettings = false): " + exception, true);
                return new Npc();
            }
        }

        public static Npc GetNpcNearbyByEntry(int entry)
        {
            try
            {
                Npc npc = new Npc();
                foreach (Npc npc2 in ListNpc)
                {
                    if ((npc2.Entry == entry) && ((npc.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > npc2.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position)) || !npc.Position.IsValid))
                    {
                        npc = npc2;
                    }
                }
                return npc;
            }
            catch (Exception exception)
            {
                Logging.WriteError("QuestersDB > GetNpcNearbyByEntry(int entry): " + exception, true);
                return new Npc();
            }
        }

        private static void JakeugioxikNo()
        {
            try
            {
                lock (typeof(QuestersDB))
                {
                    if (_mufuwigo == null)
                    {
                        _mufuwigo = XmlSerializer.Deserialize<List<Npc>>(Application.StartupPath + @"\Data\QuestersDB.xml");
                    }
                    if (_mufuwigo == null)
                    {
                        _mufuwigo = new List<Npc>();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("QuestersDB > LoadList(): " + exception, true);
            }
        }

        public static List<Npc> ListNpc
        {
            get
            {
                try
                {
                    JakeugioxikNo();
                    return _mufuwigo;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("QuestersDB > ListNpc get: " + exception, true);
                    return new List<Npc>();
                }
            }
            set
            {
                try
                {
                    JakeugioxikNo();
                    _mufuwigo = value;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("QuestersDB > ListNpc set: " + exception, true);
                }
            }
        }
    }
}

