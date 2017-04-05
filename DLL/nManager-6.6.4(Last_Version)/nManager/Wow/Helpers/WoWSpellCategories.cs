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

    public class WoWSpellCategories
    {
        private static BinaryReader[] _afeheugucai;
        private static AnielOcoihij _piojauwai;
        private static SpellCategoriesDbcRecord[] _qiwukopSiaqio;
        [CompilerGenerated]
        private static SpellCategoriesDbcRecord _spellCategoriesDB2Record0;

        [CompilerGenerated]
        private static bool <Init>b__0(Table t)
        {
            return (t.Name == "SpellCategories");
        }

        [CompilerGenerated]
        private static bool <Init>b__1(Table t)
        {
            return (t.Name == Path.GetFileName("SpellCategories"));
        }

        private static void BamuodaFuhi()
        {
            if (_piojauwai == null)
            {
                DBFilesClient client = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                if (CS$<>9__CachedAnonymousMethodDelegate2 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate2 = new Func<Table, bool>(WoWSpellCategories.<Init>b__0);
                }
                IEnumerable<Table> source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate2);
                if (!source.Any<Table>())
                {
                    if (CS$<>9__CachedAnonymousMethodDelegate3 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate3 = new Func<Table, bool>(WoWSpellCategories.<Init>b__1);
                    }
                    source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate3);
                }
                if (source.Count<Table>() == 1)
                {
                    Table def = source.First<Table>();
                    _piojauwai = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\SpellCategories.db2", def) as AnielOcoihij;
                    if (((_afeheugucai == null) || (_qiwukopSiaqio == null)) && (_piojauwai != null))
                    {
                        _afeheugucai = _piojauwai.get_Rows().ToArray<BinaryReader>();
                        _qiwukopSiaqio = new SpellCategoriesDbcRecord[_afeheugucai.Length];
                        for (int i = 0; i < (_afeheugucai.Length - 1); i++)
                        {
                            _qiwukopSiaqio[i] = AnielOcoihij.Jejaebiniqeuf<SpellCategoriesDbcRecord>(_afeheugucai[i]);
                        }
                    }
                    if (_piojauwai != null)
                    {
                        Logging.Write(string.Concat(new object[] { _piojauwai.get_FileName(), " loaded with ", _piojauwai.get_RecordsCount(), " entries." }));
                    }
                }
                else
                {
                    Logging.Write("DB2 SpellCategories not read-able.");
                }
            }
        }

        public static uint GetSpellCategoryBySpellId(uint spellid)
        {
            BamuodaFuhi();
            for (int i = 0; i <= (_piojauwai.get_RecordsCount() - 1); i++)
            {
                _spellCategoriesDB2Record0 = _qiwukopSiaqio[i];
                if ((_spellCategoriesDB2Record0.m_spellID == spellid) || (_spellCategoriesDB2Record0.m_ID == spellid))
                {
                    return _spellCategoriesDB2Record0.m_spellCategoryID;
                }
            }
            return 0;
        }

        public static uint GetSpellStartRecoverCategoryBySpellId(uint spellid)
        {
            BamuodaFuhi();
            for (int i = 0; i <= (_piojauwai.get_RecordsCount() - 1); i++)
            {
                _spellCategoriesDB2Record0 = _qiwukopSiaqio[i];
                if (_spellCategoriesDB2Record0.m_spellID == spellid)
                {
                    return _spellCategoriesDB2Record0.m_StartRecoveryCategory;
                }
            }
            return 0;
        }

        public SpellCategoriesDbcRecord Record
        {
            get
            {
                return _spellCategoriesDB2Record0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpellCategoriesDbcRecord
        {
            public uint m_ID;
            public uint m_spellID;
            public uint m_spellCategoryID;
            public ushort m_StartRecoveryCategory;
            public ushort field0A;
            public ushort m_SpellDifficultyID;
            public byte m_DefenseType;
            public byte m_DispelType;
            public byte m_SpellMechanic;
            public byte m_PreventionType;
            public byte field12_0;
            public byte field12_1;
        }
    }
}

