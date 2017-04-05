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

    public class WoWItemClass
    {
        private static AnielOcoihij _ciodo;
        private ItemClassDB2Record _obesovoleUku;

        private WoWItemClass(string name)
        {
            BamuodaFuhi();
            ItemClassDB2Record record = new ItemClassDB2Record();
            bool flag = false;
            for (int i = 0; i < (_ciodo.get_RecordsCount() - 1); i++)
            {
                record = AnielOcoihij.Jejaebiniqeuf<ItemClassDB2Record>(_ciodo.get_Rows().ElementAt<BinaryReader>(i));
                if (record.Name() == name)
                {
                    flag = true;
                    break;
                }
            }
            this._obesovoleUku = flag ? record : new ItemClassDB2Record();
        }

        [CompilerGenerated]
        private static bool <Init>b__0(Table t)
        {
            return (t.Name == "ItemClass");
        }

        [CompilerGenerated]
        private static bool <Init>b__1(Table t)
        {
            return (t.Name == Path.GetFileName("ItemClass"));
        }

        private static void BamuodaFuhi()
        {
            if (_ciodo == null)
            {
                DBFilesClient client = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                if (CS$<>9__CachedAnonymousMethodDelegate2 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate2 = new Func<Table, bool>(WoWItemClass.<Init>b__0);
                }
                IEnumerable<Table> source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate2);
                if (!source.Any<Table>())
                {
                    if (CS$<>9__CachedAnonymousMethodDelegate3 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate3 = new Func<Table, bool>(WoWItemClass.<Init>b__1);
                    }
                    source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate3);
                }
                if (source.Count<Table>() == 1)
                {
                    Table def = source.First<Table>();
                    _ciodo = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\ItemClass.db2", def) as AnielOcoihij;
                    Logging.Write(string.Concat(new object[] { _ciodo.get_FileName(), " loaded with ", _ciodo.get_RecordsCount(), " entries." }));
                }
                else
                {
                    Logging.Write("DB2 ItemClass not read-able.");
                }
            }
        }

        public static WoWItemClass FromName(string name)
        {
            return new WoWItemClass(name);
        }

        public string Name
        {
            get
            {
                return this._obesovoleUku.Name();
            }
        }

        public ItemClassDB2Record Record
        {
            get
            {
                return this._obesovoleUku;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemClassDB2Record
        {
            public uint ClassId;
            public uint PointerToStringTable;
            public uint ClassNameOffset;
            public uint lastByte;
            public string Name()
            {
                string str;
                if ((WoWItemClass._ciodo.get_StringTable() != null) && WoWItemClass._ciodo.get_StringTable().TryGetValue((int) this.ClassNameOffset, out str))
                {
                    return str;
                }
                return "";
            }
        }
    }
}

