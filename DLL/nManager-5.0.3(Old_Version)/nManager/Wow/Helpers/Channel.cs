namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using System;
    using System.Runtime.InteropServices;

    public class Channel
    {
        public int CurrentMsg;

        public Channel()
        {
            try
            {
                this.GetCurrentMsg();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Channel(): " + exception, true);
            }
        }

        private string getChannel(int channel)
        {
            try
            {
                switch (channel)
                {
                    case 0:
                        return "[Addon]";

                    case 1:
                        return "[Say]";

                    case 2:
                        return "[Party]";

                    case 3:
                        return "[Raid]";

                    case 4:
                        return "[Guild]";

                    case 5:
                        return "[Officers]";

                    case 6:
                        return "[Yell]";

                    case 7:
                        return "[Whisper]";

                    case 8:
                        return "[Monster Whisper]";

                    case 9:
                        return "[To]";

                    case 10:
                        return "[Emote]";

                    case 0x11:
                        return "[General]";

                    case 0x1b:
                        return "[Loot]";

                    case 0x33:
                        return "[Real Id Whisper]";

                    case 0x34:
                        return "[Real Id To]";
                }
                return ("[Channel " + channel + "]");
            }
            catch (Exception exception)
            {
                Logging.WriteError("getChannel(int Channel): " + exception, true);
                return "";
            }
        }

        private void GetCurrentMsg()
        {
            try
            {
                this.CurrentMsg = Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xe01894);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetCurrentMsg(): " + exception, true);
            }
        }

        public string ReadAllChannel()
        {
            try
            {
                Message message = this.ReadMsg();
                if (message.Msg != null)
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.getChannel(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadAllChannel(): " + exception, true);
            }
            return "";
        }

        public string ReadGuildChannel()
        {
            try
            {
                Message message = this.ReadMsg();
                if ((message.Msg != null) && (message.Channel == 4))
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.getChannel(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadGuildChannel(): " + exception, true);
            }
            return "";
        }

        public static string ReadLastMsg()
        {
            try
            {
                return Memory.WowMemory.Memory.ReadUTF8String(((Memory.WowProcess.WowModule + 0xda7518) + 0x65) + ((uint) (0x17e8 * Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xe01894))));
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadLastMsg(): " + exception, true);
            }
            return "";
        }

        public string ReadLootChannel()
        {
            try
            {
                Message message = this.ReadMsg();
                if ((message.Msg != null) && (message.Channel == 0x1b))
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.getChannel(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadWhisperChannel(): " + exception, true);
            }
            return "";
        }

        private Message ReadMsg()
        {
            try
            {
                if (this.CurrentMsg > 0x3b)
                {
                    this.CurrentMsg = 0;
                }
                if ((this.CurrentMsg >= Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xe01894)) && ((this.CurrentMsg - Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xe01894)) <= 2))
                {
                    return new Message();
                }
                string str = Memory.WowMemory.Memory.ReadUTF8String(((Memory.WowProcess.WowModule + 0xda7518) + 0x65) + ((uint) (0x17e8 * this.CurrentMsg)));
                Message message = new Message();
                if (str != "")
                {
                    this.CurrentMsg++;
                    message.Channel = Others.ToInt32(this.stringBetween(str, "Type: [", "]"));
                    message.Nickname = Others.ToUtf8(this.stringBetween(str, "Name: [", "]"));
                    message.Msg = Others.ToUtf8(this.stringBetween(str, "Text: [", "]"));
                }
                return message;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Chat > ReadMsg(): " + exception, true);
                return new Message();
            }
        }

        public string ReadSayChannel()
        {
            try
            {
                Message message = this.ReadMsg();
                if ((message.Msg != null) && (message.Channel == 1))
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.getChannel(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadSayChannel(): " + exception, true);
            }
            return "";
        }

        public string ReadWhisperAndRealIdChannel()
        {
            try
            {
                Message message = this.ReadMsg();
                if ((message.Msg != null) && ((message.Channel == 7) || (message.Channel == 0x33)))
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.getChannel(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadWhisperChannel(): " + exception, true);
            }
            return "";
        }

        public string ReadWhisperChannel()
        {
            try
            {
                Message message = this.ReadMsg();
                if ((message.Msg != null) && (message.Channel == 7))
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.getChannel(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadWhisperChannel(): " + exception, true);
            }
            return "";
        }

        private string stringBetween(string str, string begin, string end)
        {
            try
            {
                int startIndex = str.IndexOf(begin, StringComparison.Ordinal) + begin.Length;
                int num2 = str.IndexOf(end, startIndex, StringComparison.Ordinal);
                if ((num2 - startIndex) <= 0)
                {
                    return "";
                }
                return str.Substring(startIndex, num2 - startIndex);
            }
            catch (Exception exception)
            {
                Logging.WriteError("string stringBetween(string chaine, string debut, string fin): " + exception, true);
                return "";
            }
        }

        public int GetCurrentMsgInWow
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xe01894);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetCurrentMsgInWow: " + exception, true);
                }
                return 0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public int Channel;
            public string Msg;
            public string Nickname;
        }
    }
}

