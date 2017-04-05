namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class WoWResearchSite
    {
        private static BinaryReader[] _poviaxaq;
        private static ResearchSiteDb2Record[] _qiwukopSiaqio;
        private static AnielOcoihij _uwaque;
        private ResearchSiteDb2Record _vuoceoj;

        private WoWResearchSite(int reqId)
        {
            Awoup();
            ResearchSiteDb2Record record = new ResearchSiteDb2Record();
            bool flag = false;
            for (int i = 0; i < (_uwaque.get_RecordsCount() - 1); i++)
            {
                record = _qiwukopSiaqio[i];
                if (record.Id == reqId)
                {
                    flag = true;
                    break;
                }
            }
            this._vuoceoj = flag ? record : new ResearchSiteDb2Record();
        }

        private WoWResearchSite(string name, bool SecondOne = false)
        {
            bool flag = false;
            Awoup();
            ResearchSiteDb2Record record = new ResearchSiteDb2Record();
            bool flag2 = false;
            for (int i = 0; i < (_uwaque.get_RecordsCount() - 1); i++)
            {
                record = _qiwukopSiaqio[i];
                if ((record.Name() == name) && (record.Map == Usefuls.ContinentId))
                {
                    if (SecondOne && !flag)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag2 = true;
                        break;
                    }
                }
            }
            this._vuoceoj = flag2 ? record : new ResearchSiteDb2Record();
        }

        [CompilerGenerated]
        private static bool <init>b__0(Table t)
        {
            return (t.Name == "ResearchSite");
        }

        [CompilerGenerated]
        private static bool <init>b__1(Table t)
        {
            return (t.Name == Path.GetFileName("ResearchSite"));
        }

        private static void Awoup()
        {
            if (_uwaque == null)
            {
                DBFilesClient client = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                if (CS$<>9__CachedAnonymousMethodDelegate2 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate2 = new Func<Table, bool>(WoWResearchSite.<init>b__0);
                }
                IEnumerable<Table> source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate2);
                if (!source.Any<Table>())
                {
                    if (CS$<>9__CachedAnonymousMethodDelegate3 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate3 = new Func<Table, bool>(WoWResearchSite.<init>b__1);
                    }
                    source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate3);
                }
                if (source.Count<Table>() == 1)
                {
                    Table def = source.First<Table>();
                    if (Usefuls.GetClientLanguage().Length == 4)
                    {
                        _uwaque = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\" + Usefuls.GetClientLanguage() + @"\ResearchSite.db2", def) as AnielOcoihij;
                    }
                    else
                    {
                        _uwaque = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\AllWoW\ResearchSite.db2", def) as AnielOcoihij;
                    }
                    if (((_poviaxaq == null) || (_qiwukopSiaqio == null)) && (_uwaque != null))
                    {
                        _poviaxaq = _uwaque.get_Rows().ToArray<BinaryReader>();
                        _qiwukopSiaqio = new ResearchSiteDb2Record[_poviaxaq.Length];
                        for (int i = 0; i < (_poviaxaq.Length - 1); i++)
                        {
                            _qiwukopSiaqio[i] = AnielOcoihij.Jejaebiniqeuf<ResearchSiteDb2Record>(_poviaxaq[i]);
                        }
                    }
                    Logging.Write(string.Concat(new object[] { _uwaque.get_FileName(), " loaded with ", _uwaque.get_RecordsCount(), " entries." }));
                }
                else
                {
                    Logging.Write("DB2 ResearchSite not read-able.");
                }
            }
        }

        public static List<ResearchSiteDb2Record> ExtractAllDigsites()
        {
            List<ResearchSiteDb2Record> list = new List<ResearchSiteDb2Record>();
            Awoup();
            ResearchSiteDb2Record item = new ResearchSiteDb2Record();
            for (int i = 0; i < (_uwaque.get_RecordsCount() - 1); i++)
            {
                item = _qiwukopSiaqio[i];
                list.Add(item);
            }
            return list;
        }

        public static WoWResearchSite FromId(int id)
        {
            return new WoWResearchSite(id);
        }

        public static WoWResearchSite FromName(string name, bool secondOne = false)
        {
            return new WoWResearchSite(name, secondOne);
        }

        public int MaxIndex
        {
            get
            {
                Awoup();
                return _uwaque.get_MaxId();
            }
        }

        public int MinIndex
        {
            get
            {
                Awoup();
                return _uwaque.get_MinId();
            }
        }

        public string Name
        {
            get
            {
                return this._vuoceoj.Name();
            }
        }

        public ResearchSiteDb2Record Record
        {
            get
            {
                return this._vuoceoj;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ResearchSiteDb2Record
        {
            public uint Id;
            public uint QuestIdPoint;
            public uint NameOffset;
            public ushort Map;
            public byte textureIndex;
            public byte Unk2;
            public string Name()
            {
                string str;
                if ((WoWResearchSite._uwaque.get_StringTable() != null) && WoWResearchSite._uwaque.get_StringTable().TryGetValue((int) this.NameOffset, out str))
                {
                    return str;
                }
                return "";
            }
        }
    }
}

