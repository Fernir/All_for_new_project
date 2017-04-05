namespace nManager.Wow.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;

    internal class UruocuoraeXeowuhaxe : IEqualityComparer<DataRow>
    {
        public bool Equals(DataRow x, DataRow y)
        {
            object[] itemArray = x.ItemArray;
            object[] objArray2 = y.ItemArray;
            for (int i = 0; i < itemArray.Length; i++)
            {
                if ((this.get_IdColumnIndex() != i) && !itemArray[i].Equals(objArray2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(DataRow obj)
        {
            int num = 0;
            object[] itemArray = obj.ItemArray;
            for (int i = 0; i < itemArray.Length; i++)
            {
                if (this.get_IdColumnIndex() != i)
                {
                    num ^= itemArray[i].GetHashCode();
                }
            }
            return num;
        }

        public int _ecoibutucuosoeWa { get; set; }
    }
}

