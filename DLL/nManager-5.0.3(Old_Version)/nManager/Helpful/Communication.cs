namespace nManager.Helpful
{
    using nManager;
    using nManager.Wow.Bot.States;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Communication
    {
        private static nManager.Helpful.Timer _cleanupTimer;
        private static List<int> _currentQuestList;
        private static uint _eventSerialNumber;
        private static List<MimesisHelpers.MimesisEvent> _globalList;
        private static Thread _listenThread;
        private static object _myLock = new object();
        private static MountCapacity _myMountStatus = MountCapacity.Feet;
        private static TcpListener _tcpListener;
        private static uint RollId = 0;

        public static void EventMount()
        {
            _eventSerialNumber++;
            MimesisHelpers.MimesisEvent item = new MimesisHelpers.MimesisEvent {
                SerialNumber = _eventSerialNumber,
                eType = MimesisHelpers.eventType.mount,
                EventValue1 = (int) _myMountStatus
            };
            lock (_myLock)
            {
                _globalList.Add(item);
            }
            _cleanupTimer.Reset();
        }

        public static void EventQuestAccepted()
        {
            WoWObject objectByGuid = nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(nManager.Wow.ObjectManager.ObjectManager.Me.Target);
            if ((objectByGuid != null) && objectByGuid.IsValid)
            {
                WoWUnit unit = new WoWUnit(objectByGuid.GetBaseAddress);
                if (unit.IsValid)
                {
                    _eventSerialNumber++;
                    MimesisHelpers.MimesisEvent item = new MimesisHelpers.MimesisEvent {
                        SerialNumber = _eventSerialNumber,
                        eType = MimesisHelpers.eventType.pickupQuest,
                        EventValue1 = unit.Entry
                    };
                    foreach (int num in Quest.GetLogQuestId())
                    {
                        if (!_currentQuestList.Contains(num))
                        {
                            item.EventValue2 = num;
                            item.EventString1 = Quest.GetLogQuestTitle(num);
                            break;
                        }
                    }
                    lock (_myLock)
                    {
                        _globalList.Add(item);
                    }
                    _currentQuestList.Add(item.EventValue2);
                    _cleanupTimer.Reset();
                }
            }
        }

        public static void EventQuestFinished()
        {
            WoWObject objectByGuid = nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(nManager.Wow.ObjectManager.ObjectManager.Me.Target);
            if ((objectByGuid != null) && objectByGuid.IsValid)
            {
                WoWUnit unit = new WoWUnit(objectByGuid.GetBaseAddress);
                if (unit.IsValid)
                {
                    int item = 0;
                    List<int> logQuestId = Quest.GetLogQuestId();
                    foreach (int num2 in _currentQuestList)
                    {
                        if (!logQuestId.Contains(num2))
                        {
                            item = num2;
                            break;
                        }
                    }
                    if (item != 0)
                    {
                        _eventSerialNumber++;
                        MimesisHelpers.MimesisEvent event2 = new MimesisHelpers.MimesisEvent {
                            SerialNumber = _eventSerialNumber,
                            eType = MimesisHelpers.eventType.turninQuest,
                            EventValue1 = unit.Entry,
                            EventValue2 = item
                        };
                        lock (_myLock)
                        {
                            _globalList.Add(event2);
                        }
                        _currentQuestList.Remove(item);
                        _cleanupTimer.Reset();
                    }
                }
            }
        }

        public static void EventTaxi(object sender, EventArgs args)
        {
            Travel.TaxiEventArgs args2 = new Travel.TaxiEventArgs();
            if (args is Travel.TaxiEventArgs)
            {
                args2 = args as Travel.TaxiEventArgs;
            }
            _eventSerialNumber++;
            MimesisHelpers.MimesisEvent item = new MimesisHelpers.MimesisEvent {
                SerialNumber = _eventSerialNumber,
                eType = MimesisHelpers.eventType.taxi,
                EventValue1 = args2.From,
                EventValue2 = args2.To
            };
            lock (_myLock)
            {
                _globalList.Add(item);
            }
            _cleanupTimer.Reset();
        }

        public static void Listen()
        {
            Shutdown(0x198f);
            int port = nManagerSetting.CurrentSetting.BroadcastingPort;
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _listenThread = new Thread(() => ListenForClients(port));
            _listenThread.Start();
            MountTask.GetMountCapacity();
            Travel.AutomaticallyTookTaxi += new EventHandler(Communication.EventTaxi);
        }

        private static void ListenForClients(int port)
        {
            if ((_listenThread != null) && _listenThread.IsAlive)
            {
                StartListenOnPort(port);
                while (((_listenThread != null) && _listenThread.IsAlive) && ((_tcpListener != null) && (_tcpListener.Server != null)))
                {
                    if (_tcpListener.Pending())
                    {
                        TcpClient parameter = _tcpListener.AcceptTcpClient();
                        Socket client = parameter.Client;
                        nManager.Helpful.Logging.Write("Bot with address " + IPAddress.Parse(((IPEndPoint) client.RemoteEndPoint).Address.ToString()) + " has connected.");
                        ClientThread thread = new ClientThread();
                        new Thread(new ParameterizedThreadStart(thread.HandleClientComm)).Start(parameter);
                    }
                    else
                    {
                        Thread.Sleep(200);
                        if (_cleanupTimer.IsReady)
                        {
                            lock (_myLock)
                            {
                                if (_globalList.Count > 0)
                                {
                                    MimesisHelpers.MimesisEvent item = _globalList[0];
                                    _globalList.Remove(item);
                                }
                            }
                            _cleanupTimer.Reset();
                        }
                        bool flag2 = false;
                        if (MountTask.OnGroundMount())
                        {
                            if (_myMountStatus != MountCapacity.Ground)
                            {
                                flag2 = true;
                            }
                            _myMountStatus = MountCapacity.Ground;
                        }
                        else if (MountTask.OnFlyMount())
                        {
                            if (_myMountStatus != MountCapacity.Fly)
                            {
                                flag2 = true;
                            }
                            _myMountStatus = MountCapacity.Fly;
                        }
                        else if (MountTask.OnAquaticMount())
                        {
                            if (_myMountStatus != MountCapacity.Swimm)
                            {
                                flag2 = true;
                            }
                            _myMountStatus = MountCapacity.Swimm;
                        }
                        else if (_myMountStatus != MountCapacity.Feet)
                        {
                            flag2 = true;
                            _myMountStatus = MountCapacity.Feet;
                        }
                        if (flag2)
                        {
                            EventMount();
                        }
                    }
                }
            }
        }

        public static void Shutdown(int port = 0x198f)
        {
            bool flag = false;
            if ((_listenThread != null) && _listenThread.IsAlive)
            {
                _listenThread.Abort();
                _listenThread = null;
                flag = true;
            }
            if ((_tcpListener != null) && (_tcpListener.Server != null))
            {
                _tcpListener.Stop();
                _tcpListener = null;
                flag = true;
            }
            if (flag)
            {
                nManager.Helpful.Logging.Write("This TheNoobBot session is no longer broadcasting its position and actions on port " + port + " for others TheNoobBot sessions with Mimesis started.");
                EventsListener.UnHookEvent(WoWEventsType.QUEST_FINISHED, callback => EventQuestFinished(), false);
                EventsListener.UnHookEvent(WoWEventsType.QUEST_ACCEPTED, callback => EventQuestAccepted(), false);
            }
        }

        public static void StartListenOnPort(int port)
        {
            try
            {
                _tcpListener.Start();
                nManager.Helpful.Logging.Write("This TheNoobBot session is now broadcasting its position and actions on port " + port + " for others TheNoobBot sessions with Mimesis started.");
                EventsListener.HookEvent(WoWEventsType.QUEST_FINISHED, callback => EventQuestFinished(), false, false);
                EventsListener.HookEvent(WoWEventsType.QUEST_ACCEPTED, callback => EventQuestAccepted(), false, false);
                _eventSerialNumber = 0;
                _currentQuestList = Quest.GetLogQuestId();
                _globalList = new List<MimesisHelpers.MimesisEvent>();
                _cleanupTimer = new nManager.Helpful.Timer(5000.0);
            }
            catch (SocketException)
            {
                Random random = new Random();
                int num = random.Next(0x400, 0x10000);
                while (num == port)
                {
                    random.Next(0x400, 0x10000);
                }
                nManager.Helpful.Logging.WriteError(string.Concat(new object[] { "Mimesis Broadcaster cannot listen on port ", port, ", another application is already using this port, trying to use ", num, " instead." }), true);
                _tcpListener = new TcpListener(IPAddress.Any, num);
                StartListenOnPort(num);
            }
        }

        private class ClientThread
        {
            public void HandleClientComm(object client)
            {
                if (((Communication._listenThread != null) && Communication._listenThread.IsAlive) && ((Communication._tcpListener != null) && (Communication._tcpListener.Server != null)))
                {
                    TcpClient client2 = (TcpClient) client;
                    NetworkStream stream = client2.GetStream();
                    byte[] buffer = new byte[2];
                    byte[] buffer2 = new byte[0];
                    List<MimesisHelpers.MimesisEvent> list = new List<MimesisHelpers.MimesisEvent>();
                    uint num2 = 0;
                    while (((Communication._listenThread != null) && Communication._listenThread.IsAlive) && ((Communication._tcpListener != null) && (Communication._tcpListener.Server != null)))
                    {
                        byte[] buffer3;
                        byte num4;
                        int num = 0;
                        try
                        {
                            if (stream.DataAvailable)
                            {
                                num = stream.Read(buffer, 0, 2);
                                int count = buffer[1];
                                if (count > 0)
                                {
                                    buffer2 = new byte[count];
                                    num += stream.Read(buffer2, 0, count);
                                }
                            }
                        }
                        catch
                        {
                            nManager.Helpful.Logging.Write("Client connection lost!");
                            break;
                        }
                        if (num <= 0)
                        {
                            continue;
                        }
                        switch (((MimesisHelpers.opCodes) buffer[0]))
                        {
                            case MimesisHelpers.opCodes.QueryPosition:
                            {
                                float[] array = nManager.Wow.ObjectManager.ObjectManager.Me.Position.Array;
                                buffer3 = new byte[13];
                                Buffer.BlockCopy(array, 0, buffer3, 0, 12);
                                num4 = 0;
                                if (!(nManager.Wow.ObjectManager.ObjectManager.Me.Position.Type == "Flying"))
                                {
                                    break;
                                }
                                num4 = 2;
                                goto Label_0142;
                            }
                            case MimesisHelpers.opCodes.QueryEvent:
                            {
                                buffer[0] = 0x15;
                                if (list.Count <= 0)
                                {
                                    goto Label_025A;
                                }
                                MimesisHelpers.MimesisEvent str = list[0];
                                byte[] buffer5 = MimesisHelpers.StructToBytes(str);
                                buffer[1] = (byte) buffer5.Length;
                                nManager.Helpful.Logging.WriteDebug(string.Concat(new object[] { "Sending to client event ", str.eType, " for quest ", str.EventValue2 }));
                                stream.Write(buffer, 0, 2);
                                stream.Write(buffer5, 0, buffer5.Length);
                                list.Remove(str);
                                goto Label_02BF;
                            }
                            case MimesisHelpers.opCodes.QueryGuid:
                            {
                                byte[] buffer4 = MimesisHelpers.StructToBytes(nManager.Wow.ObjectManager.ObjectManager.Me.Guid);
                                buffer[0] = 0x1f;
                                buffer[1] = (byte) buffer4.Length;
                                stream.Write(buffer, 0, 2);
                                stream.Write(buffer4, 0, buffer4.Length);
                                goto Label_02BF;
                            }
                            case MimesisHelpers.opCodes.RequestGrouping:
                                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsHomePartyLeader)
                                {
                                    Communication.RollId = 0;
                                }
                                Lua.LuaDoString("InviteUnit(\"" + MimesisHelpers.BytesToString(buffer2) + "\")", false, true);
                                buffer[0] = 0x33;
                                buffer[1] = 4;
                                stream.Write(buffer, 0, 2);
                                stream.Write(BitConverter.GetBytes(Communication.RollId), 0, 4);
                                goto Label_02BF;

                            case MimesisHelpers.opCodes.Disconnect:
                                client2.Close();
                                nManager.Helpful.Logging.Write("Client diconnected");
                                return;

                            default:
                                goto Label_02BF;
                        }
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.Type == "Swimming")
                        {
                            num4 = 1;
                        }
                    Label_0142:
                        buffer3[12] = num4;
                        buffer[0] = 11;
                        buffer[1] = (byte) buffer3.Length;
                        stream.Write(buffer, 0, 2);
                        stream.Write(buffer3, 0, buffer3.Length);
                        goto Label_02BF;
                    Label_025A:
                        buffer[1] = 0;
                        stream.Write(buffer, 0, 2);
                    Label_02BF:
                        stream.Flush();
                        Thread.Sleep(250);
                        uint serialNumber = 0;
                        lock (Communication._myLock)
                        {
                            foreach (MimesisHelpers.MimesisEvent event3 in Communication._globalList)
                            {
                                if (event3.SerialNumber > num2)
                                {
                                    list.Add(event3);
                                    if (event3.SerialNumber > serialNumber)
                                    {
                                        serialNumber = event3.SerialNumber;
                                    }
                                }
                            }
                        }
                        num2 = serialNumber;
                    }
                    stream.Dispose();
                    client2.Close();
                }
            }
        }
    }
}

