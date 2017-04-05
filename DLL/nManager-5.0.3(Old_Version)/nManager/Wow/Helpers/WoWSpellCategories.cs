namespace nManager.Wow.Helpers
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class WoWSpellCategories
    {
        private static DBC<SpellCategoriesDbcRecord> spellCategoriesDBC;
        [CompilerGenerated]
        private static SpellCategoriesDbcRecord spellCategoriesDbcRecord_0;

        public static uint GetSpellCategoryBySpellId(uint spellid)
        {
            Init();
            for (int i = spellCategoriesDBC.MinIndex; i <= spellCategoriesDBC.MaxIndex; i++)
            {
                spellCategoriesDbcRecord_0 = spellCategoriesDBC.GetRow(i);
                if (spellCategoriesDbcRecord_0.m_spellID == spellid)
                {
                    return spellCategoriesDbcRecord_0.m_category;
                }
            }
            return 0;
        }

        public static uint GetSpellStartRecoverCategoryBySpellId(uint spellid)
        {
            for (int i = spellCategoriesDBC.MinIndex; i <= spellCategoriesDBC.MaxIndex; i++)
            {
                spellCategoriesDbcRecord_0 = spellCategoriesDBC.GetRow(i);
                if (spellCategoriesDbcRecord_0.m_spellID == spellid)
                {
                    return spellCategoriesDbcRecord_0.m_startRecoveryCategory;
                }
            }
            return 0;
        }

        private static void Init()
        {
            if (spellCategoriesDBC == null)
            {
                spellCategoriesDBC = new DBC<SpellCategoriesDbcRecord>(0xc6d85c);
            }
        }

        public SpellCategoriesDbcRecord Record
        {
            get
            {
                return spellCategoriesDbcRecord_0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpellCategoriesDbcRecord
        {
            public uint m_ID;
            public uint m_spellID;
            public uint m_difficultyID;
            public uint m_category;
            public uint m_defenseType;
            public uint m_dispelType;
            public uint m_mechanic;
            public uint m_preventionType;
            public uint m_startRecoveryCategory;
            public uint m_chargeCategory;
        }
    }
}

