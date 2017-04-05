namespace nManager.Helpful
{
    using nManager;
    using nManager.Products;
    using nManager.Wow;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Management;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    public static class Others
    {
        private static string _ahiweifuepean;
        private static int _muduajaxBavuto = -1;
        private static string _puaxoejeaIpir = "";
        private static bool _samixuaHeot;
        private static string _teunoatisiefiIgeomik = "";
        private static readonly nManager.Helpful.Timer CachedAuthServerTimer = new nManager.Helpful.Timer(300.0);
        private static readonly string[] FailOversAddress = new string[] { "http://tech.thenoobbot.com/" };
        public static readonly Dictionary<int, int> ItemStock = new Dictionary<int, int>();
        public static EventHandler ItemStockUpdated;
        private static readonly object LockerDownload = new object();
        public static Dictionary<string, long> LUAVariableToDestruct = new Dictionary<string, long>();
        private static readonly UTF8Encoding Utf8 = new UTF8Encoding();

        public static string ArrayToTextByLine(string[] array)
        {
            try
            {
                string str = "";
                foreach (string str2 in array)
                {
                    str = str + str2 + Environment.NewLine;
                }
                return str;
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("ArrayToTextByLine(string[] array): " + exception, true);
                return "";
            }
        }

        public static void CheckInventoryForLatestLoot(int eventFireCount)
        {
            lock (ItemStock)
            {
                try
                {
                    if (_muduajaxBavuto != eventFireCount)
                    {
                        _muduajaxBavuto = eventFireCount;
                        Dictionary<int, int> dictionary = new Dictionary<int, int>();
                        bool flag = ItemStock.Count == 0;
                        List<WoWItem> objectWoWItem = nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWItem();
                        foreach (WoWItem item in objectWoWItem)
                        {
                            WoWItem item2 = item;
                            if ((item2.IsValid && (item2.Entry >= 1)) && (!dictionary.ContainsKey(item2.Entry) && !EquippedItems.IsEquippedItemByGuid(item2.Guid)))
                            {
                                int itemCount = ItemsManager.GetItemCount(item2.Entry);
                                if (itemCount != 0)
                                {
                                    if (!ItemStock.ContainsKey(item2.Entry))
                                    {
                                        dictionary.Add(item2.Entry, itemCount);
                                        ItemStock.Add(item2.Entry, itemCount);
                                    }
                                    else if (ItemStock[item2.Entry] != itemCount)
                                    {
                                        if (ItemStock[item2.Entry] < itemCount)
                                        {
                                            dictionary.Add(item2.Entry, itemCount - ItemStock[item2.Entry]);
                                            ItemStock[item2.Entry] = itemCount;
                                        }
                                        else if (ItemStock[item2.Entry] > itemCount)
                                        {
                                            ItemStock[item2.Entry] = itemCount;
                                        }
                                    }
                                }
                            }
                        }
                        if (!flag)
                        {
                            foreach (KeyValuePair<int, int> pair in dictionary)
                            {
                                if (ItemStockUpdated != null)
                                {
                                    ItemStockUpdated(pair, new EventArgs());
                                }
                                nManager.Helpful.Logging.Write(string.Concat(new object[] { "You receive loot: ", ItemsManager.GetItemNameById(pair.Key), "(", pair.Key, ") x", pair.Value }));
                            }
                        }
                        List<int> list2 = new List<int>();
                        foreach (KeyValuePair<int, int> pair2 in ItemStock)
                        {
                            bool flag2 = false;
                            foreach (WoWItem item3 in objectWoWItem)
                            {
                                if (item3.Entry == pair2.Key)
                                {
                                    flag2 = true;
                                }
                            }
                            if (!flag2)
                            {
                                list2.Add(pair2.Key);
                            }
                        }
                        foreach (int num2 in list2)
                        {
                            ItemStock.Remove(num2);
                        }
                        dictionary.Clear();
                        objectWoWItem.Clear();
                        list2.Clear();
                    }
                }
                catch (Exception exception)
                {
                    nManager.Helpful.Logging.WriteError("CheckInventoryForLatestLoot(int eventFireCount): " + exception, true);
                }
            }
        }

        public static string DecryptString(string encryptedString)
        {
            try
            {
                string[] strArray = encryptedString.Split(new char[] { '-' });
                List<byte> list = new List<byte>();
                int num = HardDriveID();
                for (int i = 0; i <= (strArray.Length - 1); i++)
                {
                    list.Add(Convert.ToByte((int) ((ToInt32(strArray[i]) - num) - 15)));
                }
                byte[] bytes = list.ToArray();
                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("EncryptStringToString(string encryptText): " + exception, true);
            }
            return "";
        }

        public static void DeleteFile(string strPath)
        {
            try
            {
                System.IO.File.Delete(strPath);
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("DeleteFile(string strPath): " + exception, true);
            }
        }

        public static string DelSpecialChar(string stringSpecialChar)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (char ch in stringSpecialChar)
                {
                    if ((ch >= 'A') && ((ch <= 'z') || ((ch <= 'Z') && (ch >= 'a'))))
                    {
                        list.Add(Convert.ToString(ch));
                    }
                }
                return string.Concat((IEnumerable<string>) list);
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("DelSpecialChar(string stringSpecialChar): " + exception, true);
                return "";
            }
        }

        public static string DialogBoxOpenFile(string path, string typeFile)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog {
                    InitialDirectory = path,
                    Filter = typeFile
                };
                dialog.ShowDialog();
                return dialog.FileName;
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("DialogBoxOpenFile(string path, string typeFile): " + exception, true);
            }
            return "";
        }

        public static string[] DialogBoxOpenFileMultiselect(string path, string typeFile)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog {
                    InitialDirectory = path,
                    Filter = typeFile,
                    Multiselect = true
                };
                dialog.ShowDialog();
                return dialog.FileNames;
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("DialogBoxOpenFile(string path, string typeFile): " + exception, true);
            }
            return new string[0];
        }

        public static string DialogBoxSaveFile(string path, string typeFile)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog {
                    InitialDirectory = path,
                    Filter = typeFile
                };
                dialog.ShowDialog();
                return dialog.FileName;
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("DialogBoxSaveFile(string path, string typeFile): " + exception, true);
            }
            return "";
        }

        public static bool DownloadFile(string httpUrl, string fileDest)
        {
            bool flag2;
            try
            {
                lock (LockerDownload)
                {
                    _puaxoejeaIpir = httpUrl;
                    _teunoatisiefiIgeomik = fileDest;
                    Thread thread2 = new Thread(new ThreadStart(Others.OvetuoruoloOfa)) {
                        Name = "DownloadFile"
                    };
                    thread2.Start();
                    Thread.Sleep(200);
                    while (_samixuaHeot)
                    {
                        Application.DoEvents();
                        Thread.Sleep(100);
                    }
                    if (ExistFile(fileDest))
                    {
                        return true;
                    }
                    flag2 = false;
                }
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("DownloadFile(string httpUrl, string fileDest): " + exception, true);
                _samixuaHeot = false;
                flag2 = false;
            }
            return flag2;
        }

        public static string EncrypterMD5(string value)
        {
            try
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(value));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(buffer[i].ToString("x2"));
                }
                return builder.ToString();
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("EncrypterMD5(string value): " + exception, true);
            }
            return "";
        }

        public static string EncryptString(string text)
        {
            try
            {
                string str = "";
                string s = text;
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                int num = HardDriveID();
                for (int i = 0; i <= (bytes.Length - 1); i++)
                {
                    if (str != "")
                    {
                        str = str + "-";
                    }
                    str = str + ((bytes[i] + num) + 15);
                }
                return str;
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("StringToEncryptString(string text): " + exception, true);
            }
            return "";
        }

        public static bool ExistFile(string strPath)
        {
            try
            {
                return System.IO.File.Exists(strPath);
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("ExistFile(string strPath): " + exception, true);
            }
            return false;
        }

        public static string GetFileMd5CheckSum(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    FileStream inputStream = null;
                    try
                    {
                        try
                        {
                            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
                            inputStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read);
                            byte[] buffer = provider.ComputeHash(inputStream);
                            string str = "";
                            foreach (byte num in buffer)
                            {
                                if (num < 0x10)
                                {
                                    str = str + "0" + num.ToString("X");
                                }
                                else
                                {
                                    str = str + num.ToString("X");
                                }
                            }
                            return str;
                        }
                        catch (Exception exception)
                        {
                            nManager.Helpful.Logging.WriteError("GetFileMd5CheckSum(string filePath)#1: " + exception, true);
                        }
                        goto Label_00C2;
                    }
                    finally
                    {
                        if (inputStream != null)
                        {
                            inputStream.Close();
                        }
                    }
                }
                return "";
            }
            catch (Exception exception2)
            {
                nManager.Helpful.Logging.WriteError("GetFileMd5CheckSum(string filePath)#2: " + exception2, true);
            }
        Label_00C2:
            return "";
        }

        public static List<string> GetFilesDirectory(string pathDirectory, string searchPattern = "")
        {
            string startupPath = "";
            if (!pathDirectory.Contains(":"))
            {
                startupPath = Application.StartupPath;
            }
            if (!Directory.Exists(startupPath + pathDirectory))
            {
                return new List<string>();
            }
            return Directory.GetFiles(startupPath + pathDirectory, searchPattern).Select<string, string>(delegate (string subfolder) {
                string fileName = Path.GetFileName(subfolder);
                if (fileName == null)
                {
                    return null;
                }
                return fileName.ToString(CultureInfo.InvariantCulture);
            }).ToList<string>();
        }

        public static string GetRandomString(int maxSize)
        {
            try
            {
                char[] chArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                byte[] data = new byte[1];
                RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
                provider.GetNonZeroBytes(data);
                data = new byte[maxSize];
                provider.GetNonZeroBytes(data);
                StringBuilder builder = new StringBuilder(maxSize);
                foreach (byte num in data)
                {
                    builder.Append(chArray[num % (chArray.Length - 1)]);
                }
                lock (LUAVariableToDestruct)
                {
                    if (!LUAVariableToDestruct.ContainsKey(builder.ToString()))
                    {
                        LUAVariableToDestruct.Add(builder.ToString(), (long) Environment.TickCount);
                    }
                    else
                    {
                        LUAVariableToDestruct[builder.ToString()] = Environment.TickCount;
                    }
                }
                return builder.ToString();
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("GetRandomString(int maxSize): " + exception, true);
            }
            return "abcdef";
        }

        public static string GetRequest(string url, string data)
        {
            HttpWebResponse response = null;
            StreamReader reader = null;
            string str;
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    url = url + "?" + data;
                }
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.UserAgent = "TheNoobBot";
                response = (HttpWebResponse) request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("iso-8859-1"));
                str = reader.ReadToEnd();
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("GetRequest(string url, string data): " + exception, true);
                str = "";
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return str;
        }

        public static List<string> GetReqWithAuthHeader(string url, string userName, string userPassword)
        {
            try
            {
                WebRequest request = WebRequest.Create(ToUtf8(url));
                if ((userName != "") && (userPassword != ""))
                {
                    string s = ToUtf8(userName) + ":" + ToUtf8(userPassword);
                    s = Convert.ToBase64String(Encoding.Default.GetBytes(s));
                    request.Headers["Authorization"] = "Basic " + s;
                }
                ((HttpWebRequest) request).UserAgent = "TheNoobBot";
                WebResponse response = request.GetResponse();
                string text = "";
                if ((userName != "") && (userPassword != ""))
                {
                    text = response.Headers.Get("retn");
                }
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    string str3 = new StreamReader(responseStream).ReadToEnd();
                    return new List<string> { ToUtf8(text), ToUtf8(str3) };
                }
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("GetReqWithAuthHeader(string url, String userName, String userPassword): " + exception, true);
            }
            return new List<string> { "", "" };
        }

        [DllImport("kernel32")]
        private static extern int GetTickCount();
        public static void GetVisualStudioRedistribuable2013()
        {
            try
            {
                if (System.IO.File.Exists(Environment.SystemDirectory + @"\mfc120.dll"))
                {
                    return;
                }
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("GetVisualStudioRedistribuable2013() #1: " + exception, true);
            }
            try
            {
                if (MessageBox.Show(Translate.Get(Translate.Id.VisualStudioRedistribuablePackages), "Visual Studio 2013 Redistribuable Package " + Translate.Get(Translate.Id.Required), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Process.Start("http://www.microsoft.com/download/details.aspx?id=40784");
                }
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception exception2)
            {
                nManager.Helpful.Logging.WriteError("GetVisualStudioRedistribuable2013() #2: " + exception2, true);
            }
        }

        public static int HardDriveID()
        {
            try
            {
                ManagementObject obj2 = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
                obj2.Get();
                return obj2["VolumeSerialNumber"].ToString().Sum<char>(((Func<char, int>) (c => Convert.ToInt32(c))));
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("HardDriveID(): " + exception, true);
                return 0;
            }
        }

        public static bool IsFrameVisible(string frameName)
        {
            string randomString = GetRandomString(Random(4, 10));
            Lua.LuaDoString(randomString + " = tostring(" + frameName + " and " + frameName + ":IsVisible())", false, true);
            return (Lua.GetLocalizedText(randomString) == "true");
        }

        public static void LoginToWoW()
        {
            Login.SettingsLogin settings = new Login.SettingsLogin {
                Login = nManagerSetting.AutoStartEmail,
                Password = nManagerSetting.AutoStartPassword,
                Realm = nManagerSetting.AutoStartRealmName,
                Character = nManagerSetting.AutoStartCharacter,
                BNetName = nManagerSetting.AutoStartBattleNet
            };
            nManager.Helpful.Logging.Write("Begin player logging with informations provided.");
            Login.Pulse(settings);
            if (Usefuls.InGame && !Usefuls.IsLoading)
            {
                Thread.Sleep(0x1388);
                if (Usefuls.InGame && !Usefuls.IsLoading)
                {
                    nManager.Helpful.Logging.Write("Ending player logging with success.");
                    ConfigWowForThisBot.ConfigWow();
                    if ((nManager.Products.Products.ProductName == "Damage Dealer") && !nManagerSetting.CurrentSetting.ActivateMovementsDamageDealer)
                    {
                        ConfigWowForThisBot.StartStopClickToMove(false);
                    }
                    if ((nManager.Products.Products.ProductName == "Heal Bot") && nManagerSetting.CurrentSetting.ActivateMovementsHealerBot)
                    {
                        ConfigWowForThisBot.StartStopClickToMove(false);
                    }
                    SpellManager.UpdateSpellBook();
                }
            }
        }

        public static void LootStatistics(bool startOrStop = true)
        {
            if (startOrStop)
            {
                nManager.Helpful.Logging.Write("Initializing LootStatistics module, may take few seconds.");
                CheckInventoryForLatestLoot(0);
                EventsListener.HookEvent(WoWEventsType.CHAT_MSG_LOOT, callBack => CheckInventoryForLatestLoot((int) callBack), true, false);
            }
            else
            {
                EventsListener.UnHookEvent(WoWEventsType.CHAT_MSG_LOOT, callBack => CheckInventoryForLatestLoot((int) callBack), true);
            }
        }

        public static void LUAGlobalVarDestructor()
        {
            if (Usefuls.InGame && !Usefuls.IsLoading)
            {
                string str = "";
                if (LUAVariableToDestruct.Count > 0)
                {
                    Dictionary<string, long> dictionary = new Dictionary<string, long>();
                    lock (LUAVariableToDestruct)
                    {
                        foreach (KeyValuePair<string, long> pair in LUAVariableToDestruct)
                        {
                            if (Regex.IsMatch(pair.Key, "^[a-zA-Z]+$"))
                            {
                                if ((pair.Value + 500L) < Environment.TickCount)
                                {
                                    str = str + pair.Key + " = nil; ";
                                }
                                else
                                {
                                    dictionary.Add(pair.Key, pair.Value);
                                }
                            }
                        }
                        LUAVariableToDestruct.Clear();
                        LUAVariableToDestruct = dictionary;
                    }
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        Lua.LuaDoString(str, false, true);
                    }
                }
            }
        }

        public static void OpenWebBrowserOrApplication(string urlOrPath)
        {
            try
            {
                Process process = new Process();
                ProcessStartInfo info = new ProcessStartInfo {
                    FileName = urlOrPath
                };
                process.StartInfo = info;
                process.Start();
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("OpenWebBrowser(string url): " + exception, true);
            }
        }

        private static void OvetuoruoloOfa()
        {
            try
            {
                _samixuaHeot = true;
                try
                {
                    new WebClient().DownloadFile(_puaxoejeaIpir, _teunoatisiefiIgeomik);
                }
                catch (Exception exception)
                {
                    nManager.Helpful.Logging.WriteError("DownloadThread()#1: " + exception, true);
                }
                _samixuaHeot = false;
            }
            catch (Exception exception2)
            {
                nManager.Helpful.Logging.WriteError("DownloadThread()#2: " + exception2, true);
            }
        }

        public static string PostRequest(string url, string parameters)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                ((HttpWebRequest) request).UserAgent = "TheNoobBot";
                byte[] bytes = Encoding.UTF8.GetBytes(parameters);
                Stream requestStream = null;
                try
                {
                    request.ContentLength = bytes.Length;
                    requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                catch (WebException exception)
                {
                    nManager.Helpful.Logging.WriteError("PostRequest(string url, string parameters)#1: " + exception, true);
                }
                finally
                {
                    if (requestStream != null)
                    {
                        requestStream.Close();
                    }
                }
                try
                {
                    StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                    return reader.ReadToEnd().Trim();
                }
                catch (WebException exception2)
                {
                    nManager.Helpful.Logging.WriteError("PostRequest(string url, string parameters)#2: " + exception2.Message, true);
                    MessageBox.Show(exception2.Message, "HttpPost: Response error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception exception3)
            {
                nManager.Helpful.Logging.WriteError("PostRequest(string url, string parameters)#3: " + exception3, true);
            }
            return null;
        }

        public static void ProductStatusLog(string productName, uint stepId)
        {
            switch (stepId)
            {
                case 1:
                    if (!(nManager.Helpful.Logging.Status == (productName + " initialized")))
                    {
                        nManager.Helpful.Logging.Status = productName + " initialized";
                        nManager.Helpful.Logging.Write(productName + " initialized");
                        return;
                    }
                    return;

                case 2:
                    if (!(nManager.Helpful.Logging.Status == (productName + " disposed")))
                    {
                        nManager.Helpful.Logging.Status = productName + " disposed";
                        nManager.Helpful.Logging.Write(productName + " disposed");
                        return;
                    }
                    return;

                case 3:
                    if (!(nManager.Helpful.Logging.Status == ("Start " + productName)))
                    {
                        nManager.Helpful.Logging.Status = "Start " + productName;
                        nManager.Helpful.Logging.Write("Start " + productName);
                        return;
                    }
                    return;

                case 4:
                    if (!(nManager.Helpful.Logging.Status == (productName + " started")))
                    {
                        nManager.Helpful.Logging.Status = productName + " started";
                        nManager.Helpful.Logging.Write(productName + " started");
                        return;
                    }
                    return;

                case 5:
                    if (!(nManager.Helpful.Logging.Status == (productName + " failed to start")))
                    {
                        nManager.Helpful.Logging.Status = productName + " failed to start";
                        nManager.Helpful.Logging.Write(productName + " failed to start");
                        return;
                    }
                    return;

                case 6:
                    if (!(nManager.Helpful.Logging.Status == (productName + " stopped")))
                    {
                        nManager.Helpful.Logging.Status = productName + " stopped";
                        nManager.Helpful.Logging.Write(productName + " stopped");
                        return;
                    }
                    return;

                case 7:
                    if (!(nManager.Helpful.Logging.Status == ("Settings of " + productName + " loaded")))
                    {
                        nManager.Helpful.Logging.Status = "Settings of " + productName + " loaded";
                        nManager.Helpful.Logging.Write("Settings of " + productName + " loaded");
                        return;
                    }
                    return;
            }
        }

        public static int Random(int from, int to)
        {
            try
            {
                System.Random random = new System.Random((int) DateTime.Now.Ticks);
                return random.Next(from, to);
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("Random(int from, int to): " + exception, true);
            }
            return 0;
        }

        public static string ReadFile(string filePath, bool writeNewLine = false)
        {
            try
            {
                StreamReader reader = new StreamReader(filePath);
                string str = reader.ReadLine();
                string str2 = "";
                while (str != null)
                {
                    str2 = str2 + str;
                    if (writeNewLine)
                    {
                        str2 = str2 + Environment.NewLine;
                    }
                    str = reader.ReadLine();
                    Application.DoEvents();
                }
                reader.Close();
                return str2;
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("ReadFile(string filePath, bool writeNewLine = false): " + exception, true);
                return "";
            }
        }

        public static string[] ReadFileAllLines(string path)
        {
            try
            {
                StringCollection strings = new StringCollection();
                using (StreamReader reader = new StreamReader(path))
                {
                    string str;
                    while ((str = reader.ReadLine()) != null)
                    {
                        strings.Add(str);
                    }
                }
                string[] array = new string[strings.Count];
                strings.CopyTo(array, 0);
                return array;
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("ReadFileAllLines(string path): " + exception, true);
                return new string[0];
            }
        }

        public static void SafeSleep(int sleepMs)
        {
            Memory.WowMemory.GameFrameUnLock();
            Thread.Sleep(sleepMs);
        }

        public static string SecToHour(int sec)
        {
            try
            {
                string str = (sec / 0xe10) + "H";
                sec -= (sec / 0xe10) * 0xe10;
                if ((sec / 60) < 10)
                {
                    str = string.Concat(new object[] { str, "0", sec / 60, "M" });
                }
                else
                {
                    str = str + (sec / 60) + "M";
                }
                sec -= (sec / 60) * 60;
                if (sec < 10)
                {
                    str = str + "0" + sec;
                }
                else
                {
                    str = str + sec;
                }
                return str;
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("SecToHour(int sec): " + exception, true);
                return "00H00M00";
            }
        }

        public static void ShowMessageBox(string message, string title = "")
        {
            (string.IsNullOrEmpty(title) ? new Thread(() => MessageBox.Show(message)) : new Thread(() => MessageBox.Show(message, title))).Start();
        }

        public static void ShutDownPc()
        {
            try
            {
                Process process = new Process {
                    StartInfo = { FileName = "shutdown.exe", Arguments = " -s -f" }
                };
                process.Start();
                process.Close();
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("ShutDownPc(): " + exception, true);
            }
        }

        public static string[] TextToArrayByLine(string text)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (string str in text.Split(Environment.NewLine.ToCharArray()))
                {
                    if (str.Trim() != "")
                    {
                        list.Add(str);
                    }
                }
                return list.ToArray();
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("TextToArrayByLine(string text): " + exception, true);
                return new string[0];
            }
        }

        public static bool ToBoolean(string value)
        {
            bool flag;
            return (bool.TryParse(value, out flag) && flag);
        }

        public static char ToChar(string value)
        {
            char ch;
            if (!char.TryParse(value.Trim(), out ch))
            {
                return '\0';
            }
            return ch;
        }

        public static int ToInt32(string value)
        {
            int num;
            if (!int.TryParse(value.Trim(), out num))
            {
                return 0;
            }
            return num;
        }

        public static long ToInt64(string value)
        {
            long num;
            if (!long.TryParse(value.Trim(), out num))
            {
                return 0L;
            }
            return num;
        }

        public static float ToSingle(string value)
        {
            float num;
            if (!float.TryParse(value.Trim(), out num))
            {
                return 0f;
            }
            return num;
        }

        public static uint ToUInt32(string value)
        {
            uint num;
            if (!uint.TryParse(value.Trim(), out num))
            {
                return 0;
            }
            return num;
        }

        public static ulong ToUInt64(string value)
        {
            ulong num;
            if (!ulong.TryParse(value.Trim(), out num))
            {
                return 0L;
            }
            return num;
        }

        public static string ToUtf8(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return text;
                }
                byte[] bytes = Encoding.Default.GetBytes(text);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("ToUtf8(string text): " + exception, true);
            }
            return "";
        }

        public static string ToUtf8(byte[] bytes)
        {
            try
            {
                string str = Utf8.GetString(bytes, 0, bytes.Length);
                if (str.IndexOf("\0", StringComparison.Ordinal) != -1)
                {
                    str = str.Remove(str.IndexOf("\0", StringComparison.Ordinal), str.Length - str.IndexOf("\0", StringComparison.Ordinal));
                }
                return str;
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("ToUtf8(byte[] bytes): " + exception, true);
            }
            return "";
        }

        public static void Wait(int milsecToWait)
        {
            try
            {
                int num = GetTickCount() + milsecToWait;
                while (GetTickCount() < num)
                {
                    Application.DoEvents();
                    Thread.Sleep(5);
                }
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("Wait(int milsecToWait): " + exception, true);
            }
        }

        public static void WriteFile(string filePath, string value)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filePath);
                writer.Write(value);
                writer.Close();
            }
            catch (Exception exception)
            {
                nManager.Helpful.Logging.WriteError("WriteFile(string filePath, string value): " + exception, true);
            }
        }

        public static string GetAuthScriptLink
        {
            get
            {
                return (GetWorkingAuthServerAddress + "auth.php");
            }
        }

        public static string GetClientIPAddress
        {
            get
            {
                return GetRequest(GetClientIPScriptLink, "");
            }
        }

        public static string GetClientIPScriptLink
        {
            get
            {
                return (GetWorkingAuthServerAddress + "myIp.php");
            }
        }

        public static string GetCurrentDirectory
        {
            get
            {
                return Application.StartupPath;
            }
        }

        public static string GetMonitoringScriptLink
        {
            get
            {
                return (GetWorkingAuthServerAddress + "isOnline.php");
            }
        }

        public static string GetUpdateScriptLink
        {
            get
            {
                return (GetWorkingAuthServerAddress + "update.php");
            }
        }

        public static string GetWorkingAuthServerAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(_ahiweifuepean) && !CachedAuthServerTimer.IsReady)
                {
                    return _ahiweifuepean;
                }
                foreach (string str in from server in FailOversAddress
                    where GetRequest(server + "isOnline.php", "") == "true"
                    select server)
                {
                    _ahiweifuepean = str;
                    CachedAuthServerTimer.Reset();
                    return str;
                }
                return FailOversAddress[0];
            }
        }

        public static int Times
        {
            get
            {
                return Environment.TickCount;
            }
        }

        public static int TimesSec
        {
            get
            {
                return (Environment.TickCount / 0x3e8);
            }
        }
    }
}

