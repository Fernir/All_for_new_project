namespace nManager.Wow.MemoryClass.Magic
{
    using System;
    using System.Diagnostics;

    public static class SPattern
    {
        public static uint FindPattern(byte[] bData, byte[] bPattern, string szMask)
        {
            if ((bData == null) || (bData.Length == 0))
            {
                throw new ArgumentNullException("bData");
            }
            if ((bPattern == null) || (bPattern.Length == 0))
            {
                throw new ArgumentNullException("bPattern");
            }
            if (szMask == string.Empty)
            {
                throw new ArgumentNullException("szMask");
            }
            if (bPattern.Length != szMask.Length)
            {
                throw new ArgumentException("Pattern and Mask lengths must be the same.");
            }
            bool flag = false;
            int length = bPattern.Length;
            int num4 = bData.Length - length;
            for (int i = 0; i < num4; i++)
            {
                flag = true;
                for (int j = 0; j < length; j++)
                {
                    if (((szMask[j] == 'x') && (bPattern[j] != bData[i + j])) || ((szMask[j] == '!') && (bPattern[j] == bData[i + j])))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return (uint) i;
                }
            }
            return 0;
        }

        public static uint FindPattern(IntPtr hProcess, ProcessModule pMod, byte[] bPattern, string szMask)
        {
            return FindPattern(hProcess, (uint) ((int) pMod.BaseAddress), pMod.ModuleMemorySize, bPattern, szMask);
        }

        public static uint FindPattern(IntPtr hProcess, ProcessModule[] pMods, byte[] bPattern, string szMask)
        {
            return FindPattern(hProcess, new ProcessModuleCollection(pMods), bPattern, szMask);
        }

        public static uint FindPattern(IntPtr hProcess, ProcessModuleCollection pMods, byte[] bPattern, string szMask)
        {
            uint num = 0;
            foreach (ProcessModule module in pMods)
            {
                num = FindPattern(hProcess, module, bPattern, szMask);
                if (num != 0)
                {
                    return num;
                }
            }
            return num;
        }

        public static uint FindPattern(IntPtr hProcess, ProcessModule pMod, string szPattern, string szMask, params char[] Delimiter)
        {
            return FindPattern(hProcess, (uint) ((int) pMod.BaseAddress), pMod.ModuleMemorySize, szPattern, szMask, Delimiter);
        }

        public static uint FindPattern(IntPtr hProcess, ProcessModule[] pMods, string szPattern, string szMask, params char[] Delimiter)
        {
            return FindPattern(hProcess, new ProcessModuleCollection(pMods), szPattern, szMask, Delimiter);
        }

        public static uint FindPattern(IntPtr hProcess, ProcessModuleCollection pMods, string szPattern, string szMask, params char[] Delimiter)
        {
            uint num = 0;
            foreach (ProcessModule module in pMods)
            {
                num = FindPattern(hProcess, module, szPattern, szMask, Delimiter);
                if (num != 0)
                {
                    return num;
                }
            }
            return num;
        }

        public static uint FindPattern(IntPtr hProcess, uint dwStart, int nSize, byte[] bPattern, string szMask)
        {
            if ((bPattern == null) || (bPattern.Length == 0))
            {
                throw new ArgumentNullException("bData");
            }
            if (bPattern.Length != szMask.Length)
            {
                throw new ArgumentException("bData and szMask must be of the same size");
            }
            byte[] bData = SMemory.ReadBytes(hProcess, dwStart, nSize);
            if (bData == null)
            {
                throw new Exception("Could not read memory in FindPattern.");
            }
            return (dwStart + FindPattern(bData, bPattern, szMask));
        }

        public static uint FindPattern(IntPtr hProcess, uint dwStart, int nSize, string szPattern, string szMask, params char[] Delimiter)
        {
            if (Delimiter == null)
            {
                Delimiter = new char[] { ' ' };
            }
            string[] strArray = szPattern.Split(Delimiter);
            byte[] bPattern = new byte[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                bPattern[i] = Convert.ToByte(strArray[i], 0x10);
            }
            return FindPattern(hProcess, dwStart, nSize, bPattern, szMask);
        }
    }
}

