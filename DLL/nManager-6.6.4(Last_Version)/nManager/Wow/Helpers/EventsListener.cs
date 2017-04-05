namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class EventsListener
    {
        private static List<Dasakaloa> _jaugiu = new List<Dasakaloa>();
        private static Thread _jeuxajeuwiFesaRu = null;
        private static object _meuxaipehesubi = new object();
        private static readonly int EventsCount = Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xd75fd0);
        private static readonly uint PtrFirstEvent = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xd75fd4);
        public static Dictionary<string, uint> WoWEventsType = new Dictionary<string, uint>();

        private static int Doimiholivatos(nManager.Wow.Enums.WoWEventsType apoasigOhewo)
        {
            uint num = ItajogebiaqIpuatia(apoasigOhewo);
            if (num > EventsCount)
            {
                return 0;
            }
            uint num2 = Memory.WowMemory.Memory.ReadUInt(PtrFirstEvent + (4 * num));
            if (num2 <= 0)
            {
                return 0;
            }
            if (Memory.WowMemory.Memory.ReadUInt(num2 + 0x18) <= 0)
            {
                return 0;
            }
            return Memory.WowMemory.Memory.ReadInt(num2 + 0x48);
        }

        public static void EnumWoWEventsDumper()
        {
            string text = "";
            for (uint i = 0; i <= (EventsCount - 1); i++)
            {
                uint num2 = Memory.WowMemory.Memory.ReadUInt(PtrFirstEvent + (4 * i));
                if (num2 > 0)
                {
                    uint dwAddress = Memory.WowMemory.Memory.ReadUInt(num2 + 0x18);
                    if (dwAddress > 0)
                    {
                        string str2 = Memory.WowMemory.Memory.ReadUTF8String(dwAddress);
                        if (!(str2 == "") && !text.Contains(Environment.NewLine + str2 + " = "))
                        {
                            object obj2 = text;
                            text = string.Concat(new object[] { obj2, str2, ",", Environment.NewLine });
                        }
                    }
                }
            }
            Logging.Write(text);
        }

        private static bool HiheateasiacEpoes(nManager.Wow.Enums.WoWEventsType apoasigOhewo, string teohexeaviohibKofo, bool ecedawibJe = false)
        {
            try
            {
                lock (_meuxaipehesubi)
                {
                    foreach (Dasakaloa dasakaloa in _jaugiu)
                    {
                        if (((dasakaloa.EventType == apoasigOhewo) && (dasakaloa.CallBackName == teohexeaviohibKofo)) && (dasakaloa.SendsFireCount == ecedawibJe))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsAttached(string id, MethodDelegate method): " + exception, true);
            }
            return false;
        }

        public static void HookEvent(nManager.Wow.Enums.WoWEventsType eventType, Expression<CallBack> method, bool requestFireCount = false, bool ignoreAlreadyDone = false)
        {
            try
            {
                Logging.WriteDebug(string.Concat(new object[] { "Init HookEvent for event: ", eventType, " with CallBack: ", method, " and requestFireCount: ", requestFireCount }));
                if (HiheateasiacEpoes(eventType, method.ToString(), requestFireCount))
                {
                    if (!ignoreAlreadyDone)
                    {
                        Logging.WriteError(string.Concat(new object[] { "The event ", eventType, " with method ", method, " and parameter requestFireCount set to ", requestFireCount, " is already hooked in the exact same way, duplicates of HookEvent is a bad code manner, make sure to UnHook your event when your Stop() your plugin." }), true);
                    }
                }
                else
                {
                    lock (_meuxaipehesubi)
                    {
                        CallBack callBack = method.Compile();
                        _jaugiu.Add(new Dasakaloa(callBack, eventType, Doimiholivatos(eventType), method.ToString(), requestFireCount));
                    }
                    if ((_jeuxajeuwiFesaRu == null) || !_jeuxajeuwiFesaRu.IsAlive)
                    {
                        Thread thread = new Thread(new ThreadStart(EventsListener.PiakoIxavei)) {
                            Name = "Hook of Events"
                        };
                        _jeuxajeuwiFesaRu = thread;
                        _jeuxajeuwiFesaRu.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("HookEvent(WoWEventsType eventType, Expression<CallBack> method, bool requestFireCount = false): " + exception, true);
            }
        }

        private static uint ItajogebiaqIpuatia(nManager.Wow.Enums.WoWEventsType upioxai)
        {
            try
            {
                lock (WoWEventsType)
                {
                    if (WoWEventsType.Count > 0)
                    {
                        return (WoWEventsType.ContainsKey(upioxai.ToString()) ? WoWEventsType[upioxai.ToString()] : 0);
                    }
                    Logging.WriteDebug("Loading WoWEventsLUA database...");
                    int num = 0;
                    for (uint i = 0; i <= (EventsCount - 1); i++)
                    {
                        uint num3 = Memory.WowMemory.Memory.ReadUInt(PtrFirstEvent + (4 * i));
                        if (num3 > 0)
                        {
                            uint dwAddress = Memory.WowMemory.Memory.ReadUInt(num3 + 0x18);
                            if (dwAddress > 0)
                            {
                                string key = Memory.WowMemory.Memory.ReadUTF8String(dwAddress);
                                if (!(key == "") && !WoWEventsType.ContainsKey(key))
                                {
                                    WoWEventsType.Add(key, i);
                                    num++;
                                }
                            }
                        }
                    }
                    Logging.WriteDebug("WoWEventsLUA database loaded with " + num + ".");
                }
                return (WoWEventsType.ContainsKey(upioxai.ToString()) ? WoWEventsType[upioxai.ToString()] : 0);
            }
            catch (Exception exception)
            {
                Logging.WriteDebug("WoWEventsLUA database loading failed.");
                Logging.WriteError("GetWoWEventsTypeValue(WoWEventsType eventsType): " + exception, true);
                WoWEventsType.Clear();
                return 0;
            }
        }

        private static void PiakoIxavei()
        {
            while (_jaugiu.Count > 0)
            {
                lock (_meuxaipehesubi)
                {
                    foreach (Dasakaloa dasakaloa in _jaugiu)
                    {
                        if (dasakaloa._ribapuatuis < Doimiholivatos(dasakaloa.EventType))
                        {
                            Thread thread2 = new Thread(new ThreadStart(dasakaloa.RenoimofoajFiriutuh)) {
                                Name = "Fire callback for Event: " + dasakaloa.EventType
                            };
                            thread2.Start();
                            dasakaloa._ribapuatuis++;
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        public static void UnHookEvent(nManager.Wow.Enums.WoWEventsType eventType, Expression<CallBack> method, bool requestFireCount = false)
        {
            try
            {
                Logging.WriteDebug(string.Concat(new object[] { "Init UnHookEvent for event: ", eventType, " with CallBack: ", method, " and requestFireCount: ", requestFireCount }));
                lock (_meuxaipehesubi)
                {
                    Dasakaloa item = null;
                    foreach (Dasakaloa dasakaloa2 in _jaugiu)
                    {
                        if (((dasakaloa2.EventType == eventType) && (dasakaloa2.CallBackName == method.ToString())) && (dasakaloa2.SendsFireCount == requestFireCount))
                        {
                            item = dasakaloa2;
                            break;
                        }
                    }
                    if (item != null)
                    {
                        _jaugiu.Remove(item);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("UnHookEvent(WoWEventsType eventType, string methodName, bool requestFireCount = false): " + exception, true);
            }
        }

        public delegate void CallBack(object context);

        private class Dasakaloa
        {
            private readonly EventsListener.CallBack _callBack;
            internal int _ribapuatuis = -1;
            internal readonly string CallBackName;
            internal readonly WoWEventsType EventType;
            internal readonly bool SendsFireCount;

            internal Dasakaloa(EventsListener.CallBack callBack, WoWEventsType id, int idUsedLastCount, string callBackName, bool sendsFireCount)
            {
                this._callBack = callBack;
                this.EventType = id;
                this._ribapuatuis = idUsedLastCount;
                this.CallBackName = callBackName;
                this.SendsFireCount = sendsFireCount;
            }

            internal void RenoimofoajFiriutuh()
            {
                switch (this.EventType)
                {
                    case WoWEventsType.CHAT_MSG_LOOT:
                        Thread.Sleep(0x7d0);
                        break;

                    case WoWEventsType.QUEST_AUTOCOMPLETE:
                        Thread.Sleep(0x7d0);
                        break;
                }
                if (this.SendsFireCount)
                {
                    this._callBack(EventsListener.Doimiholivatos(this.EventType));
                }
                else
                {
                    this._callBack(null);
                }
            }
        }
    }
}

