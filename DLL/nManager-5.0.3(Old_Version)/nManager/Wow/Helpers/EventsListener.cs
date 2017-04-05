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
        private static List<HookedEventInfo> _hookedEvents = new List<HookedEventInfo>();
        private static object _ourLock = new object();
        private static Thread _threadHookEvent = null;
        private static readonly int EventsCount = Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xc054b8);
        private static readonly uint PtrFirstEvent = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xc054bc);
        public static Dictionary<string, uint> WoWEventsType = new Dictionary<string, uint>();

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

        private static int GetEventFireCount(nManager.Wow.Enums.WoWEventsType eventType)
        {
            uint woWEventsTypeValue = GetWoWEventsTypeValue(eventType);
            if (woWEventsTypeValue > EventsCount)
            {
                return 0;
            }
            uint num2 = Memory.WowMemory.Memory.ReadUInt(PtrFirstEvent + (4 * woWEventsTypeValue));
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

        private static uint GetWoWEventsTypeValue(nManager.Wow.Enums.WoWEventsType eventsType)
        {
            try
            {
                lock (WoWEventsType)
                {
                    if (WoWEventsType.Count > 0)
                    {
                        return (WoWEventsType.ContainsKey(eventsType.ToString()) ? WoWEventsType[eventsType.ToString()] : 0);
                    }
                    Logging.WriteDebug("Loading WoWEventsLUA database...");
                    for (uint i = 0; i <= (EventsCount - 1); i++)
                    {
                        uint num2 = Memory.WowMemory.Memory.ReadUInt(PtrFirstEvent + (4 * i));
                        if (num2 > 0)
                        {
                            uint dwAddress = Memory.WowMemory.Memory.ReadUInt(num2 + 0x18);
                            if (dwAddress > 0)
                            {
                                string key = Memory.WowMemory.Memory.ReadUTF8String(dwAddress);
                                if (!(key == "") && !WoWEventsType.ContainsKey(key))
                                {
                                    WoWEventsType.Add(key, i);
                                }
                            }
                        }
                    }
                    Logging.WriteDebug("WoWEventsLUA database loaded.");
                }
                return (WoWEventsType.ContainsKey(eventsType.ToString()) ? WoWEventsType[eventsType.ToString()] : 0);
            }
            catch (Exception exception)
            {
                Logging.WriteDebug("WoWEventsLUA database loading failed.");
                Logging.WriteError("GetWoWEventsTypeValue(WoWEventsType eventsType): " + exception, true);
                WoWEventsType.Clear();
                return 0;
            }
        }

        private static void Hook()
        {
            while (_hookedEvents.Count > 0)
            {
                lock (_ourLock)
                {
                    foreach (HookedEventInfo info in _hookedEvents)
                    {
                        if (info.PreviousCurrentEventFireCount < GetEventFireCount(info.EventType))
                        {
                            Thread thread2 = new Thread(new ThreadStart(info.CallBack)) {
                                Name = "Fire callback for Event: " + info.EventType
                            };
                            thread2.Start();
                            info.PreviousCurrentEventFireCount++;
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        public static void HookEvent(nManager.Wow.Enums.WoWEventsType eventType, Expression<CallBack> method, bool requestFireCount = false, bool ignoreAlreadyDone = false)
        {
            try
            {
                Logging.WriteDebug(string.Concat(new object[] { "Init HookEvent for event: ", eventType, " with CallBack: ", method, " and requestFireCount: ", requestFireCount }));
                if (IsAttached(eventType, method.ToString(), requestFireCount))
                {
                    if (!ignoreAlreadyDone)
                    {
                        Logging.WriteError(string.Concat(new object[] { "The event ", eventType, " with method ", method, " and parameter requestFireCount set to ", requestFireCount, " is already hooked in the exact same way, duplicates of HookEvent is a bad code manner, make sure to UnHook your event when your Stop() your plugin." }), true);
                    }
                }
                else
                {
                    lock (_ourLock)
                    {
                        CallBack callBack = method.Compile();
                        _hookedEvents.Add(new HookedEventInfo(callBack, eventType, GetEventFireCount(eventType), method.ToString(), requestFireCount));
                    }
                    if ((_threadHookEvent == null) || !_threadHookEvent.IsAlive)
                    {
                        Thread thread = new Thread(new ThreadStart(EventsListener.Hook)) {
                            Name = "Hook of Events"
                        };
                        _threadHookEvent = thread;
                        _threadHookEvent.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("HookEvent(WoWEventsType eventType, Expression<CallBack> method, bool requestFireCount = false): " + exception, true);
            }
        }

        private static bool IsAttached(nManager.Wow.Enums.WoWEventsType eventType, string callBack, bool sendsFireCount = false)
        {
            try
            {
                lock (_ourLock)
                {
                    foreach (HookedEventInfo info in _hookedEvents)
                    {
                        if (((info.EventType == eventType) && (info.CallBackName == callBack)) && (info.SendsFireCount == sendsFireCount))
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

        public static void UnHookEvent(nManager.Wow.Enums.WoWEventsType eventType, Expression<CallBack> method, bool requestFireCount = false)
        {
            try
            {
                Logging.WriteDebug(string.Concat(new object[] { "Init UnHookEvent for event: ", eventType, " with CallBack: ", method, " and requestFireCount: ", requestFireCount }));
                lock (_ourLock)
                {
                    HookedEventInfo item = null;
                    foreach (HookedEventInfo info2 in _hookedEvents)
                    {
                        if (((info2.EventType == eventType) && (info2.CallBackName == method.ToString())) && (info2.SendsFireCount == requestFireCount))
                        {
                            item = info2;
                            break;
                        }
                    }
                    if (item != null)
                    {
                        _hookedEvents.Remove(item);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("UnHookEvent(WoWEventsType eventType, string methodName, bool requestFireCount = false): " + exception, true);
            }
        }

        public delegate void CallBack(object context);

        private class HookedEventInfo
        {
            private readonly nManager.Wow.Helpers.EventsListener.CallBack _callBack;
            internal readonly string CallBackName;
            internal readonly WoWEventsType EventType;
            internal int PreviousCurrentEventFireCount = -1;
            internal readonly bool SendsFireCount;

            internal HookedEventInfo(nManager.Wow.Helpers.EventsListener.CallBack callBack, WoWEventsType id, int idUsedLastCount, string callBackName, bool sendsFireCount)
            {
                this._callBack = callBack;
                this.EventType = id;
                this.PreviousCurrentEventFireCount = idUsedLastCount;
                this.CallBackName = callBackName;
                this.SendsFireCount = sendsFireCount;
            }

            internal void CallBack()
            {
                if (this.EventType == WoWEventsType.CHAT_MSG_LOOT)
                {
                    Thread.Sleep(0x7d0);
                }
                if (this.SendsFireCount)
                {
                    this._callBack(EventsListener.GetEventFireCount(this.EventType));
                }
                else
                {
                    this._callBack(null);
                }
            }
        }
    }
}

