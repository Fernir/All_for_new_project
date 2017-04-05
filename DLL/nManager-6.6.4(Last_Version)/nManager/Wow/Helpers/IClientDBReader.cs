namespace nManager.Wow.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface IClientDBReader
    {
        void Save(DataTable table, Table def, string path);

        int FieldsCount { get; }

        string FileName { get; }

        bool IsSparseTable { get; }

        int RecordsCount { get; }

        int RecordSize { get; }

        IEnumerable<BinaryReader> Rows { get; }

        Dictionary<int, string> StringTable { get; }

        int StringTableSize { get; }
    }
}

