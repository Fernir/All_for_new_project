namespace nManager.Helpful
{
    using nManager;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class Logging
    {
        private static List<Log> _log = new List<Log>();
        private static string _logFileName = "";
        private static string _status = "";
        private static Thread _worker;
        private static readonly List<Log> LogQueue = new List<Log>();

        public static  event LoggingChangeEventHandler OnChanged;

        public static  event StatusChangeEventHandler OnChangedStatus;

        private static void AddLog()
        {
            while (true)
            {
                try
                {
                    if (LogQueue.Count > 0)
                    {
                        if (_logFileName == "")
                        {
                            NewFile();
                        }
                        Console.WriteLine(LogQueue[0].ToString());
                        _log.Add(LogQueue[0]);
                        if (_log.Count > 100)
                        {
                            _log.RemoveAt(0);
                        }
                        if (!Directory.Exists(Application.StartupPath + @"\Logs"))
                        {
                            Directory.CreateDirectory(Application.StartupPath + @"\Logs");
                        }
                        StreamWriter writer = new StreamWriter(Application.StartupPath + @"\Logs\" + _logFileName, true, Encoding.UTF8);
                        writer.Write("<font color=\"" + ColorTranslator.ToHtml(LogQueue[0].Color) + "\">" + LogQueue[0].ToString().Replace(Environment.NewLine, "<br> ") + "</font><br>");
                        writer.Close();
                        try
                        {
                            LoggingChangeEventArgs args2 = new LoggingChangeEventArgs();
                            Log log = new Log {
                                Color = LogQueue[0].Color,
                                DateTime = LogQueue[0].DateTime,
                                LogType = LogQueue[0].LogType,
                                Text = LogQueue[0].Text
                            };
                            args2.Log = log;
                            LoggingChangeEventArgs e = args2;
                            if (OnChanged != null)
                            {
                                OnChanged(null, e);
                            }
                        }
                        catch
                        {
                        }
                        LogQueue.RemoveAt(0);
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch (Exception exception)
                {
                    WriteError("AddLog(): " + exception, true);
                }
                Thread.Sleep(10);
            }
        }

        public static void NewFile()
        {
            try
            {
                _log = new List<Log>();
                _logFileName = DateTime.Now.ToString("d MMM yyyy HH") + "H" + DateTime.Now.ToString("mm") + ".log.html";
                if (File.Exists(Application.StartupPath + @"\Logs\" + _logFileName))
                {
                    _logFileName = DateTime.Now.ToString("d MMM yyyy HH") + "H" + DateTime.Now.ToString("mm") + " - " + Others.GetRandomString(Others.Random(4, 7)) + ".log.html";
                }
                WriteDebug("Log file created: " + _logFileName);
                Write("Welcome to " + Information.MainTitle + ", if you have any trouble, please upload the FILE of this log from your /Logs/ directory on the community forum.");
            }
            catch (Exception exception)
            {
                WriteError("NewFile(): " + exception, true);
            }
        }

        public static Log ReadLast(LogType logType)
        {
            try
            {
                for (int i = _log.Count - 1; i > 0; i--)
                {
                    if ((_log[i].LogType & logType) == _log[i].LogType)
                    {
                        return _log[i];
                    }
                }
            }
            catch (Exception exception)
            {
                WriteError("ReadLast(LogType logType): " + exception, true);
            }
            return new Log();
        }

        public static string ReadLastString(LogType logType)
        {
            try
            {
                return ReadLast(logType).ToString();
            }
            catch (Exception exception)
            {
                WriteError("ReadLastString(LogType logType): " + exception, true);
            }
            return "";
        }

        public static List<Log> ReadList()
        {
            return _log;
        }

        public static List<Log> ReadList(LogType logType, bool setProcess = false)
        {
            try
            {
                List<Log> list = new List<Log>();
                for (int i = 0; i < _log.Count; i++)
                {
                    Log item = _log[i];
                    if ((item.LogType & logType) == item.LogType)
                    {
                        item.Processed = true;
                        list.Add(item);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                WriteError("ReadList(LogType logType): " + exception, true);
            }
            return new List<Log>();
        }

        public static List<string> ReadListString(LogType logType)
        {
            try
            {
                List<string> list = new List<string>();
                for (int i = 0; i < ReadList(logType, false).Count; i++)
                {
                    Log log = ReadList(logType, 0)[i];
                    if ((log.LogType & logType) == log.LogType)
                    {
                        list.Add(log.ToString());
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                WriteError("ReadListString(LogType logType): " + exception, true);
            }
            return new List<string>();
        }

        public static void Write(Log log)
        {
            try
            {
                LogQueue.Add(log);
                lock (typeof(Logging))
                {
                    if (_worker == null)
                    {
                        Thread thread = new Thread(new ThreadStart(Logging.AddLog)) {
                            Name = "Logging"
                        };
                        _worker = thread;
                        _worker.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                WriteError("Write(Log log): " + exception, true);
            }
        }

        public static void Write(string text)
        {
            Write(text, LogType.S, Color.Black);
        }

        public static void Write(string text, LogType logType, Color color)
        {
            Write(new Log(text, logType, color));
        }

        public static void WriteDebug(string text)
        {
            Write(text, LogType.D, Color.MediumVioletRed);
        }

        public static void WriteError(string text, bool skipThreadAbortExceptionError = true)
        {
            if (!string.IsNullOrEmpty(text) && (!text.Contains("System.Threading.ThreadAbortException") || !skipThreadAbortExceptionError))
            {
                Write(text, LogType.E, Color.Red);
            }
        }

        public static void WriteFight(string text)
        {
            Write(text, LogType.F, Color.Green);
        }

        public static void WriteFileOnly(string text)
        {
            Write(text, LogType.IO, Color.Gray);
        }

        public static void WriteNavigator(string text)
        {
            Write(text, LogType.N, Color.Blue);
        }

        public static void WritePlugin(string text, string pluginName)
        {
            Write(pluginName + ": " + text, LogType.P, Color.DarkOrange);
        }

        public static void WritePluginDebug(string text, string pluginName)
        {
            Write(pluginName + ": " + text, LogType.DP, Color.DarkViolet);
        }

        public static void WritePluginError(string text, string pluginName)
        {
            Write(pluginName + ": " + text, LogType.EP, Color.OrangeRed);
        }

        public static void WriteWhispers(string text)
        {
            Write(text, LogType.W, Color.BlueViolet);
        }

        public static int CountNumberInQueue
        {
            get
            {
                return LogQueue.Count;
            }
        }

        public static List<Log> List
        {
            get
            {
                return _log;
            }
        }

        public static string Status
        {
            get
            {
                return _status;
            }
            set
            {
                try
                {
                    _status = value;
                    StatusChangeEventArgs e = new StatusChangeEventArgs {
                        Status = value
                    };
                    if (OnChangedStatus != null)
                    {
                        OnChangedStatus(Status, e);
                    }
                }
                catch (Exception exception)
                {
                    WriteError("Loggin > Status > Set: " + exception, true);
                }
            }
        }

        public class Log
        {
            public bool Processed;

            public Log()
            {
                this.Text = "";
                this.LogType = nManager.Helpful.Logging.LogType.S;
                this.Color = System.Drawing.Color.Black;
                this.DateTime = System.DateTime.Now;
            }

            public Log(string text, nManager.Helpful.Logging.LogType logType, System.Drawing.Color color)
            {
                try
                {
                    this.Text = text;
                    this.LogType = logType;
                    this.Color = color;
                    this.DateTime = System.DateTime.Now;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Log(string text, LogType logType, Color color): " + exception, true);
                }
            }

            public override string ToString()
            {
                try
                {
                    return string.Concat(new object[] { "[", this.LogType, "] ", this.DateTime.ToString("HH:mm:ss"), " - ", this.Text });
                }
                catch (Exception exception)
                {
                    Logging.WriteError("ToString(): " + exception, true);
                }
                return "";
            }

            public System.Drawing.Color Color { get; set; }

            public System.DateTime DateTime { get; set; }

            public nManager.Helpful.Logging.LogType LogType { get; set; }

            public string Text { get; set; }
        }

        public class LoggingChangeEventArgs : EventArgs
        {
            public nManager.Helpful.Logging.Log Log { get; set; }
        }

        public delegate void LoggingChangeEventHandler(object sender, Logging.LoggingChangeEventArgs e);

        [Flags]
        public enum LogType
        {
            D = 2,
            DP = 0x100,
            E = 4,
            EP = 0x80,
            F = 0x10,
            IO = 0x20,
            N = 8,
            None = 0,
            P = 0x40,
            S = 1,
            W = 0x200
        }

        public class StatusChangeEventArgs : EventArgs
        {
            public string Status { get; set; }
        }

        public delegate void StatusChangeEventHandler(object sender, Logging.StatusChangeEventArgs e);
    }
}

