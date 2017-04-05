namespace nManager.Helpful
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class IniFile
    {
        private readonly string _path;

        public IniFile(string iniPath)
        {
            try
            {
                this._path = iniPath;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IniFile(string iniPath): " + exception, true);
            }
        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public string IniReadValue(string section, string key)
        {
            try
            {
                StringBuilder retVal = new StringBuilder(0xff);
                GetPrivateProfileString(section, key, "", retVal, 0xff, this._path);
                return retVal.ToString();
            }
            catch (Exception exception)
            {
                Logging.WriteError("IniReadValue(string section, string key): " + exception, true);
            }
            return "";
        }

        public void IniWriteValue(string section, string key, string value)
        {
            try
            {
                WritePrivateProfileString(section, key, value, this._path);
            }
            catch (Exception exception)
            {
                Logging.WriteError("IniWriteValue(string section, string key, string value): " + exception, true);
            }
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    }
}

